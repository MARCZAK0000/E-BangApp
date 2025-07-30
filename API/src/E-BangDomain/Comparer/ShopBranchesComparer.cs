using E_BangDomain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangDomain.Comparer
{
    public class ShopBranchesComparer : IEqualityComparer<ShopBranchesInformations>
    {
        public bool Equals(ShopBranchesInformations? x, ShopBranchesInformations? y)
        {
            return x is not null && 
                y is not null &&
                x.ShopStreetName == y.ShopStreetName &&
                x.ShopPostalCode == y.ShopPostalCode && 
                x.ShopCity == y.ShopCity;
        }

        public int GetHashCode([DisallowNull] ShopBranchesInformations obj)
        {
           return HashCode
                .Combine(obj.ShopStreetName, 
                obj.ShopCity, 
                obj.ShopPostalCode);
        }
    }
}
