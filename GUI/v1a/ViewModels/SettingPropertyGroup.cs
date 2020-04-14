using ModLib.Attributes;

using TaleWorlds.Library;

namespace ModLib.GUI.v1a.ViewModels
{
    public class SettingPropertyGroup : ViewModel
    {
        public const string DefaultGroupName = "Misc";

        public ModSettingsScreenVM MainView { get; private set; }
        public SettingPropertyGroupDefinition SettingPropertyGroupDefinition { get; }
        public string GroupName => SettingPropertyGroupDefinition.GroupName;
        public SettingPropertyGroupAttribute Attribute
        {
            get => SettingPropertyGroupDefinition.Attribute;
            set => SettingPropertyGroupDefinition.Attribute = value;
        }
        public SettingProperty GroupToggleSettingProperty { get; private set; } = null;
        public UndoRedoStack URS { get; private set; }
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


        [DataSourceProperty]
        public string GroupNameDisplay
        {
            get
            {
                string addition = GroupToggle ? "" : "(Disabled)";
                return $"{Attribute.GroupName} {addition}";
            }
        }
        [DataSourceProperty]
        public MBBindingList<SettingProperty> SettingProperties { get; } = new MBBindingList<SettingProperty>();
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
                    foreach (var propSetting in SettingProperties)
                    {
                        propSetting.OnPropertyChanged(nameof(SettingProperty.IsEnabled));
                        propSetting.OnPropertyChanged(nameof(SettingProperty.IsSettingVisible));
                        OnPropertyChanged(nameof(GroupNameDisplay));
                    }
                }
            }
        }
        [DataSourceProperty]
        public bool HasGroupToggle => GroupToggleSettingProperty != null;

        public SettingPropertyGroup(SettingPropertyGroupDefinition definition, ModSettingsScreenVM mainView)
        {
            MainView = mainView;
            SettingPropertyGroupDefinition = definition;
            foreach (var settingPropertyDefinition in SettingPropertyGroupDefinition.SettingProperties)
                Add(settingPropertyDefinition);
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            foreach (var setting in SettingProperties)
                setting.RefreshValues();

            OnPropertyChanged(nameof(GroupNameDisplay));
        }

        private void Add(SettingPropertyDefinition definition)
        {
            var sp = new SettingProperty(definition, MainView);
            SettingProperties.Add(sp);
            sp.Group = this;

            if (sp.GroupAttribute.IsMainToggle)
            {
                Attribute = sp.GroupAttribute;
                GroupToggleSettingProperty = sp;
            }
        }

        internal void AssignUndoRedoStack(UndoRedoStack urs)
        {
            URS = urs;
            foreach (var settingProp in SettingProperties)
                settingProp.AssignUndoRedoStack(urs);
        }

        private void OnHover()
        {
            if (MainView != null && !string.IsNullOrWhiteSpace(HintText))
                MainView.HintText = HintText;
        }

        private void OnHoverEnd()
        {
            if (MainView != null)
                MainView.HintText = "";
        }
    }
}
