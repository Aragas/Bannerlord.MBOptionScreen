using MBOptionScreen.Attributes;
using MBOptionScreen.GUI.v1e_7.ResourceInjection;
using MBOptionScreen.GUI.v1e_7.ViewModels;
using MBOptionScreen.ResourceInjection;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace MBOptionScreen.GUI.v1e_7.GauntletUI
{
    [Version("e1.0.0",  220)]
    [Version("e1.0.1",  220)]
    [Version("e1.0.2",  220)]
    [Version("e1.0.3",  220)]
    [Version("e1.0.4",  220)]
    [Version("e1.0.5",  220)]
    [Version("e1.0.6",  220)]
    [Version("e1.0.7",  220)]
    [Version("e1.0.8",  220)]
    [Version("e1.0.9",  220)]
    [Version("e1.0.10", 220)]
    [Version("e1.0.11", 220)]
    [Version("e1.1.0",  220)]
    [Version("e1.2.0",  220)]
    [Version("e1.2.1",  220)]
    [Version("e1.3.0",  220)]
    public class ModOptionsGauntletScreen : MBOptionScreen
    {
        private GauntletLayer _gauntletLayer;
        private GauntletMovie _gauntletMovie;
        private ModOptionsScreenVM _dataSource;

        public ModOptionsGauntletScreen()
        {
            BrushLoader.Inject(BaseResourceInjector.Instance);
            PrefabsLoader.Inject(BaseResourceInjector.Instance);
        }

        protected override void OnInitialize()
        {
            var spriteData = UIResourceManager.SpriteData;
            var resourceContext = UIResourceManager.ResourceContext;
            var uiresourceDepot = UIResourceManager.UIResourceDepot;
            spriteData.SpriteCategories["ui_encyclopedia"].Load(resourceContext, uiresourceDepot);

            base.OnInitialize();
            _dataSource = new ModOptionsScreenVM();
            _gauntletLayer = new GauntletLayer(4000, "GauntletLayer");
            _gauntletMovie = _gauntletLayer.LoadMovie("ModOptionsView_v1e_7", _dataSource);
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
            RemoveLayer(_gauntletLayer);
            _gauntletLayer.ReleaseMovie(_gauntletMovie);
            _gauntletLayer = null!;
            _gauntletMovie = null!;
            _dataSource.ExecuteSelect(null);
            _dataSource = null!;
        }
    }
}
