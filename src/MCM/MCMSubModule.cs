using Bannerlord.BUTR.Shared.Helpers;
using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Formats;
using MCM.Abstractions.Settings.Properties;
using MCM.Extensions;

using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TaleWorlds.MountAndBlade;

namespace MCM
{
    public sealed class MCMSubModule : MBSubModuleBase
    {
        private const string SWarningTitle =
@"{=dzeWx4xSfR}Warning from MCM!";
        private const string SErrorHarmonyNotFound =
@"{=EEVJa5azpB}Bannerlord.Harmony module was not found!";
        private const string SErrorUIExtenderExNotFound =
@"{=YjsGP3mUaj}Bannerlord.UIExtenderEx module was not found!";
        private const string SErrorButterLibNotFound =
@"{=5EDzm7u4mS}Bannerlord.ButterLib module was not found!";
        private const string SErrorOfficialModulesLoadedBeforeMCM =
@"{=BccWuuSR6a}MCM is loaded after the official modules!
Make sure MCM is loaded before them!";
        private const string SErrorOfficialModules =
@"{=JP23gY34Gm}The following modules were loaded before MCM:";
        private const string SMessageContinue =
@"{=eXs6FLm5DP}It's strongly recommended to terminate the game now. Do you wish to terminate it?";

        public static MCMSubModule? Instance { get; private set; }

        private bool ServiceRegistrationWasCalled { get; set; }

        public MCMSubModule()
        {
            Instance = this;

            CheckLoadOrder();
        }

        public void OnServiceRegistration()
        {
            ServiceRegistrationWasCalled = true;

            if (this.GetServices() is { } services)
            {
                services.AddSettingsFormat<MemorySettingsFormat>();
                services.AddSettingsPropertyDiscoverer<NoneSettingsPropertyDiscoverer>();
            }
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            if (!ServiceRegistrationWasCalled)
                OnServiceRegistration();
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

            Instance = null;
        }

        private static void CheckLoadOrder()
        {
            var loadedModules = Bannerlord.ButterLib.Common.Helpers.ModuleInfoHelper.GetExtendedLoadedModules();
            if (loadedModules.Count == 0) return;

            var sb = new StringBuilder();

            var harmonyModule = loadedModules.SingleOrDefault(x => x.Id == "Bannerlord.Harmony");
            var harmonyModuleIndex = harmonyModule is not null ? loadedModules.IndexOf(harmonyModule) : -1;
            if (harmonyModuleIndex == -1)
            {
                if (sb.Length != 0) sb.AppendLine();
                sb.AppendLine(TextObjectHelper.Create(SErrorHarmonyNotFound).ToString());
            }

            var butterLibModule = loadedModules.SingleOrDefault(x => x.Id == "Bannerlord.ButterLib");
            var butterLibModuleIndex = butterLibModule is not null ? loadedModules.IndexOf(butterLibModule) : -1;
            if (butterLibModuleIndex == -1)
            {
                if (sb.Length != 0) sb.AppendLine();
                sb.AppendLine(TextObjectHelper.Create(SErrorButterLibNotFound).ToString());
            }

            var uiExtenderExModule = loadedModules.SingleOrDefault(x => x.Id == "Bannerlord.UIExtenderEx");
            var uiExtenderExIndex = uiExtenderExModule is not null ? loadedModules.IndexOf(uiExtenderExModule) : -1;
            if (uiExtenderExIndex == -1)
            {
                if (sb.Length != 0) sb.AppendLine();
                sb.AppendLine(TextObjectHelper.Create(SErrorUIExtenderExNotFound).ToString());
            }

            var mcmModule = loadedModules.SingleOrDefault(x => x.Id == "Bannerlord.MBOptionScreen");
            var mcmIndex = mcmModule is not null ? loadedModules.IndexOf(mcmModule) : -1;
            var officialModules = loadedModules.Where(x => x.IsOfficial).Select(x => (Module: x, Index: loadedModules.IndexOf(x)));
            var modulesLoadedBefore = officialModules.Where(tuple => tuple.Index < mcmIndex).ToList();
            if (modulesLoadedBefore.Count > 0)
            {
                if (sb.Length != 0) sb.AppendLine();
                sb.AppendLine(TextObjectHelper.Create(SErrorOfficialModulesLoadedBeforeMCM).ToString());
                sb.AppendLine(TextObjectHelper.Create(SErrorOfficialModules).ToString());
                foreach (var (module, _) in modulesLoadedBefore)
                    sb.AppendLine(module.Id);
            }

            if (sb.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine(TextObjectHelper.Create(SMessageContinue).ToString());

                switch (MessageBox.Show(sb.ToString(), TextObjectHelper.Create(SWarningTitle).ToString(), MessageBoxButtons.YesNo))
                {
                    case DialogResult.Yes:
                        Environment.Exit(1);
                        break;
                }
            }
        }
    }
}