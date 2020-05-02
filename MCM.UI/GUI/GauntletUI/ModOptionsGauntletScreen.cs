using MCM.Abstractions;
using MCM.Abstractions.Attributes;
using MCM.UI.GUI.ViewModels;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace MCM.UI.GUI.GauntletUI
{
    [Version("e1.0.0",  1)]
    [Version("e1.0.1",  1)]
    [Version("e1.0.2",  1)]
    [Version("e1.0.3",  1)]
    [Version("e1.0.4",  1)]
    [Version("e1.0.5",  1)]
    [Version("e1.0.6",  1)]
    [Version("e1.0.7",  1)]
    [Version("e1.0.8",  1)]
    [Version("e1.0.9",  1)]
    [Version("e1.0.10", 1)]
    [Version("e1.0.11", 1)]
    [Version("e1.1.0",  1)]
    [Version("e1.2.0",  1)]
    [Version("e1.2.1",  1)]
    [Version("e1.3.0",  1)]
    internal class ModOptionsGauntletScreen : MBOptionScreen
    {
        private GauntletLayer _gauntletLayer = default!;
        private GauntletMovie _gauntletMovie = default!;
        private ModOptionsVM _dataSource = default!;

        protected override void OnInitialize()
        {
            var spriteData = UIResourceManager.SpriteData;
            var resourceContext = UIResourceManager.ResourceContext;
            var uiresourceDepot = UIResourceManager.UIResourceDepot;
            spriteData.SpriteCategories["ui_encyclopedia"].Load(resourceContext, uiresourceDepot);
            
            base.OnInitialize();
            _dataSource = new ModOptionsVM();
            _gauntletLayer = new GauntletLayer(4000, "GauntletLayer");
            _gauntletMovie = _gauntletLayer.LoadMovie("ModOptionsView_v3", _dataSource);
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
