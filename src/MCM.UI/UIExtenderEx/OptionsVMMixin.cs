using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.ViewModels;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using MCM.UI.GUI.ViewModels;

using System;
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

        static OptionsVMMixin()
        {
            var harmony = new Harmony("bannerlord.mcm.ui.optionsvm");

            if (AccessTools2.Property(typeof(OptionsVM), "VideoOptions")?.GetMethod is { } m1)
                harmony.Patch(m1, postfix: new HarmonyMethod(AccessTools2.Method(typeof(OptionsVMMixin), nameof(OptionsPostfix)), 300));
            if (AccessTools2.Property(typeof(OptionsVM), "AudioOptions")?.GetMethod is { } m2)
                harmony.Patch(m2, postfix: new HarmonyMethod(AccessTools2.Method(typeof(OptionsVMMixin), nameof(OptionsPostfix)), 300));
            if (AccessTools2.Property(typeof(OptionsVM), "GameKeyOptionGroups")?.GetMethod is { } m3)
                harmony.Patch(m3, postfix: new HarmonyMethod(AccessTools2.Method(typeof(OptionsVMMixin), nameof(OptionsPostfix)), 300));
            if (AccessTools2.Property(typeof(OptionsVM), "GamepadOptions")?.GetMethod is { } m4)
                harmony.Patch(m4, postfix: new HarmonyMethod(AccessTools2.Method(typeof(OptionsVMMixin), nameof(OptionsPostfix)), 300));
            if (AccessTools2.Property(typeof(OptionsVM), "GameplayOptions")?.GetMethod is { } m5)
                harmony.Patch(m5, postfix: new HarmonyMethod(AccessTools2.Method(typeof(OptionsVMMixin), nameof(OptionsPostfix)), 300));
            if (AccessTools2.Property(typeof(OptionsVM), "PerformanceOptions")?.GetMethod is { } m6)
                harmony.Patch(m6, postfix: new HarmonyMethod(AccessTools2.Method(typeof(OptionsVMMixin), nameof(OptionsPostfix)), 300));

            harmony.CreateReversePatcher(
                AccessTools2.Method(typeof(OptionsVM), nameof(OptionsVM.ExecuteCloseOptions)),
                new HarmonyMethod(SymbolExtensions2.GetMethodInfo(() => OriginalExecuteCloseOptions(null!)))).Patch();

            harmony.Patch(
                AccessTools2.Method(typeof(OptionsVM), nameof(OptionsVM.ExecuteCloseOptions)),
                postfix: new HarmonyMethod(AccessTools2.Method(typeof(OptionsVMMixin), nameof(ExecuteCloseOptionsPostfix)), 300));
        }

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OptionsPostfix(OptionsVM __instance)
        {
            __instance.SetPropertyValue(nameof(ModOptionsSelected), false);
        }
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ExecuteCloseOptionsPostfix(OptionsVM __instance)
        {
            if (__instance.GetPropertyValue("MCMMixin") is WeakReference<OptionsVMMixin> weakReference && weakReference.TryGetTarget(out var mixin))
            {
                mixin?.ExecuteCloseOptions();
            }
        }

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OriginalExecuteCloseOptions(OptionsVM instance) => throw new NotImplementedException("It's a stub");

        private readonly ModOptionsVM _modOptions = new();
        private bool _modOptionsSelected;
        private int _descriptionWidth = 650;

        [DataSourceProperty]
        public WeakReference<OptionsVMMixin> MCMMixin => new(this);

        [DataSourceProperty]
        public ModOptionsVM ModOptions
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                ModOptionsSelected = true;
                return _modOptions;
            }
        }

        [DataSourceProperty]
        public int DescriptionWidth
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => _descriptionWidth;
            [MethodImpl(MethodImplOptions.NoInlining)]
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
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => _modOptionsSelected;
            [MethodImpl(MethodImplOptions.NoInlining)]
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
        [MethodImpl(MethodImplOptions.NoInlining)]
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
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ExecuteDone()
        {
            ModOptions.ExecuteDoneInternal(false, () =>
            {
                if (ViewModel is not null)
                    ExecuteDoneMethod?.Invoke(ViewModel);
            });
        }

        [DataSourceMethod]
        [MethodImpl(MethodImplOptions.NoInlining)]
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