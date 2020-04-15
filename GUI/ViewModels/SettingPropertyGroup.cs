using ModLib.Attributes;
using System;
using TaleWorlds.Library;
using System.Linq;

namespace ModLib.GUI.ViewModels
{
    public class SettingPropertyGroup : ViewModel
    {
        private string groupNameOverride = "";
        private bool _isExpanded = true;
        public const string DefaultGroupName = "Misc";
        public SettingProperty GroupToggleSettingProperty { get; private set; } = null;
        public SettingPropertyGroupAttribute Attribute { get; private set; }
        public UndoRedoStack URS { get; private set; }
        public ModSettingsScreenVM ScreenVM { get; private set; }
        public SettingPropertyGroup ParentGroup { get; set; } = null;
        public string HintText
        {
            get
            {
                if (GroupToggleSettingProperty != null && !string.IsNullOrWhiteSpace(GroupToggleSettingProperty.HintText))
                {
                    return $"{GroupToggleSettingProperty.HintText}";
                }
                return "";
            }
        }
        public bool SatisfiesSearch
        {
            get
            {
                if (ScreenVM == null || ScreenVM.SearchText == "")
                    return true;
                return GroupName.ToLower().Contains(ScreenVM.SearchText.ToLower()) || AnyChildSettingSatisfiesSearch;
            }
        }
        public bool AnyChildSettingSatisfiesSearch
        {
            get
            {
                return SettingProperties.Any((x) => x.SatisfiesSearch) || SettingPropertyGroups.Any((x) => x.SatisfiesSearch);
            }
        }

        public string GroupName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(groupNameOverride))
                    return Attribute.GroupName;
                else
                    return groupNameOverride;
            }
        }

        [DataSourceProperty]
        public string GroupNameDisplay
        {
            get
            {
                string addition = GroupToggle ? "" : "(Disabled)";
                return $"{GroupName} {addition}";
            }
        }
        [DataSourceProperty]
        public MBBindingList<SettingProperty> SettingProperties { get; } = new MBBindingList<SettingProperty>();
        [DataSourceProperty]
        public MBBindingList<SettingPropertyGroup> SettingPropertyGroups { get; } = new MBBindingList<SettingPropertyGroup>();
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
                    OnPropertyChanged();
                    OnPropertyChanged("IsExpanded");
                    OnGroupClick();
                    OnGroupClick();
                    OnPropertyChanged("GroupNameDisplay");
                    foreach (var propSetting in SettingProperties)
                    {
                        propSetting.OnPropertyChanged("IsEnabled");
                        propSetting.OnPropertyChanged("IsSettingVisible");
                    }
                    foreach (var subGroup in SettingPropertyGroups)
                    {
                        subGroup.OnPropertyChanged("IsGroupVisible");
                        subGroup.OnPropertyChanged("IsExpanded");
                    }
                }
            }
        }
        [DataSourceProperty]
        public bool HasGroupToggle => GroupToggleSettingProperty != null;
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
                    OnPropertyChanged();
                    OnPropertyChanged("IsGroupVisible");
                    foreach (var subGroup in SettingPropertyGroups)
                    {
                        subGroup.OnPropertyChanged("IsGroupVisible");
                        subGroup.OnPropertyChanged("IsExpanded");
                    }
                    foreach (var settingProp in SettingProperties)
                        settingProp.OnPropertyChanged("IsSettingVisible");
                }
            }
        }

        public SettingPropertyGroup(SettingPropertyGroupAttribute attribute, string groupNameOverride = "")
        {
            Attribute = attribute;
            this.groupNameOverride = groupNameOverride;
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            foreach (var setting in SettingProperties)
                setting.RefreshValues();

            OnPropertyChanged("GroupNameDisplay");
        }

        public void Add(SettingProperty sp)
        {
            SettingProperties.Add(sp);
            sp.Group = this;

            if (sp.GroupAttribute.IsMainToggle)
            {
                if (HasGroupToggle)
                    throw new Exception($"Tried to add a group toggle to setting property group \"{GroupName}\" but it already has a group toggle: {GroupToggleSettingProperty.Name}. A setting property group can only have one group toggle.");

                Attribute = sp.GroupAttribute;
                GroupToggleSettingProperty = sp;
            }
        }

        public void AssignUndoRedoStack(UndoRedoStack urs)
        {
            URS = urs;

            foreach (var subGroup in SettingPropertyGroups)
                subGroup.AssignUndoRedoStack(urs);

            foreach (var settingProp in SettingProperties)
                settingProp.AssignUndoRedoStack(urs);
        }

        public void SetScreenVM(ModSettingsScreenVM screenVM)
        {
            ScreenVM = screenVM;

            foreach (var subGroup in SettingPropertyGroups)
                subGroup.SetScreenVM(ScreenVM);

            foreach (var settingProperty in SettingProperties)
                settingProperty.SetScreenVM(ScreenVM);
        }

        public void SetParentGroup(SettingPropertyGroup parentGroup)
        {
            ParentGroup = parentGroup;

            if (SettingPropertyGroups.Count > 0)
            {
                foreach (var subGroup in SettingPropertyGroups)
                    subGroup.SetParentGroup(this);
            }
        }

        public void NotifySearchChanged()
        {
            if (SettingPropertyGroups.Count > 0)
            {
                foreach (var group in SettingPropertyGroups)
                    group.NotifySearchChanged();
            }
            if (SettingProperties.Count > 0)
            {
                foreach (var prop in SettingProperties)
                    prop.OnPropertyChanged("IsSettingVisible");
            }
            OnPropertyChanged("IsGroupVisible");
        }

        private void OnHover()
        {
            if (ScreenVM != null && !string.IsNullOrWhiteSpace(HintText))
                ScreenVM.HintText = HintText;
        }

        private void OnHoverEnd()
        {
            if (ScreenVM != null)
                ScreenVM.HintText = "";
        }

        private void OnGroupClick()
        {
            IsExpanded = !IsExpanded;
        }

        public override string ToString()
        {
            return GroupName;
        }
    }
}
