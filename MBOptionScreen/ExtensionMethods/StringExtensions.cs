namespace MBOptionScreen.ExtensionMethods
{
    public static class StringExtensions
    {
        public static int Count(this string str, char c)
        {
            var count = 0;
            foreach (var ch in str)
            {
                if (ch == c) count++;
            }
            return count;
        }

        public static string Last(this string str)
        {
            if (str.Length > 0)
            {
                var index = str.Length - 1;
                return str[index].ToString();
            }
            else
                return string.Empty;
        }
    }
}