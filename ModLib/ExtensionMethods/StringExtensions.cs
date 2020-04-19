namespace ModLib
{
    public static class StringExtensions
    {
        public static int Count(this string str, char c)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(str))
            {
                foreach (var ch in str)
                {
                    if (ch == c) count++;
                }
            }
            return count;
        }

        public static string Last(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length > 0)
                {
                    int index = str.Length - 1;
                    return str[index].ToString();
                }
            }

            return string.Empty;
        }
    }
}
