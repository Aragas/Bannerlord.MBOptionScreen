using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.ViewModels;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using MCM.UI.GUI.ViewModels;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace MCM.UI.UIExtenderEx
{
    [ViewModelMixin]
    internal sealed class OptionsVMMixin : BaseViewModelMixin<OptionsVM>
    {
        private delegate void ExecuteDoneDelegate(OptionsVM instance);
        private delegate void ExecuteCancelDelegate(OptionsVM instance);

        private static readonly ExecuteDoneDelegate? ExecuteDoneMethod =
            AccessTools2.GetDelegate<ExecuteDoneDelegate>(typeof(OptionsVM), "ExecuteDone");
        private static readonly ExecuteCancelDelegate? ExecuteCancelMethod =
            AccessTools2.GetDelegate<ExecuteCancelDelegate>(typeof(OptionsVM), "ExecuteCancel");

        private static readonly AccessTools.FieldRef<OptionsVM, List<ViewModel>>? _categories =
            AccessTools2.FieldRefAccess<OptionsVM, List<ViewModel>>("_categories");

        static OptionsVMMixin()
        {
            var harmony = new Harmony("bannerlord.mcm.ui.optionsvm");

            harmony.CreateReversePatcher(
                AccessTools2.Method(typeof(OptionsVM), "ExecuteCloseOptions"),
                new HarmonyMethod(SymbolExtensions2.GetMethodInfo((OptionsVM x) => OriginalExecuteCloseOptions(x)))).Patch();

            harmony.Patch(
                AccessTools2.Method(typeof(OptionsVM), "ExecuteCloseOptions"),
                postfix: new HarmonyMethod(SymbolExtensions2.GetMethodInfo((OptionsVM x) => ExecuteCloseOptionsPostfix(x)), 300));

            harmony.Patch(
                AccessTools2.Method(typeof(OptionsVM), "RefreshValues"),
                postfix: new HarmonyMethod(SymbolExtensions2.GetMethodInfo((OptionsVM x) => RefreshValuesPostfix(x)), 300));

            harmony.Patch(
                AccessTools2.PropertySetter(typeof(OptionsVM), "CategoryIndex"),
                postfix: new HarmonyMethod(SymbolExtensions2.GetMethodInfo((OptionsVM x) => SetSelectedCategoryPostfix(x)), 300));
        }

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private static void ExecuteCloseOptionsPostfix(OptionsVM __instance)
        {
            if (__instance.GetPropertyValue("MCMMixin") is WeakReference<OptionsVMMixin> weakReference && weakReference.TryGetTarget(out var mixin))
            {
                mixin?.ExecuteCloseOptions();
            }
        }
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private static void RefreshValuesPostfix(OptionsVM __instance)
        {
            if (__instance.GetPropertyValue("ModOptions") is ModOptionsVM modOptions)
            {
                modOptions.RefreshValues();
            }
        }
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private static void SetSelectedCategoryPostfix(OptionsVM __instance)
        {
            if (__instance.GetPropertyValue("MCMMixin") is WeakReference<OptionsVMMixin> weakReference && weakReference.TryGetTarget(out var mixin))
            {
                mixin.ModOptionsSelected = __instance.CategoryIndex == 4;
            }
        }

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private static void OriginalExecuteCloseOptions(OptionsVM instance) => throw new NotImplementedException("It's a stub");

        private readonly ModOptionsVM _modOptions = new();
        private bool _modOptionsSelected;
        private int _descriptionWidth = 650;

        [DataSourceProperty]
        public WeakReference<OptionsVMMixin> MCMMixin => new(this);

        [DataSourceProperty]
        public ModOptionsVM ModOptions => _modOptions;

        [DataSourceProperty]
        public int DescriptionWidth
        {
            get => _descriptionWidth;
            private set => SetField(ref _descriptionWidth, value, nameof(DescriptionWidth));
        }

        [DataSourceProperty]
        public bool ModOptionsSelected
        {
            get => _modOptionsSelected;
            set
            {
                if (!SetField(ref _modOptionsSelected, value, nameof(ModOptionsSelected))) return;
                _modOptions.IsDisabled = !value;
                _modOptionsSelected = value;
                DescriptionWidth = ModOptionsSelected ? 0 : 650;
            }
        }

        public OptionsVMMixin(OptionsVM vm) : base(vm)
        {
            vm.PropertyChanged += OptionsVM_PropertyChanged;
            var list = _categories?.Invoke(vm) ?? new List<ViewModel>();
            list.Insert(5, _modOptions);
        }

        private void OptionsVM_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            _modOptions.OnPropertyChanged(e.PropertyName);
        }

        public override void OnFinalize()
        {
            if (ViewModel is not null)
                ViewModel.PropertyChanged -= OptionsVM_PropertyChanged;

            base.OnFinalize();
        }

        [DataSourceMethod]
        public void ExecuteCloseOptions()
        {
            ModOptions.ExecuteCancelInternal(false);
            if (ViewModel is not null)
            {
                try
                {
                    OriginalExecuteCloseOptions(ViewModel);
                }
                // For some reason it throws a null exception, but ignoring it seems to be fine for now.
                catch { }
            }

            OnFinalize();
        }

        [DataSourceMethod]
        public void ExecuteDone()
        {
            ModOptions.ExecuteDoneInternal(false, () =>
            {
                if (ViewModel is not null)
                    ExecuteDoneMethod?.Invoke(ViewModel);
            });
        }

        [DataSourceMethod]
        public void ExecuteCancel()
        {
            ModOptions.ExecuteCancelInternal(false, () =>
            {
                if (ViewModel is not null)
                    ExecuteCancelMethod?.Invoke(ViewModel);
            });
        }
    }
}