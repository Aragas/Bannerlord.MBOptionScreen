using System;
using System.Diagnostics.CodeAnalysis;

using TaleWorlds.Core;
using TaleWorlds.Library;

namespace ModLib.Debugging
{
    public static class ModDebug
    {
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void ShowError(string message, string title = "", Exception? exception = null)
        {
            InformationManager.DisplayMessage(new($"ModLibV13: {message}! {exception}", Colors.Red));
        }

        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void ShowMessage(string message, string title = "", bool nonModal = false)
        {
            InformationManager.DisplayMessage(new($"ModLibV13: {message}", Colors.Yellow));
        }

        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void LogError(string error, Exception? ex = null) { }
    }
}