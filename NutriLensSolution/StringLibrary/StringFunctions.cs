using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

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

        /// <summary>
        /// Retorna uma string a partir de um double nullable, caso seja nulo, retorna 0
        /// </summary>
        /// <param name="value">Valor em double a ser obtida a string</param>
        /// <param name="decimalPlaces">Quantidade de casas decimais desejadas na string, por padrão, considera-se 2</param>
        /// <returns></returns>
        public static string GetRoundDoubleString(double? value, int decimalPlaces = 2)
        {
            if (value == null)
                return "0";

            return Math.Round((double)value, decimalPlaces).ToString();
        }

        /// <summary>
        /// Remove todos os acentos de uma string.
        /// </summary>
        /// <param name="text">string contendo acentos ou não.</param>
        /// <returns>Uma string sem acentos.</returns>
        public static string RemoveAccentsFromString(string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        /// <summary>
        /// Classe que contém os métodos para execução do algoritmo de Herbert Levenshtein,
        /// algoritmo popular para comparar duas strings a fim de se ter um retorno do quanto
        /// duas strings são parecidas.
        /// </summary>
        public static class HerbertL_Algorithm
        {
            #region Métodos privados

            private static int LevenshteinDistance(string source, string target)
            {
                if (string.IsNullOrEmpty(source))
                {
                    if (string.IsNullOrEmpty(target)) return 0;
                    return target.Length;
                }
                if (string.IsNullOrEmpty(target)) return source.Length;

                if (source.Length > target.Length)
                {
                    var temp = target;
                    target = source;
                    source = temp;
                }

                var m = target.Length;
                var n = source.Length;
                var distance = new int[2, m + 1];

                for (var j = 1; j <= m; j++) distance[0, j] = j;

                var currentRow = 0;
                for (var i = 1; i <= n; ++i)
                {
                    currentRow = i & 1;
                    distance[currentRow, 0] = i;
                    var previousRow = currentRow ^ 1;
                    for (var j = 1; j <= m; j++)
                    {
                        var cost = (target[j - 1] == source[i - 1] ? 0 : 1);
                        distance[currentRow, j] = Math.Min(Math.Min(
                                    distance[previousRow, j] + 1,
                                    distance[currentRow, j - 1] + 1),
                                    distance[previousRow, j - 1] + cost);
                    }
                }
                return distance[currentRow, m];
            }
            private static double Invert(double min, double max)
            {
                return max - min;
            }

            #endregion

            /// <summary>
            /// Método original para uso do algoritmo. Compara duas strings sem realizar nenhum
            /// tratamento prévio nas strings.
            /// </summary>
            /// <param name="a">string A</param>
            /// <param name="b">string B</param>
            /// <returns>Um byte de 0 a 255 indicando o quão parecida são as duas strings</returns>
            public static byte CompareOriginal(string a, string b)
            {
                double distance = LevenshteinDistance(a, b);
                if (distance == 0) return 255;
                double length = Math.Max(a.Length, b.Length);
                if (distance == length) return 0;

                double inverted = Invert(distance, length);
                byte percent = (byte)((inverted / length) * 255);
                return percent;
            }

            /// <summary>
            /// Método alternativo para uso do algoritmo. Antes de comparar as strings são removidos
            /// os acentos, os espaços excedentes e as letras trocadas para maiúsculas.
            /// </summary>
            /// <param name="a">string A</param>
            /// <param name="b">string B</param>
            /// <returns>Um byte de 0 a 255 indicando o quão parecida são as duas strings</returns>
            public static byte CompareEdited(string a, string b)
            {
                a = RemoveAccentsFromString(a).ToUpper().Trim();
                b = RemoveAccentsFromString(b).ToUpper().Trim();
                return CompareOriginal(a, b);
            }

            /// <summary>
            /// Método alternativo para uso do algoritmo. Antes de comparar as strings são removidos
            /// os acentos, os espaços excedentes e as letras trocadas para maiúsculas, além de também
            /// remover quaisquer preposições singulares.
            /// </summary>
            /// <param name="a">string A</param>
            /// <param name="b">string B</param>
            /// <returns>Um byte de 0 a 255 indicando o quão parecida são as duas strings</returns>
            public static byte CompareEditedPrepRegex(string a, string b)
            {
                Regex regexPreposicao = new Regex(@" D[AEO] ", RegexOptions.IgnoreCase);

                while (regexPreposicao.IsMatch(a))
                    a = a.Replace(regexPreposicao.Match(a).Value, " ");
                while (regexPreposicao.IsMatch(b))
                    b = b.Replace(regexPreposicao.Match(b).Value, " ");

                a = RemoveAccentsFromString(a).ToUpper().Trim();
                b = RemoveAccentsFromString(b).ToUpper().Trim();

                return CompareOriginal(a, b);
            }
        }
    }
}
