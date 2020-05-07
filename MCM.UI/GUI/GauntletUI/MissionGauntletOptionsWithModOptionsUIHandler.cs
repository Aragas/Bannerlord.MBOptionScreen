using MCM.Abstractions;
using MCM.Abstractions.Attributes;
using MCM.UI.GUI.ViewModels;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.MountAndBlade.LegacyGUI.Missions.Multiplayer;
using TaleWorlds.MountAndBlade.Source.Missions;
using TaleWorlds.MountAndBlade.View.Missions;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;
using TaleWorlds.TwoDimension;

namespace MCM.UI.GUI.GauntletUI
{
    /// <summary>
    /// In-mission Option screen
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
	[OverrideView(typeof(MissionOptionsUIHandler))]
	public class MissionGauntletOptionsWithModOptionsUIHandler : OptionsWithMCMOptionsMissionView
	{
        private GauntletLayer _gauntletLayer = default!;
		private OptionsModOptionsViewModel _dataSource = default!;
		private GauntletMovie _movie = default!;
		private KeybindingPopup _keybindingPopup = default!;
		private GameKeyOptionVM _currentGameKey = default!;
		private readonly SpriteCategory _spriteCategoryOptions;
		private readonly SpriteCategory _spriteCategoryEncyclopedia;

		public MissionGauntletOptionsWithModOptionsUIHandler()
		{
			var spriteData = UIResourceManager.SpriteData;
			var resourceContext = UIResourceManager.ResourceContext;
			var uiresourceDepot = UIResourceManager.UIResourceDepot;
            _spriteCategoryOptions = spriteData.SpriteCategories["ui_options"];
            _spriteCategoryOptions.Load(resourceContext, uiresourceDepot);
            _spriteCategoryEncyclopedia = spriteData.SpriteCategories["ui_encyclopedia"];
            _spriteCategoryEncyclopedia.Load(resourceContext, uiresourceDepot);
			ViewOrderPriorty = 49;
		}

		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			Mission.GetMissionBehaviour<MissionOptionsComponent>().OnOptionsAdded += OnShowOptions;
			_keybindingPopup = new KeybindingPopup(SetHotKey, MissionScreen);
		}

		public override void OnMissionScreenFinalize()
		{
			Mission.GetMissionBehaviour<MissionOptionsComponent>().OnOptionsAdded -= OnShowOptions;
			base.OnMissionScreenFinalize();
			var dataSource = _dataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			_dataSource = null!;
            _movie.Release(); // TODO
			_movie = null!;
            var keybindingPopup = _keybindingPopup;
			if (keybindingPopup != null)
			{
				keybindingPopup.OnToggle(false);
			}
			_keybindingPopup = null!;
			_gauntletLayer = null!;
            _spriteCategoryOptions.Unload();
            _spriteCategoryEncyclopedia.Unload();
		}

		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			var keybindingPopup = _keybindingPopup;
			if (keybindingPopup == null)
			{
				return;
			}
			keybindingPopup.Tick();
		}

		public override bool OnEscape()
		{
			if (_dataSource != null)
			{
				_dataSource.ExecuteCloseOptions();
				return true;
			}
			return base.OnEscape();
		}

		private void OnShowOptions()
		{
			OnEscapeMenuToggled(true);
		}

		private void OnCloseOptions()
		{
			OnEscapeMenuToggled(false);
		}

		private void OnEscapeMenuToggled(bool isOpened)
		{
			if (isOpened)
			{
				if (!GameNetwork.IsMultiplayer)
				{
					MBCommon.PauseGameEngine();
				}
			}
			else
			{
				MBCommon.UnPauseGameEngine();
			}
			if (isOpened)
			{
				_dataSource = new OptionsModOptionsViewModel(new OptionsVM(GameNetwork.IsMultiplayer, OnCloseOptions, OnKeybindRequest), new ModOptionsVM());
				_gauntletLayer = new GauntletLayer(ViewOrderPriorty, "GauntletLayer");
				_gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				_gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
				_movie = _gauntletLayer.LoadMovie("OptionsWithModOptionsView_v3", _dataSource);
				MissionScreen.AddLayer(_gauntletLayer);
				_gauntletLayer.IsFocusLayer = true;
				ScreenManager.TrySetFocus(_gauntletLayer);
				return;
			}
			_gauntletLayer.InputRestrictions.ResetInputRestrictions();
			_gauntletLayer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(_gauntletLayer);
			MissionScreen.RemoveLayer(_gauntletLayer);
			var keybindingPopup = _keybindingPopup;
			if (keybindingPopup != null)
			{
				keybindingPopup.OnToggle(false);
			}
			_gauntletLayer = null!;
			_dataSource.OnFinalize();
			_dataSource = null!;
			_gauntletLayer = null!;
		}

		private void OnKeybindRequest(GameKeyOptionVM requestedHotKeyToChange)
		{
			_currentGameKey = requestedHotKeyToChange;
			_keybindingPopup.OnToggle(true);
		}

        private void SetHotKey(Key key)
		{
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
