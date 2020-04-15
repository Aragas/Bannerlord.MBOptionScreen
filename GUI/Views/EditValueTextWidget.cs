using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaleWorlds.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace ModLib.GUI.Views
{
    public class EditValueTextWidget : EditableTextWidget
    {
        [DataSourceProperty]
        public SettingType SettingType { get; set; } = SettingType.Float;
        [DataSourceProperty]
        public float MaxValue { get; set; } = 0f;
        [DataSourceProperty]
        public float MinValue { get; set; } = 0f;

        public EditValueTextWidget(UIContext context) : base(context)
        {
        }

        public override void HandleInput(IReadOnlyList<int> lastKeysPressed)
        {
            if (Input.IsKeyDown(InputKey.LeftControl) && Input.IsKeyPressed(InputKey.V))
                return;

            if (lastKeysPressed.Count > 0)
            {
                for (int i = 0; i < lastKeysPressed.Count; i++)
                {
                    int key = lastKeysPressed[i];
                    if (Enum.IsDefined(typeof(KeyCodes), key))
                    {
                        if (SettingType == SettingType.Float)
                        {
                            //Handle input for float types
                            if (key == (int)KeyCodes.Decimal)
                            {
                                if (RealText.Count('.') >= 1)
                                    continue;
                            }
                        }
                        else if (SettingType == SettingType.Int)
                        {
                            //Handle input for int types.
                            if (key == (int)KeyCodes.Decimal)
                                continue;
                        }
                        base.HandleInput(lastKeysPressed);
                        float value;
                        float.TryParse(RealText, out value);
                        float newVal = value;
                        if (value > MaxValue)
                            newVal = MaxValue;
                        else if (value < MinValue)
                            newVal = MinValue;
                        if (newVal != value)
                        {
                            string format = SettingType == SettingType.Int ? "0" : "0.00";
                            RealText = newVal.ToString(format);
                        }
                    }
                }
            }
        }


        private enum KeyCodes
        {
            Zero = 48,
            One = 49,
            Two = 50,
            Three = 51,
            Four = 52,
            Five = 53,
            Six = 54,
            Seven = 55,
            Eight = 56,
            Nine = 57,
            Decimal = 46
        }
    }
}
