using System;
using System.Text;

namespace ModLib
{
    public static class ExceptionExtensionMethods
    {
        public static string ToStringFull(this Exception ex)
        {
            if (ex != null)
                return GetString(ex);
            else
                return string.Empty;
        }

        private static string GetString(Exception ex)
        {
            var sb = new StringBuilder();
            GetStringRecursive(ex, sb);
            sb.AppendLine();
            sb.AppendLine("Stack trace:");
            sb.AppendLine(ex.StackTrace);
            return sb.ToString();
        }

        private static void GetStringRecursive(Exception ex, StringBuilder sb)
        {
            sb.Append(ex.GetType().Name).AppendLine(":");
            sb.AppendLine(ex.Message);
            if (ex.InnerException != null)
            {
                sb.AppendLine();
                GetStringRecursive(ex.InnerException, sb);
            }
        }
    }
}