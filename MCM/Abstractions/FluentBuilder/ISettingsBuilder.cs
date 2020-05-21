using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Base.PerCharacter;

using System;
using System.ComponentModel;

namespace MCM.Abstractions.FluentBuilder
{
    public interface ISettingsBuilder
    {
        ISettingsBuilder SetFolderName(string value);
        ISettingsBuilder SetSubFolder(string value);
        ISettingsBuilder SetFormat(string value);
        ISettingsBuilder SetUIVersion(int value);
        ISettingsBuilder SetSubGroupDelimiter(char value);
        ISettingsBuilder SetOnPropertyChanged(PropertyChangedEventHandler value);

        ISettingsBuilder CreateGroup(string name, Action<ISettingsPropertyGroupBuilder> action);

        FluentGlobalSettings BuildAsGlobal();
        FluentPerCharacterSettings BuildAsPerCharacter();
    }
}