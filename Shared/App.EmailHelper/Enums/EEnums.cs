using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.EmailHelper.Enums
{
    public enum HeaderEnum
    {
        Defualt = 0,
        Custom = 1  
    }
    public enum EEnumEmailBodyBuilderType
    {
        Registration = 0,
        ConfirmEmail = 1,
        ChangePassword = 2,
        TwoWayToken = 3,
    }
    public enum FooterEnum
    {
        Defualt = 0,
        Custom = 1
    }
}
