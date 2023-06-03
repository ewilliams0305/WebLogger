namespace WebLogger.Generators
{
    public static class StringExtensions
    {
        public static string RemoveWhiteSpace(this string str)
        {
            var split = str.Split(' ');
            var joined = string.Join("", split);

            return joined;
        }
    }
}