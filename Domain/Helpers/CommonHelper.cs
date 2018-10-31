using System;

namespace Armin.Suitsupply.Domain.Helpers
{
    public static class CommonHelper
    {
        public static string GetRandomString(int len, bool upperChars, bool numbers)
        {
            string source = "abcdefghijklmnopqrstuvwxyz";
            if (upperChars)
                source += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (numbers)
                source += "0123456789";

            Random random = new Random();
            string randomString = "";
            for (int i = 0; i < len; i++)
                randomString += source[random.Next(source.Length)];

            return randomString;
        }
    }
}