using BUTR.DependencyInjection;

using MCM.Abstractions.Base.Global;
using MCM.Abstractions.Base.PerCampaign;
using MCM.Abstractions.Base.PerSave;

using System;
using System.ComponentModel;

namespace MCM.Abstractions.FluentBuilder
{
    public abstract class BaseSettingsBuilder : ISettingsBuilder
    {
        public static ISettingsBuilder? Create(string id, string displayName) =>
            GenericServiceProvider.GetService<ISettingsBuilderFactory>()?.Create(id, displayName);

        public abstract ISettingsBuilder SetFolderName(string value);
        public abstract ISettingsBuilder SetSubFolder(string value);
        public abstract ISettingsBuilder SetFormat(string value);
        public abstract ISettingsBuilder SetUIVersion(int value);
        public abstract ISettingsBuilder SetSubGroupDelimiter(char value);
        public abstract ISettingsBuilder SetOnPropertyChanged(PropertyChangedEventHandler value);

        public abstract ISettingsBuilder CreateGroup(string name, Action<ISettingsPropertyGroupBuilder> builder);
        public abstract ISettingsBuilder CreatePreset(string id, string name, Action<ISettingsPresetBuilder> builder);

        public abstract ISettingsBuilder WithoutDefaultPreset();

        public abstract FluentGlobalSettings BuildAsGlobal();
        public abstract FluentPerSaveSettings BuildAsPerSave();
        public abstract FluentPerCampaignSettings BuildAsPerCampaign();
    }
}