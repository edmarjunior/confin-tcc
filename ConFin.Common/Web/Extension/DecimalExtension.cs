namespace ConFin.Common.Web.Extension
{
    public static class DecimalExtension
    {
        public static string ToMoney(this decimal? value, string defaultValue = "")
        {
            return value != null && value != 0 ? $"{value:N}" : defaultValue;
        }

        public static string ToMoney(this decimal value, string defaultValue = "")
        {
            return value != 0 ? $"{value:N}" : defaultValue;
        }
    }
}
