using MBOptionScreen.GUI.v1b.ViewModels;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace MBOptionScreen.GUI.v1b.GauntletUI
{
    public class EditValueGauntletScreen : ScreenBase
    {
        private readonly SettingPropertyVM _settingProperty;
        private GauntletLayer _gauntletLayer;
        private GauntletMovie _gauntletMovie;
        private EditValueVM _dataSource;

        public EditValueGauntletScreen(SettingPropertyVM settingProperty)
        {
            _settingProperty = settingProperty;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _dataSource = new EditValueVM(_settingProperty);
            _gauntletLayer = new GauntletLayer(4000, "GauntletLayer");
            _gauntletMovie = _gauntletLayer.LoadMovie("EditValueView_v1b", _dataSource);
            _gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("ChatLogHotKeyCategory"));
            _gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            _gauntletLayer.IsFocusLayer = true;
            AddLayer(_gauntletLayer);
            ScreenManager.TrySetFocus(_gauntletLayer);
        }

        protected override void OnFrameTick(float dt)
        {
            base.OnFrameTick(dt);
            // || gauntletLayer.Input.IsGameKeyReleased(34)
            if (_gauntletLayer.Input.IsHotKeyReleased("Exit"))
            {
                _dataSource?.ExecuteCancel();
            }
            else if (_gauntletLayer.Input.IsHotKeyReleased("FinalizeChat"))
            {
                _dataSource?.ExecuteDone();
            }
        }

        protected override void OnFinalize()
        {
            base.OnFinalize();
            RemoveLayer(_gauntletLayer);
            _gauntletLayer.ReleaseMovie(_gauntletMovie);
            _gauntletLayer = null!;
            _gauntletMovie = null!;
            _dataSource.SettingProperty = null!;
            _dataSource = null!;
        }
    }
}
