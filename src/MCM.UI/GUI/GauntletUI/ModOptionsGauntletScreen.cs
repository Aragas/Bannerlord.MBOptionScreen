using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

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
        private delegate object LoadMovieDelegate(object instance, string movieName, ViewModel dataSource);
        private delegate void ReleaseMovieDelegate(object instance, object movie);

        private static readonly LoadMovieDelegate? LoadMovie =
            AccessTools2.GetDelegateObjectInstance<LoadMovieDelegate>(AccessTools.Method(typeof(GauntletLayer), "LoadMovie"));
        private static readonly ReleaseMovieDelegate? ReleaseMovie =
            AccessTools2.GetDelegateObjectInstance<ReleaseMovieDelegate>(AccessTools.Method(typeof(GauntletLayer), "ReleaseMovie"));

        private readonly ILogger<ModOptionsGauntletScreen> _logger;

        private GauntletLayer _gauntletLayer = default!;
        private object? _gauntletMovie;
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
            _gauntletMovie = LoadMovie is not null ? LoadMovie(_gauntletLayer, "ModOptionsView_MCM", _dataSource) : null;
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
            if (_gauntletMovie is not null && ReleaseMovie is not null) ReleaseMovie(_gauntletLayer, _gauntletMovie);
            _gauntletLayer = null!;
            _gauntletMovie = null!;
            _dataSource.ExecuteSelect(null);
            _dataSource = null!;
        }
    }
}
