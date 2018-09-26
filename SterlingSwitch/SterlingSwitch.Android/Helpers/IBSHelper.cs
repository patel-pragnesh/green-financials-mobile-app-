using SterlingSwitch.Helper;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(SterlingSwitch.Droid.Helpers.IBSHelper))]
namespace SterlingSwitch.Droid.Helpers
{
    public class IBSHelper: ICryptoService
    {
        private StringBuilder rqt = new StringBuilder();

        public string GenerateRndNumber(int cnt)
        {
            string[] key2 = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            Random rand1 = new Random();
            string txt = "";
            for (int j = 0; j < cnt; j++)
                txt += key2[rand1.Next(0, 9)];
            return txt;

        }
        string shdkvl = "000000010000001000000101000001010000011100001011000011010001000100010010000100010000110100001011000001110000001000000100000010000000000100001100000000110000010100000111000010110000110100011100";
        string shdvectorKey = "0000000100000010000000110000010100000111000010110000110100000100";

        public string Encrypt(string val)
        {

            string sharedkeyval = shdkvl; // ConfigurationManager.AppSettings["sharedkeyval"];
            string sharedvectorval = shdvectorKey; //ConfigurationManager.AppSettings["sharedvectorval"];
            sharedkeyval = BinaryToString(sharedkeyval);
            sharedvectorval = BinaryToString(sharedvectorval);
            byte[] sharedkey = System.Text.Encoding.GetEncoding("utf-8").GetBytes(sharedkeyval);
            byte[] sharedvector = System.Text.Encoding.GetEncoding("utf-8").GetBytes(sharedvectorval);
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            byte[] toEncrypt = Encoding.UTF8.GetBytes(val);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, tdes.CreateEncryptor(sharedkey, sharedvector), CryptoStreamMode.Write);
            cs.Write(toEncrypt, 0, toEncrypt.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
        public string Decrypt(string val)
        {
            string sharedkeyval = shdkvl; // ConfigurationManager.AppSettings["sharedkeyval"];
            string sharedvectorval = shdvectorKey;//ConfigurationManager.AppSettings["sharedvectorval"];
            sharedkeyval = BinaryToString(sharedkeyval);
            sharedvectorval = BinaryToString(sharedvectorval);

            byte[] sharedkey = System.Text.Encoding.GetEncoding("utf-8").GetBytes(sharedkeyval);
            byte[] sharedvector = System.Text.Encoding.GetEncoding("utf-8").GetBytes(sharedvectorval);
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            byte[] toDecrypt = Convert.FromBase64String(val);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, tdes.CreateDecryptor(sharedkey, sharedvector), CryptoStreamMode.Write);
            cs.Write(toDecrypt, 0, toDecrypt.Length);
            cs.FlushFinalBlock();
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        private string BinaryToString(string binary)
        {
            if (string.IsNullOrEmpty(binary))
                throw new ArgumentNullException("binary");

            if ((binary.Length % 8) != 0)
                throw new ArgumentException("Binary string invalid (must divide by 8)", "binary");

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < binary.Length; i += 8)
            {
                string section = binary.Substring(i, 8);
                int ascii = 0;
                try
                {
                    ascii = Convert.ToInt32(section, 2);
                }
                catch
                {
                    throw new ArgumentException("Binary string contains invalid section: " + section, "binary");
                }
                builder.Append((char)ascii);
            }
            return builder.ToString();
        }
    }
}