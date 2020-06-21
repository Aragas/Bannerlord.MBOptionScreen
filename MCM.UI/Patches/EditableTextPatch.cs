using HarmonyLib;

using System;
using System.Reflection;

using TaleWorlds.TwoDimension;

namespace MCM.UI.Patches
{
    /// <summary>
    /// Cursor in the editable text box sometimes creates an exception with the string length or something, suppress it
    /// </summary>
    public static class EditableTextPatch
    {
        public static MethodBase GetCursorPositionMethod { get; } =
            AccessTools.Method(typeof(EditableText), "GetCursorPosition");

        public static Exception? GetCursorPosition(Exception? __exception) => null;
    }
}