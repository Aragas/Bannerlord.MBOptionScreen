using MCM.UI.GUI.ViewModels;

using Microsoft.Extensions.Logging;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace MCM.UI.GUI.GauntletUI
{
    /// <summary>
    /// Mod's Option screen
    /// </summary>
    internal sealed class ModOptionsGauntletScreen : ScreenBase, IMCMOptionsScreen
    {
        private readonly ILogger<ModOptionsGauntletScreen> _logger;

        private GauntletLayer? _gauntletLayer;
#if v130
        private GauntletMovieIdentifier? _gauntletMovie;
#elif v100 || v101 || v102 || v103 || v110 || v111 || v112 || v113 || v114 || v115 || v116 || v120 || v121 || v122 || v123 || v124 || v125 || v126 || v127 || v128 || v129 || v1210 || v1211 || v1212
        private IGauntletMovie? _gauntletMovie;
#else
#error DEFINE
#endif
        private ModOptionsVM _dataSource = default!;
        private SpriteCategory? _spriteCategoryMCM;

        public ModOptionsGauntletScreen(ILogger<ModOptionsGauntletScreen> logger)
        {
            _logger = logger;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            var spriteData = UIResourceManager.SpriteData;
            var resourceContext = UIResourceManager.ResourceContext;
#if v130
            var uiresourceDepot = UIResourceManager.ResourceDepot;
#elif v100 || v101 || v102 || v103 || v110 || v111 || v112 || v113 || v114 || v115 || v116 || v120 || v121 || v122 || v123 || v124 || v125 || v126 || v127 || v128 || v129 || v1210 || v1211 || v1212
            var uiresourceDepot = UIResourceManager.UIResourceDepot;
#else
#error DEFINE
#endif
            _spriteCategoryMCM = spriteData.SpriteCategories.TryGetValue("ui_mcm", out var spriteCategoryMCMVal) ? spriteCategoryMCMVal : null;
            _spriteCategoryMCM?.Load(resourceContext, uiresourceDepot);
            _dataSource = new ModOptionsVM();
            _gauntletLayer = new GauntletLayer(4000, "GauntletLayer");
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
            if (_spriteCategoryMCM is not null)
                _spriteCategoryMCM.Unload();
            if (_gauntletLayer is not null)
                RemoveLayer(_gauntletLayer);
            if (_gauntletLayer is not null && _gauntletMovie is not null)
                _gauntletLayer.ReleaseMovie(_gauntletMovie);
            _gauntletLayer = null;
            _gauntletMovie = null;
            _dataSource.ExecuteSelect(null);
            _dataSource = null!;
        }
    }
}