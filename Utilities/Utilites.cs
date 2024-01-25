using AbidiCompanySenario.ViewModels.Personnels;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
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

        public static string GetHtmlContent(List<PersonnelViewModel> personnelViewModel)
        {
            var sb = new StringBuilder();

            sb.Append("<html><body><table class='table table-striped table-hover'>");
            sb.Append("<thead><tr>");
            sb.Append("<th>Personnel Code</th>");
            sb.Append("<th>First Name</th>");
            sb.Append("<th>Last Name</th>");
            sb.Append("<th>National Code</th>");
            sb.Append("<th>Created Date</th>");
            sb.Append("</tr></thead>");

            sb.Append("<tbody>");
            foreach (var personnel in personnelViewModel)
            {
                sb.Append("<tr>");
                sb.Append("<td>").Append(personnel.Personnel_Code).Append("</td>");
                sb.Append("<td>").Append(personnel.First_Name).Append("</td>");
                sb.Append("<td>").Append(personnel.Last_Name).Append("</td>");
                sb.Append("<td>").Append(personnel.National_Code).Append("</td>");
                sb.Append("<td>").Append(personnel.CreateDate).Append("</td>");
                sb.Append("</tr>");
            }
            sb.Append("</tbody>");
            sb.Append("</table></body></html>");
            return sb.ToString();
        }

        public static bool CheckFileType(string file)
        {
            string fileExtension = Path.GetExtension(file);
            if (fileExtension.Equals(".pdf", StringComparison.OrdinalIgnoreCase) ||
                fileExtension.Equals(".txt", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("File type is valid.");
                return true;
            }
            else
            {
                Console.WriteLine("Invalid file type. Accepted file types are PDF and TXT.");
                return false;
            }
        }
    }
}
