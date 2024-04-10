using System.Globalization;
using System.Text;

namespace StringLibrary
{
    public static class StringFunctions
    {
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
            //if (string.IsNullOrEmpty(text))
            //    return text;

            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < text.Length; i++)
            //{
            //    if (text[i] > 255)
            //        sb.Append(text[i]);
            //    else
            //        sb.Append(s_Diacritics[text[i]]);
            //}

            //return sb.ToString();
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
    }
}
