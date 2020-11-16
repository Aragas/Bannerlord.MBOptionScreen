using HarmonyLib;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

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

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Exception? GetCursorPosition(Exception? __exception) => null;
    }
}