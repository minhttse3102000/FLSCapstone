using System;
using System.Linq;

namespace BEAPICapstoneProjectFLS.RandomKey
{
    public class RandomPKKey
    {
        public static string NewRamDomPKKey()
        {
            Random random = new Random();
            int length = 30;
            const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string NewRamDomToken()
        {
            Random random = new Random();
            int length = 60;
            const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
