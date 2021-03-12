using Bannerlord.ButterLib.Common.Helpers;

using MCM.UI.GUI.ViewModels;

using Microsoft.Extensions.Logging;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace MCM.UI.GUI.GauntletUI
{
    /// <summary>
    /// Mod's Option screen
    /// </summary>
    internal sealed class ModOptionsGauntletScreen : ScreenBase, IMCMOptionsScreen
    {
        private delegate void ReleaseMovieDelegate(object movie);

        private readonly ILogger<ModOptionsGauntletScreen> _logger;

        private GauntletLayer _gauntletLayer = default!;
        private object _gauntletMovie = default!;
        private ModOptionsVM _dataSource = default!;
        private SpriteCategory _spriteCategoryEncyclopedia = default!;
        private ReleaseMovieDelegate? _releaseMovie;

        public ModOptionsGauntletScreen(ILogger<ModOptionsGauntletScreen> logger)
        {
            _logger = logger;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            var spriteData = UIResourceManager.SpriteData;
            var resourceContext = UIResourceManager.ResourceContext;
            var uiresourceDepot = UIResourceManager.UIResourceDepot;
            _spriteCategoryEncyclopedia = spriteData.SpriteCategories["ui_encyclopedia"];
            _spriteCategoryEncyclopedia.Load(resourceContext, uiresourceDepot);
            _dataSource = new ModOptionsVM();
            _gauntletLayer = new GauntletLayer(4000, "GauntletLayer");
            _releaseMovie = AccessTools2.GetDelegate<ReleaseMovieDelegate>(typeof(GauntletLayer), "ReleaseMovie");
            _gauntletMovie = _gauntletLayer.LoadMovie("ModOptionsView_MCM", _dataSource);
            _gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
            _gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            _gauntletLayer.IsFocusLayer = true;
            AddLayer(_gauntletLayer);
            ScreenManager.TrySetFocus(_gauntletLayer);
        }

        protected override void OnFrameTick(float dt)
        {
            base.OnFrameTick(dt);
            if (_gauntletLayer.Input.IsHotKeyReleased("Exit"))
            {
                _dataSource.ExecuteClose();
                ScreenManager.TryLoseFocus(_gauntletLayer);
                ScreenManager.PopScreen();
            }
        }

        protected override void OnFinalize()
        {
            base.OnFinalize();
            // TODO: There was a report that the encyclopedia UI is bugged
            //_spriteCategoryEncyclopedia.Unload();
            RemoveLayer(_gauntletLayer);
            _releaseMovie?.Invoke(_gauntletMovie);
            _gauntletLayer = null!;
            _gauntletMovie = null!;
            _dataSource.ExecuteSelect(null);
            _dataSource = null!;
        }
    }
}
