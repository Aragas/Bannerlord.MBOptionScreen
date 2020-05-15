using MCM.Abstractions.Settings.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using MCM.Abstractions.Settings;

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