using SterlingSwitch.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Extensions
{
    public class CustomerAccountEquality : IEqualityComparer<CustomerAccount>
    {
        public bool Equals(CustomerAccount x, CustomerAccount y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null | y == null)
                return false;
            else if (x.nuban == y.nuban)
                return true;
            else
                return false;
        }

        public int GetHashCode(CustomerAccount obj)
        {
            int hCode = obj.nuban.GetHashCode() ;
            return hCode.GetHashCode();
        }
    }
}
