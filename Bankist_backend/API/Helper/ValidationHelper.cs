using System.Text.RegularExpressions;
using System.Text;

namespace API.Helper
{
    public class ValidationHelper
    {
        public static string CheckPasswordStrength(string password)
        {
            StringBuilder sb = new StringBuilder();
            if (password.Length < 8)
            {
                sb.Append("Password must have at least 8 characters");
            }

            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]")))
            {
                sb.Append("Password must contain at least one uppercase letter");
            }

            if (!Regex.IsMatch(password, "[0-9]"))
            {
                sb.Append("Password must contain at least one number");
            }

            if (!(Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,-,+,/,|,~,=]")))
            {
                sb.Append("Password must contain at least one special character");
            }

            return sb.ToString();
        }

        public static string CheckPhoneNumber(string phoneNumber)
        {
            if (int.TryParse(phoneNumber, out _))
            {
                if (!(phoneNumber.Length == 9 || phoneNumber.Length == 10))
                {
                    return "The phone number must have 9 or 10 digits";
                }
                return string.Empty;
            }
            else
            {
                return "The entered text is not a valid phone number";
            }

        }
    }
}
