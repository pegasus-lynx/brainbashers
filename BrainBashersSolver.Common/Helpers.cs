using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainBashersSolver.Common
{
    public static class Helpers
    {
        public static string DateToString(DateTime date)
        {
            return $"{date.Month:00}{date.Day:00}";
        }

        public static bool TryParseDateString(string dateString, out DateTime date)
        {
            date = DateTime.Today;

            if (string.Equals(dateString, "today", StringComparison.OrdinalIgnoreCase))
                return true;

            if (dateString.Length != 4)
                return false;

            string month = dateString[..2];
            string day = dateString[2..];

            if (int.TryParse(month, out int monthInt) && int.TryParse(day, out int dayInt))
            {
                date = new DateTime(2000, monthInt, dayInt);
                return true;
            }

            return false;
        }
    }
}