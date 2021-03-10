using Bannerlord.ButterLib.Common.Helpers;

using MCM.UI.GUI.ViewModels;

using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace MCM.UI.GUI.GauntletUI
{
    internal sealed class EditValueGauntletScreen : ScreenBase
    {
        private delegate void ReleaseMovieDelegate(object movie);

        private readonly SettingsPropertyVM _settingProperty;
        private GauntletLayer _gauntletLayer = default!;
        private GauntletMovie _gauntletMovie = default!;
        private EditValueVM _dataSource = default!;
        private ReleaseMovieDelegate? _releaseMovie;

        public EditValueGauntletScreen(SettingsPropertyVM settingProperty)
        {
            _settingProperty = settingProperty;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _dataSource = new EditValueVM(_settingProperty);
            _gauntletLayer = new GauntletLayer(4000, "GauntletLayer");
            _releaseMovie = AccessTools2.GetDelegate<ReleaseMovieDelegate>(typeof(GauntletLayer), "ReleaseMovie");
            _gauntletMovie = _gauntletLayer.LoadMovie("EditValueView_MCM", _dataSource);
            _gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("ChatLogHotKeyCategory"));
            _gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            _gauntletLayer.IsFocusLayer = true;
            AddLayer(_gauntletLayer);
            ScreenManager.TrySetFocus(_gauntletLayer);
        }

        protected override void OnFrameTick(float dt)
        {
            base.OnFrameTick(dt);
            LoadingWindow.DisableGlobalLoadingWindow();
            // || gauntletLayer.Input.IsGameKeyReleased(34)
            if (_gauntletLayer.Input.IsHotKeyReleased("Exit"))
            {
                _dataSource.ExecuteCancel();
            }
            else if (_gauntletLayer.Input.IsHotKeyReleased("FinalizeChat"))
            {
                _dataSource.ExecuteDone();
            }
        }

        protected override void OnFinalize()
        {
            base.OnFinalize();
            RemoveLayer(_gauntletLayer);
            _releaseMovie?.Invoke(_gauntletMovie);
            _gauntletLayer = null!;
            _gauntletMovie = null!;
            _dataSource.SettingProperty = null!;
            _dataSource = null!;
        }
    }
}
