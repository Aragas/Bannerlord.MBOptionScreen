using MCM.Abstractions.Attributes;
using MCM.UI.GUI.ViewModels;

using System.Linq;

using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.MountAndBlade.View.Missions;
using TaleWorlds.MountAndBlade.View.Screen;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;
using TaleWorlds.TwoDimension;

namespace MCM.UI.GUI.GauntletUI
{
	/// <summary>
	/// Game's Option screen
	/// </summary>
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
	[Version("e1.3.1",  1)]
    [Version("e1.4.0",  1)]
    [Version("e1.4.1",  1)]
	[OverrideView(typeof(OptionsScreen))]
    public class OptionsWithModOptionsGauntletScreen : ScreenBase, IOptionsWithMCMOptionsScreen
	{
        private GauntletLayer _gauntletLayer = default!;
		private OptionsModOptionsViewModel _dataSource = default!;
		private GauntletMovie _gauntletMovie = default!;
        private KeybindingPopup _keybindingPopup = default!;
		private GameKeyOptionVM _currentGameKey = default!;
		private SpriteCategory _spriteCategoryOptions = default!;
		private SpriteCategory _spriteCategoryEncyclopedia = default!;

		protected override void OnInitialize()
		{
            base.OnInitialize();
			var spriteData = UIResourceManager.SpriteData;
			var resourceContext = UIResourceManager.ResourceContext;
			var uiresourceDepot = UIResourceManager.UIResourceDepot;
            _spriteCategoryOptions = spriteData.SpriteCategories["ui_options"];
            _spriteCategoryOptions.Load(resourceContext, uiresourceDepot);
            _spriteCategoryEncyclopedia = spriteData.SpriteCategories["ui_encyclopedia"];
            _spriteCategoryEncyclopedia.Load(resourceContext, uiresourceDepot);
			_dataSource = new OptionsModOptionsViewModel(new OptionsVM(true, false, OnKeybindRequest), new ModOptionsVM());
			_gauntletLayer = new GauntletLayer(4000, "GauntletLayer");
			_gauntletMovie = _gauntletLayer.LoadMovie("OptionsWithModOptionsView_v3", _dataSource);
			_gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			_gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			_gauntletLayer.IsFocusLayer = true;
			_keybindingPopup = new KeybindingPopup(SetHotKey, this);
			AddLayer(_gauntletLayer);
			ScreenManager.TrySetFocus(_gauntletLayer);
			Utilities.SetForceVsync(true);
		}

		protected override void OnFinalize()
		{
			base.OnFinalize();
            _spriteCategoryOptions?.Unload();
			// TODO: There was a report that the encyclopedia UI is bugged
			//_spriteCategoryEncyclopedia?.Unload();
			if (_gauntletLayer != null)
                RemoveLayer(_gauntletLayer);
            _gauntletLayer?.ReleaseMovie(_gauntletMovie);
            _gauntletLayer = null!;
            _gauntletMovie = null!;
            _dataSource?.ModOptions?.ExecuteSelect(null);
            _dataSource = null!;
			Utilities.SetForceVsync(false);
            _keybindingPopup = null!;
		}

        protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (!_keybindingPopup.IsActive && _gauntletLayer.Input.IsHotKeyReleased("Exit"))
			{
				_dataSource.ExecuteCloseOptions();
				ScreenManager.TrySetFocus(_gauntletLayer);
				if (Game.Current != null)
				{
					Game.Current.GameStateManager.ActiveStateDisabledByUser = _dataSource.OldGameStateManagerDisabledStatus;
				}
				ScreenManager.PopScreen();
			}
			_keybindingPopup.Tick();
		}

		private void OnKeybindRequest(GameKeyOptionVM requestedHotKeyToChange)
		{
			_currentGameKey = requestedHotKeyToChange;
			_keybindingPopup.OnToggle(true);
		}

		private void SetHotKey(Key key)
		{
			if (_dataSource.GameKeyOptionGroups.Groups.First(g => g.GameKeys.Contains(_currentGameKey)).GameKeys.Any(k => k.CurrentKey.InputKey == key.InputKey))
			{
				InformationManager.AddQuickInformation(new TextObject("{=n4UUrd1p}Already in use", null), 0, null, "");
				return;
			}
			if (_gauntletLayer.Input.IsHotKeyReleased("Exit"))
			{
				_keybindingPopup.OnToggle(false);
				return;
			}
			var currentGameKey = _currentGameKey;
			if (currentGameKey != null)
			{
				currentGameKey.Set(key.InputKey);
			}
			_currentGameKey = null!;
			_keybindingPopup.OnToggle(false);
		}
    }
}