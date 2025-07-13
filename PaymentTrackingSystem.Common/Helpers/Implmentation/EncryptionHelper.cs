using Microsoft.Extensions.Configuration;
using PaymentTrackingSystem.Common.Helpers.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Common.Helpers.Implmentation
{
    public class EncryptionHelper : IEncryptionHelper
    {
        public IConfiguration Configuration { get; }
        private string PTSEncryptionKeyValue { get; set; } = string.Empty;
        private string PTSEncryptionIVValue { get; set; } = string.Empty;
        public EncryptionHelper(IConfiguration _Configuration)
        {
            Configuration = _Configuration;
        }
        public string Encrypt(string plainText)
        {
            PTSEncryptionKeyValue = GetEnvriptedKeyValueFromAppSettings();
            PTSEncryptionIVValue = GetEnvripteIVValueFromAppSettings();

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(PTSEncryptionKeyValue);
            aes.IV = Encoding.UTF8.GetBytes(PTSEncryptionIVValue);

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string cipherText)
        {
            PTSEncryptionKeyValue = GetEnvriptedKeyValueFromAppSettings();
            PTSEncryptionIVValue = GetEnvripteIVValueFromAppSettings();

            var buffer = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(PTSEncryptionKeyValue);
            aes.IV = Encoding.UTF8.GetBytes(PTSEncryptionIVValue);

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream(buffer);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            {
                return sr.ReadToEnd();
            }
        }

        private string GetEnvriptedKeyValueFromAppSettings()
        {
            return Configuration.GetSection("PTSEncryptionKeySettings").GetChildren().First(x => x.Key == "EncryptionHelperKey").Value.ToString();
        }
        private string GetEnvripteIVValueFromAppSettings()
        {
            return Configuration.GetSection("PTSEncryptionKeySettings").GetChildren().First(x => x.Key == "EncryptionHelperIVKey").Value.ToString();
        }

    }

  
}