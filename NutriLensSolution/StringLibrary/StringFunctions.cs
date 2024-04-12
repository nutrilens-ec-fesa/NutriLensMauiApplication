using System.Globalization;
using System.Text;

namespace StringLibrary
{
    public static class StringFunctions
    {
        public static readonly char cultureDecimalSeparator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
        public static readonly char decimalToBeReplaced = cultureDecimalSeparator == ',' ? '.' : ',';

        /// <summary>
        /// Remove acentos e caracteres especiais
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var character in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(character);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(character);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Verifica se duas strings são iguais, desconsiderando acentos
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool CompareString(string a, string b)
        {
            return RemoveDiacritics(a) == RemoveDiacritics(b);
        }

        public static bool ParseDoubleValue(string value, out double result)
        {
            return double.TryParse(value.Replace(decimalToBeReplaced, cultureDecimalSeparator), out result);
        }

        public static double ParseDoubleValue(string value)
        {
            if (double.TryParse(value.Replace(decimalToBeReplaced, cultureDecimalSeparator), out double result))
                return result;
            else
                return double.NaN;
        }
    }
}
