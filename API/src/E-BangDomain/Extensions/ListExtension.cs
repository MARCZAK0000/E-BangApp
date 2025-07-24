using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangDomain.Extensions
{
    public static class ListExtension
    {
        public static void Empty<T>(this List<T> values) => values ??= [];
    }
}
