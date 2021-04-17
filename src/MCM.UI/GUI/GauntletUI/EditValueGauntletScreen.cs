using HarmonyLib.BUTR.Extensions;

using MCM.UI.GUI.ViewModels;

using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace MCM.UI.GUI.GauntletUI
{
    internal sealed class EditValueGauntletScreen : ScreenBase
    {
        private delegate object LoadMovieDelegate(object instance, string movieName, ViewModel dataSource);
        private delegate void ReleaseMovieDelegate(object instance, object movie);

        private static readonly LoadMovieDelegate? LoadMovie =
            AccessTools2.GetDelegateObjectInstance<LoadMovieDelegate>(typeof(GauntletLayer), "LoadMovie");
        private static readonly ReleaseMovieDelegate? ReleaseMovie =
            AccessTools2.GetDelegateObjectInstance<ReleaseMovieDelegate>(typeof(GauntletLayer), "ReleaseMovie");

        private readonly SettingsPropertyVM _settingProperty;
        private GauntletLayer _gauntletLayer = default!;
        private object? _gauntletMovie;
        private EditValueVM _dataSource = default!;

        public EditValueGauntletScreen(SettingsPropertyVM settingProperty)
        {
            _settingProperty = settingProperty;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _dataSource = new EditValueVM(_settingProperty);
            _gauntletLayer = new GauntletLayer(4000, "GauntletLayer");
            _gauntletMovie = LoadMovie is not null ? LoadMovie(_gauntletLayer, "EditValueView_MCM", _dataSource) : null;
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
            if (_gauntletMovie is not null && ReleaseMovie is not null) ReleaseMovie(_gauntletLayer, _gauntletMovie);
            _gauntletLayer = null!;
            _gauntletMovie = null!;
            _dataSource.SettingProperty = null!;
            _dataSource = null!;
        }
    }
}
