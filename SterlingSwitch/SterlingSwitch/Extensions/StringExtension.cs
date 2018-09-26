using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Extensions
{
    public static class StringExtension
    {
        public static string JsonCleanUp(this string jsonString)
        {
            jsonString = jsonString.Trim('"');
            jsonString = jsonString.Replace(@"\", "");
            jsonString = jsonString.Replace("\"{", "{");
            jsonString = jsonString.Replace("}\"", "}");
            jsonString = jsonString.Replace("\"\"[", "[");
            jsonString = jsonString.Replace("]\"\"", "]");
            return jsonString;
        }

        public static string ToMSDN(this string mobileNumber)
        {
            string number = "";
            if (mobileNumber.StartsWith("234"))
            {
                if (mobileNumber.Length == 13)
                {
                    number = mobileNumber;
                }
            }
            else if (mobileNumber.StartsWith("+234"))
            {
                if (mobileNumber.Length == 14)
                {
                    mobileNumber = mobileNumber.Substring(1, 13);
                    number = mobileNumber;
                }
                else
                    number = "";
            }
            else
            {
                number = "234";
                if (mobileNumber.Length == 11)
                    number += mobileNumber.Substring(1, 10);
                else if (number.Length == 10)
                    number += mobileNumber;
                else
                    number = "";
            }
            return number;
        }
    }
}
