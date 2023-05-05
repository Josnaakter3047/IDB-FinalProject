using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CryptographyService
    {
        public const String strPermutation = "manuSharGorUAkshaThe";
        public const Int32 bytePermutation1 = 0x32;
        public const Int32 bytePermutation2 = 0x49;
        public const Int32 bytePermutation3 = 0x25;
        public const Int32 bytePermutation4 = 0x42;

        public static string Encrypt(string strData)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(strData)));
        }

        public static byte[] Encrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes = new(strPermutation, new byte[] { bytePermutation1, bytePermutation2, bytePermutation3, bytePermutation4 });

            MemoryStream memstream = new();

            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new(memstream,
                aes.CreateEncryptor(), CryptoStreamMode.Write);

            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();

            return memstream.ToArray();
        }

        public static string Decrypt(string strData)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(strData)));
        }

        public static byte[] Decrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes = new(strPermutation,
            new byte[] { bytePermutation1, bytePermutation2, bytePermutation3, bytePermutation4 });

            MemoryStream memstream = new();

            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new(memstream,
                aes.CreateDecryptor(), CryptoStreamMode.Write);

            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();

            return memstream.ToArray();
        }
    }
}
