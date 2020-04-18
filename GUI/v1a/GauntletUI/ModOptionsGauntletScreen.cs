using MBOptionScreen.Attributes;
using MBOptionScreen.GUI.v1a.ViewModels;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace MBOptionScreen.GUI.v1a.GauntletUI
{
    [ModuleOptionVersion("e1.0.0",  2)]
    [ModuleOptionVersion("e1.0.1",  2)]
    [ModuleOptionVersion("e1.0.2",  2)]
    [ModuleOptionVersion("e1.0.3",  2)]
    [ModuleOptionVersion("e1.0.4",  2)]
    [ModuleOptionVersion("e1.0.5",  2)]
    [ModuleOptionVersion("e1.0.6",  2)]
    [ModuleOptionVersion("e1.0.7",  2)]
    [ModuleOptionVersion("e1.0.8",  2)]
    [ModuleOptionVersion("e1.0.9",  2)]
    [ModuleOptionVersion("e1.0.10", 2)]
    [ModuleOptionVersion("e1.0.11", 2)]
    [ModuleOptionVersion("e1.1.0",  2)]
    public class ModOptionsGauntletScreen : ScreenBase
    {
        private GauntletLayer _gauntletLayer;
        private GauntletMovie _gauntletMovie;
        private ModOptionsScreenVM _dataSource;

        protected override void OnInitialize()
        {
            var spriteData = UIResourceManager.SpriteData;
            var resourceContext = UIResourceManager.ResourceContext;
            var uiresourceDepot = UIResourceManager.UIResourceDepot;
            spriteData.SpriteCategories["ui_encyclopedia"].Load(resourceContext, uiresourceDepot);

            base.OnInitialize();
            _dataSource = new ModOptionsScreenVM();
            _gauntletLayer = new GauntletLayer(4000, "GauntletLayer");
            _gauntletMovie = _gauntletLayer.LoadMovie("ModOptionsView_v1a", _dataSource);
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
            _gauntletLayer = null;
            _gauntletMovie = null;
            _dataSource.ExecuteSelect(null);
            //_dataSource.AssignParent(true);
            _dataSource = null;
        }
    }
}
