using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Helper
{
    public interface ICryptoService
    {
        string Encrypt(string valueToEncrypt);
        string Decrypt(string valueToDecrypt);
    }
}
