using System;

using TaleWorlds.Core;
using TaleWorlds.Library;

namespace ModLib.Debugging
{
    public static class ModDebug
    {
        public static void ShowError(string message, string title = "", Exception? exception = null)
        {
            InformationManager.DisplayMessage(new($"ModLibV13: {message}! {exception}", Colors.Red));
        }

        public static void ShowMessage(string message, string title = "", bool nonModal = false)
        {
            InformationManager.DisplayMessage(new($"ModLibV13: {message}", Colors.Yellow));
        }
        public static void LogError(string error, Exception? ex = null) { }
    }
}