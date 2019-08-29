using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cig.YanFa.Account.Web.Core.Utils
{
    /// <summary>
    /// DES加密解密类 
    /// </summary>
    public class DESEncryptor
    {
        private const string DefaultDesKey = "auto@#$&";

        private static readonly byte[] DefaultIV = new byte[]
        {
            18,
            52,
            86,
            120,
            144,
            171,
            205,
            239,
            169,
            58,
            207,
            174,
            177,
            240,
            243,
            155
        };

        private static DESCryptoServiceProvider CreateDESProvider(string encryptKey)
        {
            int length = encryptKey.Length;
            if (length < 8)
            {
                for (int i = 0; i < 8 - length; i++)
                {
                    encryptKey += "@";
                }
            }
            DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
            dESCryptoServiceProvider.Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, dESCryptoServiceProvider.KeySize / 8));
            byte[] array = new byte[dESCryptoServiceProvider.BlockSize / 8];
            Array.Copy(DESEncryptor.DefaultIV, 0, array, 0, array.Length);
            dESCryptoServiceProvider.IV = array;
            return dESCryptoServiceProvider;
        }

        public static string Encrypt(string strText)
        {
            return DESEncryptor.Encrypt(strText, "auto@#$&");
        }

        public static string Encrypt(string strText, string encryptKey)
        {
            if (string.IsNullOrEmpty(strText))
            {
                return strText;
            }
            DESCryptoServiceProvider dESCryptoServiceProvider = DESEncryptor.CreateDESProvider(encryptKey);
            byte[] bytes = Encoding.UTF8.GetBytes(strText);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static string Decrypt(string encryptedText)
        {
            return DESEncryptor.Decrypt(encryptedText, "auto@#$&");
        }

        public static string Decrypt(string encryptedText, string decryptKey)
        {
            if (string.IsNullOrEmpty(encryptedText))
            {
                return encryptedText;
            }
            DESCryptoServiceProvider dESCryptoServiceProvider = DESEncryptor.CreateDESProvider(decryptKey);
            byte[] array = Convert.FromBase64String(encryptedText);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(array, 0, array.Length);
            cryptoStream.FlushFinalBlock();
            return new UTF8Encoding().GetString(memoryStream.ToArray());
        }

        public static void EncryptFile(string inputFilePath, string outFilePath)
        {
            DESEncryptor.EncryptFile(inputFilePath, outFilePath, "auto@#$&");
        }

        public static void EncryptFile(string inputFilePath, string outFilePath, string encryptKey)
        {
            FileStream fileStream = null;
            FileStream fileStream2 = null;
            CryptoStream cryptoStream = null;
            try
            {
                fileStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
                fileStream2 = new FileStream(outFilePath, FileMode.OpenOrCreate, FileAccess.Write);
                fileStream2.SetLength(0L);
                byte[] buffer = new byte[100];
                long num = 0L;
                long length = fileStream.Length;
                DES dES = DESEncryptor.CreateDESProvider(encryptKey);
                cryptoStream = new CryptoStream(fileStream2, dES.CreateEncryptor(), CryptoStreamMode.Write);
                while (num < length)
                {
                    int num2 = fileStream.Read(buffer, 0, 100);
                    cryptoStream.Write(buffer, 0, num2);
                    num += (long)num2;
                }
            }
            finally
            {
                if (cryptoStream != null)
                {
                    cryptoStream.Close();
                }
                if (fileStream2 != null)
                {
                    fileStream2.Close();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }

        public static void DecryptFile(string inputFilePath, string outFilePath)
        {
            DESEncryptor.DecryptFile(inputFilePath, outFilePath, "auto@#$&");
        }

        public static void DecryptFile(string inputFilePath, string outFilePath, string decryptKey)
        {
            FileStream fileStream = null;
            FileStream fileStream2 = null;
            CryptoStream cryptoStream = null;
            try
            {
                fileStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
                fileStream2 = new FileStream(outFilePath, FileMode.OpenOrCreate, FileAccess.Write);
                fileStream2.SetLength(0L);
                byte[] buffer = new byte[100];
                long num = 0L;
                long length = fileStream.Length;
                DES dES = DESEncryptor.CreateDESProvider(decryptKey);
                cryptoStream = new CryptoStream(fileStream2, dES.CreateDecryptor(), CryptoStreamMode.Write);
                while (num < length)
                {
                    int num2 = fileStream.Read(buffer, 0, 100);
                    cryptoStream.Write(buffer, 0, num2);
                    num += (long)num2;
                }
            }
            finally
            {
                if (cryptoStream != null)
                {
                    cryptoStream.Close();
                }
                if (fileStream2 != null)
                {
                    fileStream2.Close();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }

        public static string MD5Hash(string strText)
        {
            return DESEncryptor.MD5Hash(strText, Encoding.Default);
        }

        public static string MD5Hash(string strText, Encoding encoding)
        {
            MD5 mD = new MD5CryptoServiceProvider();
            byte[] bytes = encoding.GetBytes(strText);
            byte[] array = mD.ComputeHash(bytes);
            string text = null;
            for (int i = 0; i < array.Length; i++)
            {
                string text2 = array[i].ToString("x");
                if (text2.Length == 1)
                {
                    text2 = "0" + text2;
                }
                text += text2;
            }
            return text;
        }
    }
}
