using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Reflection;

using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.GamepadOptions;

namespace MCM.UI.GUI.ViewModels
{
    internal class OptionsModOptionsViewModel : ViewModel
    {
        private const BindingFlags BindingFlagsAnyDeclared = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

        private readonly OptionsVM _wrapped;
        private readonly ModOptionsVM _modOptions = new ModOptionsVM();
        private bool _modOptionsSelected;

        public OptionsModOptionsViewModel(OptionsVM wrapped)
        {
            _wrapped = wrapped;
            _wrapped.PropertyChanged += (sender, args) => OnPropertyChanged(args.PropertyName);
        }

        #region Wrapped Properties

        public bool OldGameStateManagerDisabledStatus
        {
            get => _wrapped.OldGameStateManagerDisabledStatus;
            set => typeof(OptionsVM).InvokeMember("set_OldGameStateManagerDisabledStatus", BindingFlags.InvokeMethod | BindingFlagsAnyDeclared, null, this, new object[] { value });
        }

        public Dictionary<NativeOptions.NativeOptionsType, float[]> DefaultOptions => (Dictionary<NativeOptions.NativeOptionsType, float[]>)
            typeof(OptionsVM).InvokeMember("get_DefaultOptions", BindingFlags.InvokeMethod | BindingFlagsAnyDeclared, null, this, Array.Empty<object>());

        public IEnumerable<IOptionData> GameplayOptionsList => (IEnumerable<IOptionData>)
            typeof(OptionsVM).InvokeMember("get_DefaultOptions", BindingFlags.InvokeMethod | BindingFlagsAnyDeclared, null, this, Array.Empty<object>());

        public IEnumerable<IOptionData> VideoOptionsList => (IEnumerable<IOptionData>)
            typeof(OptionsVM).InvokeMember("get_VideoOptionsList", BindingFlags.InvokeMethod | BindingFlagsAnyDeclared, null, this, Array.Empty<object>());

        public IEnumerable<IOptionData> PerformanceOptionsList => (IEnumerable<IOptionData>)
            typeof(OptionsVM).InvokeMember("get_PerformanceOptionsList", BindingFlags.InvokeMethod | BindingFlagsAnyDeclared, null, this, Array.Empty<object>());

        [DataSourceProperty]
        public string OptionsLbl { get => _wrapped.OptionsLbl; set => _wrapped.OptionsLbl = value; }

        [DataSourceProperty]
        public string CancelLbl { get => _wrapped.CancelLbl; set => _wrapped.CancelLbl = value; }

        [DataSourceProperty]
        public string ResetLbl { get => _wrapped.ResetLbl; set => _wrapped.ResetLbl = value; }

        [DataSourceProperty]
        public string DoneLbl { get => _wrapped.DoneLbl; set => _wrapped.DoneLbl = value; }

        [DataSourceProperty]
        public string MessageBoxText { get => _wrapped.MessageBoxText; set { } }

        public string VideoMemoryUsageName { get => _wrapped.VideoMemoryUsageName; set => _wrapped.VideoMemoryUsageName = value; }

        [DataSourceProperty]
        public string VideoMemoryUsageText { get => _wrapped.VideoMemoryUsageText; set => _wrapped.VideoMemoryUsageText = value; }

        [DataSourceProperty]
        public float VideoMemoryUsageNormalized { get => _wrapped.VideoMemoryUsageNormalized; set => _wrapped.VideoMemoryUsageNormalized = value; }

        [DataSourceProperty]
        public GameKeyOptionCategoryVM GameKeyOptionGroups
        {
            get
            {
                ModOptionsSelected = false; 
                return _wrapped.GameKeyOptionGroups;
            }
        }

        [DataSourceProperty]
        public GamepadOptionCategoryVM GamepadOptions
        {
            get
            {
                ModOptionsSelected = false;
                return _wrapped.GamepadOptions;
            }
        }

        [DataSourceProperty]
        public OptionCategoryVM GameplayOptions
        {
            get
            {
                ModOptionsSelected = false;
                return _wrapped.GameplayOptions;
            }
        }

        [DataSourceProperty]
        public OptionCategoryVM AudioOptions
        {
            get
            {
                ModOptionsSelected = false; 
                return _wrapped.AudioOptions;
            }
        }

        [DataSourceProperty]
        public OptionCategoryVM GraphicsOptions
        {
            get
            {
                ModOptionsSelected = false;
                return _wrapped.GraphicsOptions;
            }
        }

        [DataSourceProperty]
        public OptionCategoryVM VideoOptions
        {
            get
            {
                ModOptionsSelected = false;
                return _wrapped.VideoOptions;
            }
        }

        [DataSourceProperty]
        public BrightnessOptionVM BrightnessPopUp { get => _wrapped.BrightnessPopUp; set => _wrapped.BrightnessPopUp = value; }

        public bool IsDevelopmentMode { get => _wrapped.IsDevelopmentMode; set => _wrapped.IsDevelopmentMode = value; }

        public override void OnFinalize()
        {
            _wrapped.OnFinalize();
            base.OnFinalize();
        }

        public override void RefreshValues()
        {
            _wrapped.RefreshValues();
            base.RefreshValues();
        }

        #endregion

        [DataSourceProperty]
        public ModOptionsVM ModOptions
        {
            get
            {
                ModOptionsSelected = true;
                return _modOptions;
            }
        }

        [DataSourceProperty]
        public int DescriptionWidth => ModOptionsSelected ? 0 : 650;

        private bool ModOptionsSelected
        {
            get => _modOptionsSelected;
            set
            {
                OnPropertyChanged(nameof(DescriptionWidth));
                _modOptionsSelected = value;
            }
        }

        public void ExecuteCloseOptions()
        {
            _wrapped.ExecuteCloseOptions();
        }

        public void ExecuteDone()
        {
            ModOptions.ExecuteDoneInternal(false);
            AccessTools.Method(typeof(OptionsVM), "ExecuteDone").Invoke(_wrapped, Array.Empty<object>());
        }

        public void ExecuteCancel()
        {
            ModOptions.ExecuteCancelInternal(false);
            AccessTools.Method(typeof(OptionsVM), "ExecuteCancel").Invoke(_wrapped, Array.Empty<object>());
        }
    }
}