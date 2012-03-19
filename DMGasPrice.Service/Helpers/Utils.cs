using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace DMGasPrice.Service.Helpers
{
    public static class Utils
    {

        private const string DATE_FORMAT = MONTHS + "\\s+\\d{1,2},\\s+\\d{4}";
        private const string MONTHS = "(Jan\\w+|Feb\\w+|Mar\\w+|Apr\\w+|May\\w+|Jun\\w+|Jul\\w+|Aug\\w+|Sep\\w+|Oct\\w+|Nov\\w+|Dec\\w+)";
        public static DateTime ExtractDate(string text)
        {
            Match match = Regex.Match(text, DATE_FORMAT, RegexOptions.IgnoreCase);
            if (match.Success && match.Groups.Count > 0)
            {
                DateTime result;
                if (DateTime.TryParse(match.Groups[0].Value, out result))
                {
                    return result;
                }
            }

            return DateTime.Today.AddDays(1);
        }

        public static int ExtractCityKey(string text)
        {
            Match match = Regex.Match(text, ".+\\?city=(\\d{2,3})", RegexOptions.IgnoreCase);
            if (match.Success && match.Groups.Count >= 2)
            {
                return int.Parse(match.Groups[1].Value);
            }

            return default(int);
        }

        public static string ExtractCityName(string text)
        {
            Match match = Regex.Match(text, "(.+) gas price", RegexOptions.IgnoreCase);
            if (match.Success && match.Groups.Count >= 2)
            {
                return match.Groups[1].Value;
            }

            return default(string);
        }

        public static double ExtractPrice(string text)
        {
            Match match = Regex.Match(text, "(\\d+(\\.\\d+)*)");
            if (match.Success && match.Groups.Count >= 2)
            {
                return double.Parse(match.Groups[1].Value);
            }

            return default(double);
        }
    }
}