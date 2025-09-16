using Bannerlord.ButterLib.HotKeys;

using TaleWorlds.InputSystem;

using HotKeyManager = Bannerlord.ButterLib.HotKeys.HotKeyManager;

namespace MCM.UI.HotKeys;

public class ResetValueToDefault : HotKeyBase
{
    protected override string DisplayName { get; }
    protected override string Description { get; }
    protected override InputKey DefaultKey { get; }
    protected override string Category { get; }

    public ResetValueToDefault() : base(nameof(ResetValueToDefault))
    {
        DisplayName = "{=HOV8WIcBrb}Reset Mod Options Value to Default";
        Description = "{=2d99VmOZZH}Resets a value in Mod Options menu to its default value when hovered.";
        DefaultKey = InputKey.R;
        Category = HotKeyManager.Categories[HotKeyCategory.MenuShortcut];
    }
}