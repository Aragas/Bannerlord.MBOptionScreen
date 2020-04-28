using System;

namespace ModLib.Debugging
{
    public static class ModDebug
    {
        public static void ShowError(string message, string title = "", Exception exception = null) { }

        public static void ShowMessage(string message, string title = "", bool nonModal = false) { }

        public static void LogError(string error, Exception ex = null) { }
    }
}