using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Backend.Common
{
    public static class Common
    {
        private static readonly Random Random = new Random();
        const string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
        const string UPPER_CAES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string NUMBERS = "123456789";
        const string SPECIALS = @"!@£$%^&*()#€";


        public static string GeneratePassword(bool useLowercase, bool useUppercase, bool useNumbers, bool useSpecial,
            int passwordSize)
        {
            char[] _password = new char[passwordSize];
            string charSet = ""; // Initialise to blank
            Random _random = new Random();
            int counter;

            // Build up the character set to choose from
            if (useLowercase) charSet += LOWER_CASE;

            if (useUppercase) charSet += UPPER_CAES;

            if (useNumbers) charSet += NUMBERS;

            if (useSpecial) charSet += SPECIALS;

            for (counter = 0; counter < passwordSize; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }

            return string.Join(null, _password);
        }


        public static string StringChecker(string text)
        {
            string newstr = null;
            var regexItem = new Regex("[^a-zA-Z0-9_.]+");
            if (regexItem.IsMatch(text[text.Length - 1].ToString()))
            {
                newstr = text.Remove(text.Length - 1);
            }
            string replacestr = Regex.Replace(newstr, "[^a-zA-Z0-9_]+", "-");
            return replacestr;
        }


        public static int RandomNumber(int min, int max)
        {
            return Random.Next(min, max);
        }

        // Generates a random string with a given size.    
        public static string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)Random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }


        public static string RandomPassword()
        {
            var passwordBuilder = new StringBuilder();

            // 4-Letters lower case   
            passwordBuilder.Append(RandomString(4, true));

            // 4-Digits between 1000 and 9999  
            passwordBuilder.Append(RandomNumber(1000, 9999));

            // 2-Letters upper case  
            passwordBuilder.Append(RandomString(2));
            return passwordBuilder.ToString();
        }

        public static bool IsDateTime(DateTime dateTime)
        {
            DateTime tempDate;
            return DateTime.TryParse(dateTime.ToString(), out tempDate);
        }

    }
}
