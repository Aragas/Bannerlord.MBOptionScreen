using MBOptionScreen.Attributes;
using ModLib.GUI.ViewModels;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace ModLib.GUI.GauntletUI
{
    [ModuleOptionVersion("e1.0.0",  1)]
    [ModuleOptionVersion("e1.0.1",  1)]
    [ModuleOptionVersion("e1.0.2",  1)]
    [ModuleOptionVersion("e1.0.3",  1)]
    [ModuleOptionVersion("e1.0.4",  1)]
    [ModuleOptionVersion("e1.0.5",  1)]
    [ModuleOptionVersion("e1.0.6",  1)]
    [ModuleOptionVersion("e1.0.7",  1)]
    [ModuleOptionVersion("e1.0.8",  1)]
    [ModuleOptionVersion("e1.0.9",  1)]
    [ModuleOptionVersion("e1.0.10", 1)]
    [ModuleOptionVersion("e1.0.11", 1)]
    [ModuleOptionVersion("e1.1.0",  1)]
    internal class ModOptionsGauntletScreen : ScreenBase
    {
        private GauntletLayer gauntletLayer;
        private GauntletMovie movie;
        private ModSettingsScreenVM vm;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            SpriteData spriteData = UIResourceManager.SpriteData;
            TwoDimensionEngineResourceContext resourceContext = UIResourceManager.ResourceContext;
            ResourceDepot uiresourceDepot = UIResourceManager.UIResourceDepot;
            spriteData.SpriteCategories["ui_encyclopedia"].Load(resourceContext, uiresourceDepot);
            gauntletLayer = new GauntletLayer(1);
            gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
            gauntletLayer.IsFocusLayer = true;
            ScreenManager.TrySetFocus(gauntletLayer);
            AddLayer(gauntletLayer);
            vm = new ModSettingsScreenVM();
            movie = gauntletLayer.LoadMovie("ModOptionsScreen_v1", vm);
        }

        protected override void OnFrameTick(float dt)
        {
            base.OnFrameTick(dt);
            // || gauntletLayer.Input.IsGameKeyReleased(34)
            if (gauntletLayer.Input.IsHotKeyReleased("Exit"))
            {
                vm.ExecuteCancel();
            }
        }

        protected override void OnFinalize()
        {
            base.OnFinalize();
            RemoveLayer(gauntletLayer);
            gauntletLayer.ReleaseMovie(movie);
            gauntletLayer = null;
            movie = null;
            vm.ExecuteSelect(null);
            vm.AssignParent(true);
            vm = null;
        }
    }
}
