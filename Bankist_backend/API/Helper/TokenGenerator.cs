using System.Security.Cryptography;
using System.Text;

namespace API.Helper
{
    public class TokenGenerator
    {
        public static string Generate(int size)
        {
            // Characters except I, l, O, 1, and 0 to decrease confusion when hand typing tokens
            var charSet = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var chars = charSet.ToCharArray();
            var data = new byte[1];
#pragma warning disable SYSLIB0023 // Type or member is obsolete
            var crypto = new RNGCryptoServiceProvider();
#pragma warning restore SYSLIB0023 // Type or member is obsolete
            crypto.GetNonZeroBytes(data);
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            var result = new StringBuilder(size);
            foreach (var b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
        public static string GenerateRandomPhoneNumber()
        {
            Random random = new Random();

            int numberOfDigits = random.Next(9, 11);
            string phoneNumber = "0";
            for (int i = 1; i < numberOfDigits; i++)
            {
                phoneNumber += random.Next(0, 10);
            }

            return phoneNumber;
        }
        public static string GeneratePassword()
        {
            const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
            const string digitChars = "0123456789";
            const string specialChars = "!@#$%^&*()-_+=<>?";

            int requiredLength = 8;
            int requiredUppercase = 1;
            int requiredDigit = 1;
            int requiredSpecialChar = 1;

            int length = requiredLength;
            int uppercaseCount = requiredUppercase;
            int digitCount = requiredDigit;
            int specialCharCount = requiredSpecialChar;

            StringBuilder password = new StringBuilder();

            // Characters except I, l, O, 1, and 0 to decrease confusion when hand typing tokens
            string charSet = $"{uppercaseChars}{lowercaseChars}{digitChars}{specialChars}".Replace("I", "").Replace("l", "").Replace("O", "").Replace("1", "").Replace("0", "");

            char[] chars = charSet.ToCharArray();
            byte[] data = new byte[1];

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[length];
                crypto.GetNonZeroBytes(data);

                foreach (byte b in data)
                {
                    password.Append(chars[b % (chars.Length)]);
                }
            }

            if (uppercaseCount > 0)
            {
                int randomUppercaseIndex = new Random().Next(password.Length);
                password[randomUppercaseIndex] = uppercaseChars[new Random().Next(uppercaseChars.Length)];
            }

            if (digitCount > 0)
            {
                int randomDigitIndex = new Random().Next(password.Length);
                password[randomDigitIndex] = digitChars[new Random().Next(digitChars.Length)];
            }

            if (specialCharCount > 0)
            {
                int randomSpecialCharIndex = new Random().Next(password.Length);
                password[randomSpecialCharIndex] = specialChars[new Random().Next(specialChars.Length)];
            }

            return password.ToString();
        }
        public static string GenerateRandomEmail()
        {
            string[] domains = { "gmail.com", "yahoo.com", "outlook.com", "hotmail.com" };

            Random random = new Random();
            string username = GenerateName(8);
            string domain = domains[random.Next(domains.Length)];
            return $"{username}@{domain}";
        }

        public static DateTime GenerateRandomBirthDate()
        {
            Random random = new Random();

            int years = random.Next(18, 81);

            int day = random.Next(1, 29);

            int month = random.Next(1, 13);

            int currentYear = DateTime.Now.Year;
            int birthYear = currentYear - years;

            DateTime birthDate = new DateTime(birthYear, month, day);

            return birthDate;
        }
        public static string GenerateName(int size)
        {
            // Characters except I, l, O, 1, and 0 to decrease confusion when hand typing tokens
            var charSet = "ABCDEFGHJKLMNPQRSTUVWXYZ".ToLower();
            var chars = charSet.ToCharArray();
            var data = new byte[1];
#pragma warning disable SYSLIB0023 // Type or member is obsolete
            var crypto = new RNGCryptoServiceProvider();
#pragma warning restore SYSLIB0023 // Type or member is obsolete
            crypto.GetNonZeroBytes(data);
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            var result = new StringBuilder(size);
            foreach (var b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            var s = result.ToString();
            return "S" + result;
        }

        public static string GeneratePurpose()
        {
            string[] purposes = { "Purchase", "Bills", "Bill Payment", "Money Transfer", "Donation", "Other" };
            var random = new Random();
            return purposes[random.Next(purposes.Length)];
        }
        public static string GenerateRandomKey()
        {
            Random random = new Random();

            int[] randomNumbers = new int[6];
            for (int i = 0; i < randomNumbers.Length; i++)
            {
                randomNumbers[i] = random.Next(0, 10);
            }

            char[] charArray = new char[randomNumbers.Length];
            for (int i = 0; i < randomNumbers.Length; i++)
            {
                charArray[i] = Convert.ToChar(randomNumbers[i] + '0');
            }

            string randomKey = new string(charArray);

            return randomKey;
        }
    }

}
