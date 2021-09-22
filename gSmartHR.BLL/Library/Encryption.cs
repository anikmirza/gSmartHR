using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using gSmartHR.DAL.Models.Obj;

namespace gSmartHR.BLL.Library
{
    public static class Encryption
    {
        public static string Encrypt(string text, string keyString = null)
        {
            keyString = null == keyString ? Common.EncryptionKey : keyString;
            var key = System.Text.Encoding.UTF8.GetBytes(keyString);
            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }
                        var iv = aesAlg.IV;
                        var decryptedContent = msEncrypt.ToArray();
                        var result = new byte[iv.Length + decryptedContent.Length];
                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);
                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string keyString = null)
        {
            keyString = null == keyString ? Common.EncryptionKey : keyString;
            var fullCipher = Convert.FromBase64String(cipherText);
            var iv = new byte[16];
            var cipher = new byte[16];
            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = System.Text.Encoding.UTF8.GetBytes(keyString);
            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                    return result;
                }
            }
        }

		public static HashSalt EncryptPassword(string Password)
		{
			Password += Common.RandomPepper();
	        // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
	        byte[] Salt = new byte[128 / 8];
	        using (var RngCsp = new RNGCryptoServiceProvider())
	        {
	            RngCsp.GetNonZeroBytes(Salt);
	        }
	        // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
	        string Hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
	            password: Password,
	            salt: Salt,
	            prf: KeyDerivationPrf.HMACSHA256,
	            iterationCount: 100000,
	            numBytesRequested: 256 / 8)
	        );
		    return new HashSalt { Hash = Hashed , Salt = Salt };
		}
		    
		public static bool VerifyPassword(string EnteredPassword, byte[] Salt, string StoredPassword)
		{
			foreach(string Pepper in Common.Pepper)
	        {
		        string Hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
		            password: EnteredPassword + Pepper,
		            salt: Salt,
		            prf: KeyDerivationPrf.HMACSHA256,
		            iterationCount: 100000,
		            numBytesRequested: 256 / 8)
		        );
		        if (Hashed == StoredPassword) return true;
		    }
		    return false;
		}
    }
}
