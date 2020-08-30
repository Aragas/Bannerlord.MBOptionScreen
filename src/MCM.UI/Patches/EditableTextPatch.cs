using HarmonyLib;

using System;

using TaleWorlds.TwoDimension;

namespace MCM.UI.Patches
{
    /// <summary>
    /// Cursor in the editable text box sometimes creates an exception with the string length or something, suppress it
    /// </summary>
    internal static class EditableTextPatch
    {
        public static void Patch(Harmony harmony)
        {
            harmony.Patch(
                AccessTools.Method(typeof(EditableText), "GetCursorPosition"),
                finalizer: new HarmonyMethod(typeof(EditableTextPatch), nameof(GetCursorPosition)));
        }

        private static Exception? GetCursorPosition(Exception? __exception) => null;
    }
}