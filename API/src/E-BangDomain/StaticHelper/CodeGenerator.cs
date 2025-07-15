using System.Security.Cryptography;

namespace E_BangDomain.StaticHelper
{
    public static class CodeGenerator
    {
        public static string GenerateRandomNumberCode()
        {
            var digits = Enumerable.Repeat(RandomNumberGenerator.GetInt32(0, 10), 6).Select(x => x.ToString());

            return string.Concat(digits);
        }
    }
}
