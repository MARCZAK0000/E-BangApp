using E_BangDomain.MaybePattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangApplication.Authentication
{
    public interface IUserContext
    {
        CurrentUser GetCurrentUser();
    }
}
