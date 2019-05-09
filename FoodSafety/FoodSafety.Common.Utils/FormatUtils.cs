using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FoodSafety.Common.Utils
{
    /// <summary>
    /// Format utils.
    /// </summary>
    public static class FormatUtils
    {
        static CultureInfo cultureAmount = new CultureInfo("es-CL");

        /// <summary>
        /// Formats the amount.
        /// </summary>
        /// <returns>The amount.</returns>
        /// <param name="amount">Amount.</param>
        public static string FormatAmount(int amount)
        {
            var newAmount = amount.ToString("C", cultureAmount);
            return newAmount;
        }

        /// <summary>
        /// Quits the accents.
        /// </summary>
        /// <returns>The accents.</returns>
        /// <param name="inputString">Input string.</param>
        public static string RemoveAccents(string inputString)
        {
            Regex a = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex e = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex i = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex o = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex u = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            Regex n = new Regex("[ñ|Ñ]", RegexOptions.Compiled);
            inputString = a.Replace(inputString, "a");
            inputString = e.Replace(inputString, "e");
            inputString = i.Replace(inputString, "i");
            inputString = o.Replace(inputString, "o");
            inputString = u.Replace(inputString, "u");
            inputString = n.Replace(inputString, "n");
            return inputString;
        }

        /// <summary>
        /// Emails the is valid.
        /// </summary>
        /// <returns><c>true</c>, if is valid was emailed, <c>false</c> otherwise.</returns>
        /// <param name="email">Email.</param>
        public static bool EmailIsValid(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// Sets the max date.
        /// </summary>
        /// <returns>The max date.</returns>
        /// <param name="days">Days.</param>
        public static long SetMaxDate(int days)
        {
            DateTime _dt_now = DateTime.Now;
            DateTime _start = new DateTime(1970, 1, 1);
            TimeSpan ts = (_dt_now - _start);

            //Add Days to SetMax Days;
            int noOfDays = ts.Days + days;
            return (long)(TimeSpan.FromDays(noOfDays).TotalMilliseconds);
        }

        /// <summary>
        /// Sets the minimum date.
        /// </summary>
        /// <returns>The minimum date.</returns>
        public static long SetMinDate()
        {
            var javaMinDt = new DateTime(1970, 1, 1);
            var dt = DateTime.Now;
            if (dt.CompareTo(javaMinDt) < 0)
                throw new ArgumentException("la fecha debe ser >= a 1/1970");

            var longVal = dt.AddMonths(-24) - javaMinDt;
            return (long)longVal.TotalMilliseconds;
        }
    }
}
