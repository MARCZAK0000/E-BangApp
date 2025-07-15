using System.Security.Cryptography;

namespace E_BangDomain.StaticHelper
{
    public static class CodeGenerator
    {
        private readonly static string _chars = "abcdefghijklmnoprstuvwxyz123456789";
        public static string GenerateRandomNumberCode()
        {
            var digits = Enumerable.Repeat(RandomNumberGenerator.GetInt32(0, 10), 6).Select(x => x.ToString());

            return string.Concat(digits);
        
        public static string GenerateRandomStringCode()
        {
            char[] buffer = new char[10];
            for (int i = 0; i < 10; i++)
            {
                buffer[i] = _chars[Random.Shared.Next(_chars.Length)];
            }
            return new string(buffer);
        }
    }
}
