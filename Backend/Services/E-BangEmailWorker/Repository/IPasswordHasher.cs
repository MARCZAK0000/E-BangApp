using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangEmailWorker.Repository
{
    public interface IPasswordHasher
    {
        string GeneratePasswordHash(string password, string salt);
        bool VerifyPassword(string password, string hashedPassword, string salt);
    }
}
