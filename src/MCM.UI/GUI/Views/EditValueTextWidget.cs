using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using MCM.Abstractions.Settings;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using TaleWorlds.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace MCM.UI.GUI.Views
{
    internal sealed class EditValueTextWidget : EditableTextWidget
    {
        private static readonly AccessTools.FieldRef<EditableTextWidget, EditableText>? _editableText =
            AccessTools2.FieldRefAccess<EditableTextWidget, EditableText>("_editableText");

        private readonly EditableText? _editableWidget;

        [DataSourceProperty]
        public SettingType SettingType { get; set; } = SettingType.Float;
        [DataSourceProperty]
        public float MaxValue { get; set; } = 0f;
        [DataSourceProperty]
        public float MinValue { get; set; } = 0f;

        public EditValueTextWidget(UIContext context) : base(context)
        {
            if (_editableText != null)
                _editableWidget = _editableText(this);
        }

        public override void HandleInput(IReadOnlyList<int> lastKeysPressed)
        {
            if (Input.IsKeyDown(InputKey.LeftControl) && Input.IsKeyPressed(InputKey.V))
            {
                base.HandleInput(lastKeysPressed);
                return;
            }

            if (lastKeysPressed.Count > 0)
            {
                foreach (var key in lastKeysPressed)
                {
                    if (SettingType == SettingType.String)
                    {
                        base.HandleInput(lastKeysPressed);
                    }
                    else if (Enum.IsDefined(typeof(KeyCodes), key))
                    {
                        if (key == (int) KeyCodes.Minus)
                        {
                            if (_editableWidget?.SelectedTextBegin != 0)
                                continue;
                        }
                        else if (SettingType == SettingType.Float)
                        {
                            // Handle input for float types
                            if (key == (int) KeyCodes.Decimal)
                            {
                                if (RealText.Any(ch => ch == '.'))
                                    continue;
                            }
                        }
                        else if (SettingType == SettingType.Int)
                        {
                            // Handle input for int types.
                            if (key == (int) KeyCodes.Decimal)
                                continue;
                        }
                        base.HandleInput(lastKeysPressed);

                        if (SettingType == SettingType.Float)
                        {
                            if (float.TryParse(RealText, out var value))
                            {
                                var newVal = value;
                                if (value > MaxValue)
                                    newVal = MaxValue;
                                else if (value < MinValue)
                                    newVal = MinValue;
                                if (newVal != value)
                                {
                                    var format = SettingType == SettingType.Int ? "0" : "0.00";
                                    RealText = newVal.ToString(format);
                                    _editableWidget?.SetCursorPosition(0, true);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                base.HandleInput(lastKeysPressed);
            }
        }

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private enum KeyCodes
        {
            Backspace = 8,
            Minus = 45,
            Decimal = 46,
            Zero = 48,
            One = 49,
            Two = 50,
            Three = 51,
            Four = 52,
            Five = 53,
            Six = 54,
            Seven = 55,
            Eight = 56,
            Nine = 57
        }
    }
}