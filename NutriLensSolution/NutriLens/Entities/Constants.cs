namespace NutriLens.Entities
{
    public static class Constants
    {
        public static readonly string kcalUnit = "kcal";
        public static readonly string kJUnit = "kJ";
        public static readonly double kcalToKJFactor = 4.184;

        public static readonly char cultureDecimalSeparator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
        public static readonly char decimalToBeReplaced = cultureDecimalSeparator == ',' ? '.' : ',';
    }
}
