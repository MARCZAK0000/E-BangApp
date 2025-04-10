
using System.Text;
using E_BangEmailWorker.Helper;

namespace E_BangEmailWorker.Repository
{
    public class PasswordHasher : IPasswordHasher
    {
        public string GeneratePasswordHash(string password, string salt)
        {
            var saltBuffer = Encoding.UTF8.GetBytes(salt);
            var saltedPassword = PasswordHelper.SaltPassword(saltBuffer, password);
            var hashedPassword = PasswordHelper.HashPassword(saltedPassword);
            return hashedPassword;
        }
        public bool VerifyPassword(string password, string hashedPassword, string salt)
        {
            var saltBuffer = Encoding.UTF8.GetBytes(salt);
            var saltedPassword = PasswordHelper.SaltPassword(saltBuffer, password);
            var currentHashPassword = PasswordHelper.HashPassword(saltedPassword);
            return hashedPassword == currentHashPassword;
        }
    }
}
