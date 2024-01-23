using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AbidiCompanySenario.Utilities
{
    public static class Utilites
    {
        public static string SplitString(string input, int length)
        {
            if (input.Length <= length)
                return input;

            string truncatedString = input.Substring(0, length) + "...";
            return truncatedString;
        }

  

        public static bool IsValidMobileNumber(this string input)
        {
            const string pattern = @"^09[0|1|2|3][0-9]{8}$";
            Regex reg = new Regex(pattern);
            return reg.IsMatch(input);
        }
        public static bool NationalCodeIsValid(this string input)
        {
            if (!Regex.IsMatch(input, @"^\d{10}$"))
                return false;

            var check = Convert.ToInt32(input.Substring(9, 1));
            var sum = Enumerable.Range(0, 9)
                    .Select(x => Convert.ToInt32(input.Substring(x, 1)) * (10 - x))
                    .Sum() % 11;
            return (sum < 2 && check == sum) || (sum >= 2 && check + sum == 11);
        }
    }
}
