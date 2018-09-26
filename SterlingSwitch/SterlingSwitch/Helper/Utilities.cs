using Newtonsoft.Json;
using SterlingSwitch.Models;
using SterlingSwitch.Pages.Dashboard;
using SterlingSwitch.PlatformSpecs;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace SterlingSwitch.Helper
{
    public class Utilities
    {
        public static string XmlSerializer<T>(T model)
        {



            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var xmlserialize = new XmlSerializer(typeof(T));
            string xml = string.Empty;

            using (var writer = new StringWriter())
            {
                using (var xmlwriter = XmlWriter.Create(writer))
                {
                    xmlserialize.Serialize(xmlwriter, model, ns);
                    xml = writer.ToString();
                }
            }

            return xml;
        }
        public static class ListExtensions
        {
            public static T[] ConvertToArray<T>(IList list)
            {
                return list.Cast<T>().ToArray();
            }

            public static object[] ConvertToArrayRuntime(IList list, Type elementType)
            {
                var convertMethod = typeof(ListExtensions).GetMethod("ConvertToArray", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(IList) }, null);
                var genericMethod = convertMethod.MakeGenericMethod(elementType);
                return (object[])genericMethod.Invoke(null, new object[] { list });
            }

        }

        public static string GenerateReferenceId()
        {
            Random r = new Random();
            r.Next(1000, 9999);
            string referenceId = "00055" + DateTime.Now.ToString("yymmddHHmmss") + r.Next().ToString();
            return referenceId;
        }

        public static string GetCurrency(string currencyCode, CurrencyReturnType returnType = CurrencyReturnType.CurrencySymbol)
        {
            if (string.IsNullOrEmpty(currencyCode)) return string.Empty;
            currencyCode = currencyCode.ToUpper();
            string symbol = currencyCode;
            try
            {

                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Dashboard)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("SterlingSwitch.Resources.Currencies.json");
                string text = "";
                using (var reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }

                text = text.Replace(Environment.NewLine, string.Empty);
                text = text.Replace(" ", string.Empty);

                var jsonObject = JsonConvert.DeserializeObject<CurrencyType[]>(text);

                var item = jsonObject.Where(i => i.code == currencyCode).Single();

                if (item != null)
                {
                    if (returnType == CurrencyReturnType.CurrencySymbol)
                    {
                        symbol = item.symbol_native;

                        if (string.IsNullOrWhiteSpace(symbol))
                            return currencyCode;
                    }
                    else
                        symbol = item.name;

                }
            }
            catch (Exception)
            {
                ;
            }

            return symbol;
        }


        internal static string[] SplitBeneficiaryDetailsByTab(string displayText)
        {
            var arrayItem = displayText.Trim().Split('|');
            return arrayItem;
        }

        public static void ShowToast(string message)
        {
            DependencyService.Get<IToastMessage>().ShowToast(message);
        }

        public static string ConvertToFullMSDN(string mobileNumber)
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

        public static string EncryptData(byte[] dataToEncrypt, string exponent, string modulus)
        {
            byte[] cypherText;
            using (var rsa = new RSACryptoServiceProvider(4096))
            {
                rsa.ImportParameters(new RSAParameters
                {
                    Exponent = Convert.FromBase64String(exponent),
                    Modulus = Convert.FromBase64String(modulus)

                });
                cypherText = rsa.Encrypt(dataToEncrypt, false);
            }
            return Convert.ToBase64String(cypherText);
        }

        public static async Task<string> GetUniqueKey()
        {
            return (await Microsoft.AppCenter.AppCenter.GetInstallIdAsync()).ToString();
        }
    }
}
