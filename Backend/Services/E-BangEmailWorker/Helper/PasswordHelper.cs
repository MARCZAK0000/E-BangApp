using System.Security.Cryptography;
using System.Text;
namespace E_BangEmailWorker.Helper
{
    public static class PasswordHelper
    {
        public static byte[] SaltPassword(byte[] salt, string password)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var passwordBuffer = new byte[salt.Length + passwordBytes.Length];

            Buffer.BlockCopy(passwordBytes, 0, passwordBuffer, 0, password.Length);
            Buffer.BlockCopy(salt, 0, passwordBuffer, passwordBytes.Length, salt.Length);

            return passwordBuffer;
        }

        public static string HashPassword(byte[] saltedPassword) =>
            Convert.ToBase64String(SHA256.HashData(saltedPassword));
    }
}
