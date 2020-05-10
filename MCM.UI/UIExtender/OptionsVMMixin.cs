#if UIE
using HarmonyLib;

using MCM.UI.GUI.ViewModels;

using System;
using System.ComponentModel;

using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

using UIExtenderLib.Interface;

namespace MCM.UI.UIExtender
{
    [ViewModelMixin]
    internal class OptionsVMMixin : BaseViewModelMixin<OptionsVM>
    {
        private readonly ModOptionsVM _modOptions;
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
                if (_vm.TryGetTarget(out var vm))
                    vm.OnPropertyChanged(nameof(DescriptionWidth));
            }
        }

        private bool ModOptionsSelected
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
            _modOptions = new ModOptionsVM();
            _modOptions.PropertyChanged += _modOptions_PropertyChanged;
        }

        private void _modOptions_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_vm.TryGetTarget(out var vm))
                vm.OnPropertyChanged(e.PropertyName);
        }

        public override void OnFinalize()
        {
            _modOptions.PropertyChanged -= _modOptions_PropertyChanged;
            base.OnFinalize();
        }

        [DataSourceMethod]
        public void ExecuteCloseOptions()
        {
            ModOptions.ExecuteCancelInternal(false);
            if (_vm.TryGetTarget(out var vm))
                vm.ExecuteCloseOptions();
        }

        [DataSourceMethod]
        public void ExecuteDone()
        {
            ModOptions.ExecuteDoneInternal(false, () =>
            {
                if (_vm.TryGetTarget(out var vm))
                    AccessTools.Method(typeof(OptionsVM), "ExecuteDone").Invoke(vm, Array.Empty<object>());
            });
        }

        [DataSourceMethod]
        public void ExecuteCancel()
        {
            ModOptions.ExecuteCancelInternal(false, () =>
            {
                if (_vm.TryGetTarget(out var vm))
                    AccessTools.Method(typeof(OptionsVM), "ExecuteCancel").Invoke(vm, Array.Empty<object>());
            });
        }
    }
}
#endif