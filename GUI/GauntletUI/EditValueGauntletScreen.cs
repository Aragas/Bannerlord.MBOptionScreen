using ModLib.GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace ModLib.GUI.GauntletUI
{
    public class EditValueGauntletScreen : ScreenBase
    {
        private GauntletLayer gauntletLayer;
        private GauntletMovie movie;
        private EditValueVM vm;
        private SettingProperty settingProperty = null;

        public EditValueGauntletScreen(SettingProperty settingProperty)
        {
            this.settingProperty = settingProperty;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            gauntletLayer = new GauntletLayer(4500);
            gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
            ScreenManager.TrySetFocus(gauntletLayer);
            vm = new EditValueVM(settingProperty);
            AddLayer(gauntletLayer);
            movie = gauntletLayer.LoadMovie("EditValueView", vm);
        }

        protected override void OnFrameTick(float dt)
        {
            base.OnFrameTick(dt);
            // || gauntletLayer.Input.IsGameKeyReleased(34)
            if (gauntletLayer.Input.IsHotKeyReleased("Exit"))
            {
                vm?.ExecuteCancel();
            }
        }

        protected override void OnFinalize()
        {
            base.OnFinalize();
            RemoveLayer(gauntletLayer);
            gauntletLayer.ReleaseMovie(movie);
            gauntletLayer = null;
            movie = null;
            vm.SettingProperty = null;
            vm = null;
        }
    }
}
