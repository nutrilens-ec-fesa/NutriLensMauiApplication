using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

namespace DateTimeLibrary
{
    public static class DateTimeFunctions
    {
        private static CultureInfo _culture = new CultureInfo("pt-BR");

        /// <summary>
        /// Obtém o número da semana, baseado em DateTime.MinValue.
        /// Sempre que o número for inteiro, quer dizer que é o primeiro dia da detemrinada semana.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private static double GetWeekNumber(DateTime dateTime)
        {
            return (int)Math.Round((dateTime - DateTime.MinValue).TotalDays + 1) / (double)7;
        }

        /// <summary>
        /// Obtém o dia da semana a partir de uma data
        /// </summary>
        /// <param name="dateTime">Data</param>
        /// <returns></returns>
        public static string GetWeekDayNameByDateTime(DateTime dateTime)
        {
            return _culture.DateTimeFormat.GetDayName(dateTime.DayOfWeek);
        }

        /// <summary>
        /// Obtém o mês a partir de uma data
        /// </summary>
        /// <param name="dateTime">Data</param>
        /// <returns></returns>
        public static string GetMonthNameByDateTime(DateTime dateTime)
        {
            return _culture.DateTimeFormat.GetMonthName(dateTime.Month);
        }

        /// <summary>
        /// Obtém o último dia da semana da data passada como parâmetro
        /// </summary>
        /// <param name="dateTime">Data</param>
        /// <returns></returns>
        public static DateTime GetLastDayOfWeekByDateTime(DateTime dateTime)
        {
            DateTime lastDayOfWeek = dateTime;

            double weekNumber = GetWeekNumber(lastDayOfWeek);

            if (weekNumber % 1 != 0)
            {
                do
                {
                    lastDayOfWeek = lastDayOfWeek.AddDays(1);
                    weekNumber = GetWeekNumber(lastDayOfWeek);
                } while (weekNumber % 1 != 0);
            }

            return lastDayOfWeek;
        }

        /// <summary>
        /// Obtém o último dia da semana a partir da data atual
        /// </summary>
        /// <returns></returns>
        public static DateTime GetLastDayOfWeekByDateTimeNow()
        {
            return GetLastDayOfWeekByDateTime(DateTime.Now);
        }

        /// <summary>
        /// Retorna a data passada como a mesma, porém às 23:59:59.999
        /// </summary>
        /// <param name="dateTime">Data</param>
        /// <returns></returns>
        public static DateTime GetDateTimeEnd(DateTime dateTime)
        {
            return dateTime.Date
                        .AddHours(23)
                        .AddMinutes(59)
                        .AddSeconds(59)
                        .AddMilliseconds(999)
                        .AddMicroseconds(999);
        }

        /// <summary>
        /// Verifica se duas datas estão no mesmo dia, mês e ano
        /// </summary>
        /// <param name="date1">Data 1</param>
        /// <param name="date2">Data 2</param>
        /// <returns>True caso seja a mesma data</returns>
        public static bool CheckSameIfIsSameDate(DateTime date1, DateTime date2)
        {
            return date1.Day == date2.Day && date1.Month == date2.Month && date1.Year == date2.Year;
        }
    }
}
