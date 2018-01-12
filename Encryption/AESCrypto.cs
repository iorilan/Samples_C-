using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Imps.Services.IDCService.Utility
{
    public static class AESCrypto
    {
		public static byte[] Decrypt(byte[] encryptedBytes, byte[] key)
        {
			MemoryStream mStream = new MemoryStream( encryptedBytes );

			RijndaelManaged aes = new RijndaelManaged( );
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
			aes.Key = key;

			CryptoStream cryptoStream = new CryptoStream( mStream, aes.CreateDecryptor( ), CryptoStreamMode.Read );
			try {

				byte[] tmp = new byte[ encryptedBytes.Length + 32 ];
				int len = cryptoStream.Read( tmp, 0, encryptedBytes.Length + 32 );
				byte[] ret = new byte[ len ];
				Array.Copy( tmp, 0, ret, 0, len );
				return ret;
			}
			finally {
				cryptoStream.Close( );
				mStream.Close( );
				aes.Clear( );
			}
		}

        public static byte[] Encrypt(byte[] plainBytes, byte[] key)
        {
            MemoryStream mStream = new MemoryStream();

            RijndaelManaged aes = new RijndaelManaged();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            aes.Key = key;

            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            try
            {
                cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                cryptoStream.FlushFinalBlock();
                return mStream.ToArray();
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }
    }
}
