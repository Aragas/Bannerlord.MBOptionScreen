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
        public string GroupName => SettingPropertyGroupDefinition.DisplayGroupName.ToString();
        public SettingsPropertyGroupVM? ParentGroup { get; }
        public SettingsPropertyVM GroupToggleSettingProperty { get; private set; } = default!;
        public string HintText
        {
            get
            {
                if (GroupToggleSettingProperty != null && !string.IsNullOrWhiteSpace(GroupToggleSettingProperty.HintText))
                {
                    return GroupToggleSettingProperty.HintText;
                }
                return "";
            }
        }
        public bool SatisfiesSearch
        {
            get
            {
                if (MainView == null || string.IsNullOrEmpty(MainView.SearchText))
                    return true;
                return GroupName.IndexOf(MainView.SearchText, StringComparison.OrdinalIgnoreCase) >= 0 || AnyChildSettingSatisfiesSearch;
            }
        }
        public bool AnyChildSettingSatisfiesSearch => SettingProperties.Any(x => x.SatisfiesSearch) || SettingPropertyGroups.Any(x => x.SatisfiesSearch);

        [DataSourceProperty]
        public string GroupNameDisplay => GroupToggle
            ? GroupName
            : new TextObject("{=SettingsPropertyGroupVM_Disabled}{GROUPNAME} (Disabled)", new Dictionary<string, TextObject>()
            {
                {"GROUPNAME", new TextObject(GroupName)}
            }).ToString();
        [DataSourceProperty]
        public MBBindingList<SettingsPropertyVM> SettingProperties { get; } = new MBBindingList<SettingsPropertyVM>();
        [DataSourceProperty]
        public MBBindingList<SettingsPropertyGroupVM> SettingPropertyGroups { get; } = new MBBindingList<SettingsPropertyGroupVM>();
        [DataSourceProperty]
        public bool GroupToggle
        {
            get
            {
                if (GroupToggleSettingProperty != null)
                    return GroupToggleSettingProperty.BoolValue;
                else
                    return true;
            }
            set
            {
                if (GroupToggleSettingProperty != null && GroupToggleSettingProperty.BoolValue != value)
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
                else if (ParentGroup != null)
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
        public bool HasGroupToggle => GroupToggleSettingProperty != null;

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

            if (sp.SettingPropertyDefinition.IsMainToggle)
            {
                if (HasGroupToggle)
                    throw new Exception($"Tried to add a group toggle to Setting Property Group {GroupName} but it already has a group toggle: {GroupToggleSettingProperty.Name}. A Setting Property Group can only have one group toggle.");

                //Attribute = sp.SettingPropertyDefinition;
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
        private void OnHover() { if (MainView != null && !string.IsNullOrWhiteSpace(HintText)) MainView.HintText = HintText; }
        private void OnHoverEnd() { if (MainView != null) MainView.HintText = ""; }
        private void OnGroupClick() => IsExpanded = !IsExpanded;

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
