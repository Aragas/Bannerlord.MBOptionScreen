using Bannerlord.BUTR.Shared.Helpers;

using MCM.Abstractions.Settings.Models;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed class SettingsPropertyGroupVM : ViewModel
    {
        private bool _isExpanded = true;

        private ModOptionsVM MainView => SettingsVM.MainView;
        private SettingsVM SettingsVM { get; }

        public SettingsPropertyGroupDefinition SettingPropertyGroupDefinition { get; }
        public string GroupName => SettingPropertyGroupDefinition.GroupName;
        public SettingsPropertyGroupVM? ParentGroup { get; }
        public SettingsPropertyVM? GroupToggleSettingProperty { get; private set; }
        public string HintText
        {
            get
            {
                if (GroupToggleSettingProperty is not null && !string.IsNullOrWhiteSpace(GroupToggleSettingProperty.HintText))
                {
                    return GroupToggleSettingProperty.HintText;
                }
                return string.Empty;
            }
        }
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
        public string GroupNameDisplay => GroupToggle
            ? GroupName
            : TextObjectHelper.Create("{=SettingsPropertyGroupVM_Disabled}{GROUPNAME} (Disabled)", new Dictionary<string, TextObject>
            {
                { "GROUPNAME", TextObjectHelper.Create(GroupName) }
            }).ToString();
        [DataSourceProperty]
        public MBBindingList<SettingsPropertyVM> SettingProperties { get; } = new();
        [DataSourceProperty]
        public MBBindingList<SettingsPropertyGroupVM> SettingPropertyGroups { get; } = new();
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
        public bool HasSettingsProperties => SettingProperties.Count > 0;
        [DataSourceProperty]
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    OnPropertyChanged(nameof(IsExpanded));
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
            foreach (var settingPropertyDefinition in SettingPropertyGroupDefinition.SettingProperties)
                Add(settingPropertyDefinition);
            foreach (var settingPropertyDefinition in SettingPropertyGroupDefinition.SubGroups.Reverse())
                SettingPropertyGroups.Add(new SettingsPropertyGroupVM(settingPropertyDefinition, SettingsVM, this));

            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            foreach (var setting in SettingProperties)
                setting.RefreshValues();
            foreach (var setting in SettingPropertyGroups)
                setting.RefreshValues();

            OnPropertyChanged(nameof(GroupNameDisplay));
        }

        private void Add(ISettingsPropertyDefinition definition)
        {
            var sp = new SettingsPropertyVM(definition, SettingsVM);
            SettingProperties.Add(sp);
            sp.Group = this;

            if (sp.SettingPropertyDefinition.IsToggle)
            {
                if (HasGroupToggle)
                    throw new Exception($"Tried to add a group toggle to Setting Property Group {GroupName} but it already has a group toggle: {GroupToggleSettingProperty?.Name}. A Setting Property Group can only have one group toggle.");

                // Attribute = sp.SettingPropertyDefinition;
                GroupToggleSettingProperty = sp;
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

        public void OnResetStart()
        {
            foreach (var setting in SettingProperties)
                setting.OnResetStart();
            foreach (var setting in SettingPropertyGroups)
                setting.OnResetStart();
        }
        public void OnResetEnd()
        {
            foreach (var setting in SettingProperties)
                setting.OnResetEnd();
            foreach (var setting in SettingPropertyGroups)
                setting.OnResetEnd();
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
