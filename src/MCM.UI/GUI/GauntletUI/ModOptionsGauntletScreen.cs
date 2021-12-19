using Bannerlord.BUTR.Shared.Helpers;

using HarmonyLib.BUTR.Extensions;

using MCM.UI.GUI.ViewModels;
using MCM.UI.Utils;

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
            AccessTools2.GetDelegateObjectInstance<LoadMovieDelegate>(typeof(GauntletLayer), "LoadMovie");
        private static readonly ReleaseMovieDelegate? ReleaseMovie =
            AccessTools2.GetDelegateObjectInstance<ReleaseMovieDelegate>(typeof(GauntletLayer), "ReleaseMovie");

        private readonly ILogger<ModOptionsGauntletScreen> _logger;

        private GauntletLayer? _gauntletLayer;
        private object? _gauntletMovie;
        private ModOptionsVM _dataSource = default!;
        private SpriteCategory? _spriteCategoryEncyclopedia;
        private SpriteCategory? _spriteCategorySaveLoad;

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
            _spriteCategoryEncyclopedia = spriteData.SpriteCategories.TryGetValue("ui_encyclopedia", out var spriteCategoryEncyclopediaVal) ? spriteCategoryEncyclopediaVal : null;
            if (ApplicationVersionHelper.GameVersion() is { } gameVersion)
            {
                if ((gameVersion.Major >= 1 && gameVersion.Minor >= 6 && gameVersion.Revision >= 1) || (gameVersion.Major >= 1 && gameVersion.Minor >= 7))
                    _spriteCategorySaveLoad = spriteData.SpriteCategories.TryGetValue("ui_saveload", out var spriteCategorySaveLoadVal) ? spriteCategorySaveLoadVal : null;
            }
            _spriteCategoryEncyclopedia?.Load(resourceContext, uiresourceDepot);
            _spriteCategorySaveLoad?.Load(resourceContext, uiresourceDepot);
            _dataSource = new ModOptionsVM();
            if (GauntletLayerUtils.Create(4000, "GauntletLayer") is { } gauntletLayer)
            {
                _gauntletLayer = gauntletLayer;
                _gauntletMovie = LoadMovie is not null ? LoadMovie(_gauntletLayer, "ModOptionsView_MCM", _dataSource) : null;
                _gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
                _gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
                _gauntletLayer.IsFocusLayer = true;
                AddLayer(_gauntletLayer);
                ScreenManager.TrySetFocus(_gauntletLayer);
            }
        }

        protected override void OnFrameTick(float dt)
        {
            base.OnFrameTick(dt);
            if (_gauntletLayer is not null && _gauntletLayer.Input.IsHotKeyReleased("Exit"))
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
            if (_gauntletLayer is not null)
                RemoveLayer(_gauntletLayer);
            if (_gauntletLayer is not null && _gauntletMovie is not null && ReleaseMovie is not null)
                ReleaseMovie(_gauntletLayer, _gauntletMovie);
            _gauntletLayer = null;
            _gauntletMovie = null;
            _dataSource.ExecuteSelect(null);
            _dataSource = null!;
        }
    }
}