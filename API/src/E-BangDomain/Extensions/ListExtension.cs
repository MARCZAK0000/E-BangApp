namespace E_BangDomain.Extensions
{
    public static class ListExtension
    {
        public static List<T> Empty<T>(this List<T> values)
        {
            values = [];
            return values;
        }
    }
}
