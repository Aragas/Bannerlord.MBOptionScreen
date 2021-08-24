extern alias v1;
extern alias v2;
extern alias v4;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using MCM.Adapter.MBO.Settings.Containers;
using MCM.Adapter.MBO.Settings.Properties;
using MCM.Adapter.MBO.Settings.Providers;

using System.Reflection;

using TaleWorlds.MountAndBlade;

using v1::MBOptionScreen;
using v1::MBOptionScreen.State;

using v4::BUTR.DependencyInjection.Extensions;
using v4::MCM.Extensions;

using MBOv2SettingsBase = v2::MBOptionScreen.Settings.SettingsBase;

namespace MCM.Adapter.MBO
{
    public sealed class MCMImplementationMBOSubModule : MBSubModuleBase
    {
        private static readonly AccessTools.FieldRef<SharedStateObject>? MBOv1SharedStateObject =
            AccessTools2.StaticFieldRefAccess<SharedStateObject>(typeof(MBOptionScreenSubModule), "SharedStateObject");

        private static readonly PropertyInfo? MBOv2SettingsProvider =
            AccessTools2.Property(typeof(MBOv2SettingsBase).Assembly.GetType("MBOptionScreen.Settings.SettingsDatabase"), "MBOptionScreenSettingsProvider");

        private bool ServiceRegistrationWasCalled { get; set; }

        public void OnServiceRegistration()
        {
            ServiceRegistrationWasCalled = true;

            if (this.GetServiceContainer() is { } services)
            {
                services.AddSettingsContainer<MBOv1GlobalSettingsContainer>();
                services.AddSettingsContainer<MBOv2GlobalSettingsContainer>();
                services.AddSettingsPropertyDiscoverer<MBOv1SettingsPropertyDiscoverer>();
                services.AddSettingsPropertyDiscoverer<MBOv2SettingsPropertyDiscoverer>();
            }
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            if (!ServiceRegistrationWasCalled)
                OnServiceRegistration();

            if (MBOv1SharedStateObject is not null)
                MBOv1SharedStateObject() = new SharedStateObject(new MBOv1SettingsProvider(), null!, null!);

            if (MBOv2SettingsProvider is not null)
                MBOv2SettingsProvider.SetValue(null, new MBOv2SettingsProvider());
        }
    }
}