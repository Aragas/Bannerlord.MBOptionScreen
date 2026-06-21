using MCM.Abstractions;
using MCM.UI.Extensions;
using MCM.UI.Utils;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Library;
using TaleWorlds.Localization;

using Debug = System.Diagnostics.Debug;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed class SettingsPropertyGroupVM : ViewModel
    {
        private bool _isExpanded = true;
        private string _groupName = string.Empty;
        private string _hintText = string.Empty;
        private string _groupNameDisplay = string.Empty;

        private ModOptionsVM MainView => SettingsVM.MainView;
        private SettingsVM SettingsVM { get; }

        public SettingsPropertyGroupDefinition SettingPropertyGroupDefinition { get; }

        public string GroupName { get => _groupName; private set => SetField(ref _groupName, value, nameof(GroupName)); }
        public SettingsPropertyGroupVM? ParentGroup { get; }
        public SettingsPropertyVM? GroupToggleSettingProperty { get; private set; }
        public string HintText { get => _hintText; private set => SetField(ref _hintText, value, nameof(HintText)); }
        public bool SatisfiesSearch
        {
            get
            {
                if (string.IsNullOrEmpty(MainView.SearchText))
                    return true;
                return GroupName.IndexOf(MainView.SearchText, StringComparison.InvariantCultureIgnoreCase) >= 0 || AnyChildSettingSatisfiesSearch;
            }
        }
        public bool AnyChildSettingSatisfiesSearch => SettingProperties.Any(x => x.SatisfiesSearch) || SettingPropertyGroups.Any(x => x.SatisfiesSearch);

        [DataSourceProperty]
        public string GroupNameDisplay { get => _groupNameDisplay; set => SetField(ref _groupNameDisplay, value, nameof(GroupNameDisplay)); }
        [DataSourceProperty]
        public MBBindingList<SettingsPropertyVM> SettingProperties { get; } = [];
        [DataSourceProperty]
        public MBBindingList<SettingsPropertyGroupVM> SettingPropertyGroups { get; } = [];
        [DataSourceProperty]
        public bool GroupToggle
        {
            get => GroupToggleSettingProperty?.BoolValue != false;
            set
            {
                if (GroupToggleSettingProperty is not null && GroupToggleSettingProperty.BoolValue != value)
                {
                    GroupToggleSettingProperty.BoolValue = value;
                    OnPropertyChanged(nameof(GroupToggle));
                    OnPropertyChanged(nameof(IsExpanded));
                    OnGroupClick();
                    OnGroupClick();
                    OnPropertyChanged(nameof(GroupNameDisplay));
                    foreach (var propSetting in SettingProperties)
                    {
                        propSetting.OnPropertyChanged(nameof(SettingsPropertyVM.IsEnabled));
                        propSetting.OnPropertyChanged(nameof(SettingsPropertyVM.IsSettingVisible));
                    }
                    foreach (var subGroup in SettingPropertyGroups)
                    {
                        subGroup.OnPropertyChanged(nameof(IsGroupVisible));
                        subGroup.OnPropertyChanged(nameof(IsExpanded));
                    }
                }
            }
        }
        [DataSourceProperty]
        public bool IsGroupVisible
        {
            get
            {
                if (!SatisfiesSearch && !AnyChildSettingSatisfiesSearch)
                    return false;
                else if (ParentGroup is not null)
                    return ParentGroup.IsExpanded && ParentGroup.GroupToggle;
                else
                    return true;
            }
        }
        [DataSourceProperty]
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (SetField(ref _isExpanded, value, nameof(IsExpanded)))
                {
                    OnPropertyChanged(nameof(IsGroupVisible));
                    foreach (var subGroup in SettingPropertyGroups)
                    {
                        subGroup.OnPropertyChanged(nameof(IsGroupVisible));
                        subGroup.OnPropertyChanged(nameof(IsExpanded));
                    }
                    foreach (var settingProp in SettingProperties)
                        settingProp.OnPropertyChanged(nameof(SettingsPropertyVM.IsSettingVisible));
                }
            }
        }
        [DataSourceProperty]
        public bool HasGroupToggle => GroupToggleSettingProperty is not null;
        
        public SettingsPropertyGroupVM(SettingsPropertyGroupDefinition definition, SettingsVM settingsVM, SettingsPropertyGroupVM? parentGroup = null)
        {
            SettingsVM = settingsVM;
            SettingPropertyGroupDefinition = definition;
            ParentGroup = parentGroup;

            AddRange(SettingPropertyGroupDefinition.SettingProperties);
            SettingPropertyGroups.AddRange(SettingPropertyGroupDefinition.SubGroups.Select(x => new SettingsPropertyGroupVM(x, SettingsVM, this)));

            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            GroupName = SettingPropertyGroupDefinition.GroupName;
            HintText = GroupToggleSettingProperty is not null && !string.IsNullOrWhiteSpace(GroupToggleSettingProperty.HintText)
                ? GroupToggleSettingProperty.HintText
                : string.Empty;
            GroupNameDisplay = GroupToggle
                ? new TextObject(GroupName).ToString()
                : new TextObject("{=SettingsPropertyGroupVM_Disabled}{GROUPNAME} (Disabled)", new()
                {
                    { "GROUPNAME", new TextObject(GroupName).ToString() }
                }).ToString();

            SettingProperties.Sort(UISettingsUtils.SettingsPropertyVMComparer);
            SettingPropertyGroups.Sort(UISettingsUtils.SettingsPropertyGroupVMComparer);
            foreach (var setting in SettingProperties)
                setting.RefreshValues();
            foreach (var setting in SettingPropertyGroups)
                setting.RefreshValues();
        }

        private void AddRange(IEnumerable<ISettingsPropertyDefinition> definitions)
        {
            foreach (var definition in definitions)
            {
                var sp = new SettingsPropertyVM(definition, SettingsVM);
                SettingProperties.Add(sp);
                sp.Group = this;

                if (sp.SettingPropertyDefinition.IsToggle)
                {
                    if (HasGroupToggle)
                        Debug.Fail($"Tried to add a group toggle to Setting Property Group {GroupName} but it already has a group toggle: {GroupToggleSettingProperty?.Name}. A Setting Property Group can only have one group toggle.");

                    // Attribute = sp.SettingPropertyDefinition;
                    GroupToggleSettingProperty = sp;
                }
            }
        }

        public void NotifySearchChanged()
        {
            foreach (var group in SettingPropertyGroups)
                group.NotifySearchChanged();
            foreach (var prop in SettingProperties)
                prop.OnPropertyChanged(nameof(SettingsPropertyVM.IsSettingVisible));
            OnPropertyChanged(nameof(IsGroupVisible));
        }

        public void OnHover() => MainView.HintText = HintText;
        public void OnHoverEnd() => MainView.HintText = string.Empty;
        public void OnGroupClick() => IsExpanded = !IsExpanded;

        public override string ToString() => GroupName;
        public override int GetHashCode() => GroupName.GetHashCode();

        public override void OnFinalize()
        {
            foreach (var settingPropertyGroup in SettingPropertyGroups)
                settingPropertyGroup.OnFinalize();
            foreach (var settingProperty in SettingProperties)
                settingProperty.OnFinalize();

            base.OnFinalize();
        }
    }
}