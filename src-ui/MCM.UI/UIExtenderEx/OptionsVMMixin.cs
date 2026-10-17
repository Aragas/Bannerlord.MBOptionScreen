using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.ViewModels;

using JetBrains.Annotations;

using MCM.UI.GUI.ViewModels;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace MCM.UI.UIExtenderEx
{
    [ViewModelMixin(nameof(OptionsVM.RefreshValues))]
    internal sealed class OptionsVMMixin : BaseViewModelMixin<OptionsVM>
    {
        [BUTRUnsafeAccessor(BUTRAccessorKind.Field, Name = "_categories")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ref List<ViewModel> Categories(OptionsVM _) => throw new NotImplementedException();

        // DataSource="{ModOptions}" on the inserted tab toggle and page
        [DataSourceProperty, UsedImplicitly]
        public ModOptionsVM ModOptions { get; } = new();

        // SuggestedWidth="@DescriptionWidth" on the game's DescriptionsRightPanel
        [DataSourceProperty, UsedImplicitly]
        public int DescriptionWidth
        {
            get;
            private set => SetField(ref field, value, nameof(DescriptionWidth));
        } = 650;

        public OptionsVMMixin(OptionsVM vm) : base(vm)
        {
            // At the index the MCM tab has on screen: OptionsPrefabExtension1/2 put it before the fifth tab, GameKey.
            // GetActiveCategory and the controller's tab switching index _categories by CategoryIndex
            Categories(vm).Insert(4, ModOptions);
        }

        // After OptionsVM.RefreshValues. OptionsVM calls it from its constructor, so the first call arrives once this
        // constructor has finished
        public override void OnRefresh() => ModOptions.RefreshValues();

        // Every notification OptionsVM raises, typed or not. CategoryIndex is raised through the typed overload, which a
        // PropertyChanged handler never heard; that is why this used to be a Harmony postfix on its setter
        protected override void OnViewModelPropertyChanged(string propertyName)
        {
            if (propertyName != nameof(OptionsVM.CategoryIndex) || ViewModel is not { } vm)
                return;

            var selected = vm.CategoryIndex == 4;
            ModOptions.IsDisabled = !selected;
            DescriptionWidth = selected ? 0 : 650;
        }

        // The overrides apply to every caller: the buttons, the hotkeys, the game's own code.
        // OptionsVM.ExecuteDone and ExecuteCancel both end in CloseScreen, which calls ExecuteCloseOptions, so MCM leaves
        // the screen in ExecuteCloseOptions only, once, whichever way the screen is closed

        // Set once Done saved MCM's changes, so the close that follows does not undo them
        private bool _modOptionsSaved;

        // MCM saves before the game applies its own options. ExecuteDoneInternal calls original once saved, or, when a
        // changed setting needs a restart, asks first and does nothing when declined
        [BUTRViewModelOverride(nameof(OptionsVM.ExecuteDone)), UsedImplicitly]
        private void ExecuteDone(Action original) => ModOptions.ExecuteDoneInternal(false, () =>
        {
            _modOptionsSaved = true;
            original();
        });

        // No override of ExecuteCancel: the game asks for confirmation when its own options changed and closes through
        // ExecuteCloseOptions once confirmed. Overriding it undid MCM's changes before the question was answered, and
        // answering No left the screen open with MCM finalized

        [BUTRViewModelOverride(nameof(OptionsVM.ExecuteCloseOptions)), UsedImplicitly]
        private void ExecuteCloseOptions(Action original)
        {
            if (_modOptionsSaved)
                original();
            else
                ModOptions.ExecuteCancelInternal(false, original);
        }
    }
}
