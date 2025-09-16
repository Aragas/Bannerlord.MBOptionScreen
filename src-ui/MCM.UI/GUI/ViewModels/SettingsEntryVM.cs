using MCM.Abstractions;

using System;

using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace MCM.UI.GUI.ViewModels;

internal sealed class SettingsEntryVM : ViewModel
{
    private readonly Action<SettingsEntryVM> _executeSelect;

    private bool? _isSelected;

    public string Id => UnavailableSetting?.Id ?? SettingsVM?.SettingsDefinition.SettingsId ?? "ERROR";

    [DataSourceProperty]
    public string DisplayName => UnavailableSetting?.DisplayName ?? SettingsVM?.DisplayName ?? "ERROR";

    [DataSourceProperty]
    public bool IsSelected
    {
        get => _isSelected ?? SettingsVM?.IsSelected ?? false;
        set
        {
            if (SettingsVM is not null)
            {
                SettingsVM.IsSelected = value;
            }
            else
            {
                _isSelected = value;
            }

            OnPropertyChanged(nameof(IsSelected));
        }
    }

    public SettingsVM? SettingsVM { get; }
    public UnavailableSetting? UnavailableSetting { get; }

    public SettingsEntryVM(UnavailableSetting unavailableSetting, Action<SettingsEntryVM> command)
    {
        UnavailableSetting = unavailableSetting;
        _isSelected = false;
        _executeSelect = command;
    }

    public SettingsEntryVM(SettingsVM settingsVM, Action<SettingsEntryVM> command)
    {
        SettingsVM = settingsVM;
        _executeSelect = command;
    }

    public void ExecuteSelect() => _executeSelect.Invoke(this);
}