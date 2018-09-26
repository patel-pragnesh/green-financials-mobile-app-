using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Models
{
    public class CurrencyType
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public string symbol_native { get; set; }
        public int decimal_digits { get; set; }
        public float rounding { get; set; }
        public string code { get; set; }
        public string name_plural { get; set; }
    }

    public enum CurrencyReturnType
    {
        CurrencySymbol, CurrencyName
    }
}
