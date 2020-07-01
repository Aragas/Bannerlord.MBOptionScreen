using System;

namespace ModLib.Debugging
{
    public static class ModDebug
    {
        public static void ShowError(string message, string title = string.Empty, Exception exception = null) { }
        public static void ShowMessage(string message, string title = string.Empty, bool nonModal = false) { }
        public static void LogError(string error, Exception ex = null) { }
    }
}