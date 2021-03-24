namespace IntegrateCRM.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmptyOrNone(this string source)
        {
            return source.IsNullOrEmpty()  || source.ToLowerInvariant() == "none";
        }
        public static bool IsNullOrEmpty(this string source)
        {
            return source == null || source == string.Empty;
        }
    }
}
