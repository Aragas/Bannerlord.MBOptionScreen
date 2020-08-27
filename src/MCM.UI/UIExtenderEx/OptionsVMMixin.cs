using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.ViewModels;

using HarmonyLib;

using MCM.UI.GUI.ViewModels;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace MCM.UI.UIExtenderEx
{
    [ViewModelMixin]
    internal class OptionsVMMixin : BaseViewModelMixin<OptionsVM>
    {
        private static readonly Harmony _harmony = new Harmony("bannerlord.mcm.ui.optionsvm");
        private static readonly AccessTools.FieldRef<ViewModel, Dictionary<string, PropertyInfo>> _propertyInfosField =
            AccessTools.FieldRefAccess<ViewModel, Dictionary<string, PropertyInfo>>("_propertyInfos");
        static OptionsVMMixin()
        {
            _harmony.Patch(
                AccessTools.Property(typeof(OptionsVM), nameof(OptionsVM.VideoOptions)).GetMethod,
                postfix: new HarmonyMethod(AccessTools.Method(typeof(OptionsVMMixin), nameof(Postfix)), 300));
            _harmony.Patch(
                AccessTools.Property(typeof(OptionsVM), nameof(OptionsVM.AudioOptions)).GetMethod,
                postfix: new HarmonyMethod(AccessTools.Method(typeof(OptionsVMMixin), nameof(Postfix)), 300));
            _harmony.Patch(
                AccessTools.Property(typeof(OptionsVM), nameof(OptionsVM.GameKeyOptionGroups)).GetMethod,
                postfix: new HarmonyMethod(AccessTools.Method(typeof(OptionsVMMixin), nameof(Postfix)), 300));
            _harmony.Patch(
                AccessTools.Property(typeof(OptionsVM), nameof(OptionsVM.GamepadOptions)).GetMethod,
                postfix: new HarmonyMethod(AccessTools.Method(typeof(OptionsVMMixin), nameof(Postfix)), 300));
            _harmony.Patch(
                AccessTools.Property(typeof(OptionsVM), nameof(OptionsVM.GameplayOptions)).GetMethod,
                postfix: new HarmonyMethod(AccessTools.Method(typeof(OptionsVMMixin), nameof(Postfix)), 300));
            _harmony.Patch(
                AccessTools.Property(typeof(OptionsVM), nameof(OptionsVM.GraphicsOptions)).GetMethod,
                postfix: new HarmonyMethod(AccessTools.Method(typeof(OptionsVMMixin), nameof(Postfix)), 300));
        }
        private static void Postfix(OptionsVM __instance)
        {
            var property = _propertyInfosField(__instance);
            if (property.TryGetValue(nameof(ModOptionsSelected), out var propertyInfo))
                propertyInfo.SetValue(__instance, false);
        }

        private static readonly MethodInfo ExecuteDoneMethod = AccessTools.Method(typeof(OptionsVM), "ExecuteDone");
        private static readonly MethodInfo ExecuteCancelMethod = AccessTools.Method(typeof(OptionsVM), "ExecuteCancel");

        private readonly ModOptionsVM _modOptions = new ModOptionsVM();
        private bool _modOptionsSelected;
        private int _descriptionWidth = 650;

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
        public int DescriptionWidth
        {
            get => _descriptionWidth;
            private set
            {
                if (_descriptionWidth == value)
                    return;

                _descriptionWidth = value;
                ViewModel?.OnPropertyChanged(nameof(DescriptionWidth));
            }
        }

        [DataSourceProperty]
        public bool ModOptionsSelected
        {
            get => _modOptionsSelected;
            set
            {
                if (_modOptionsSelected == value)
                    return;

                _modOptionsSelected = value;
                DescriptionWidth = ModOptionsSelected ? 0 : 650;
            }
        }

        public OptionsVMMixin(OptionsVM vm) : base(vm)
        {
            vm.PropertyChanged += OptionsVM_PropertyChanged;
        }

        private void OptionsVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _modOptions.OnPropertyChanged(e.PropertyName);
        }

        public override void OnFinalize()
        {
            if (ViewModel != null)
                ViewModel.PropertyChanged -= OptionsVM_PropertyChanged;

            base.OnFinalize();
        }

        [DataSourceMethod]
        public void ExecuteCloseOptions()
        {
            ModOptions.ExecuteCancelInternal(false);
            ViewModel?.ExecuteCloseOptions();
        }

        [DataSourceMethod]
        public void ExecuteDone()
        {
            ModOptions.ExecuteDoneInternal(false, () =>
            {
                if (ViewModel != null)
                    ExecuteDoneMethod.Invoke(ViewModel, Array.Empty<object>());
            });
        }

        [DataSourceMethod]
        public void ExecuteCancel()
        {
            ModOptions.ExecuteCancelInternal(false, () =>
            {
                if (ViewModel != null)
                    ExecuteCancelMethod.Invoke(ViewModel, Array.Empty<object>());
            });
        }
    }
}