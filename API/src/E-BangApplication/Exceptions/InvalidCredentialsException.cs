﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangApplication.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException(string? message) : base(message)
        {
        }
    }
}
