public enum EncryptionAlgorithm { DES, RC2, Rijndael, TripleDES, AES }

    public class CryptographyProvider
    {
        private const string Secretkey = "absjdhsajdwq"; //can be any string
        private const string VectorIv = "Cr$fe~L4mV+?}%|2"; //must be 16bytes
        private readonly byte[] _keyBytes;
        private readonly byte[] _ivBytes;
        private readonly SymmetricAlgorithm _symmetricAlgorithm;
        private readonly EncryptionAlgorithm _algorithm;

        public static CryptographyProvider AESEncryptor = new CryptographyProvider(EncryptionAlgorithm.AES);

        public CryptographyProvider(EncryptionAlgorithm algorithm)
        {
            _algorithm = algorithm;

            switch (_algorithm)
            {
                case EncryptionAlgorithm.DES: _symmetricAlgorithm = new DESCryptoServiceProvider(); break;
                case EncryptionAlgorithm.RC2: _symmetricAlgorithm = new RC2CryptoServiceProvider(); break;
                case EncryptionAlgorithm.Rijndael: _symmetricAlgorithm = new RijndaelManaged(); break;
                case EncryptionAlgorithm.TripleDES: _symmetricAlgorithm = new TripleDESCryptoServiceProvider(); break;
                case EncryptionAlgorithm.AES: _symmetricAlgorithm = new AesCryptoServiceProvider(); break;
            }

            _symmetricAlgorithm.Mode = CipherMode.CBC;

            _keyBytes = GetLegalKey(Secretkey);
            _ivBytes = Encoding.ASCII.GetBytes(VectorIv);
        }

        private byte[] GetLegalKey(string key)
        {
            string sTemp;
            
            if (_symmetricAlgorithm.LegalKeySizes.Length > 0)
            {
                var moreSize = _symmetricAlgorithm.LegalKeySizes[0].MinSize;

                while (key.Length * 8 > moreSize) { moreSize += _symmetricAlgorithm.LegalKeySizes[0].SkipSize; } sTemp = key.PadRight(moreSize / 8, ' ');
            }
            else sTemp = key;

            return Encoding.ASCII.GetBytes(sTemp);
        }


        public string Encrypt(string encryptText)
        {
            byte[] encryptTextBytes = Encoding.UTF8.GetBytes(encryptText);

            var encryptor = _symmetricAlgorithm.CreateEncryptor(_keyBytes, _ivBytes);

            var memoryStream = new MemoryStream();

            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(encryptTextBytes, 0, encryptTextBytes.Length);

            cryptoStream.FlushFinalBlock();

            byte[] cipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();

            cryptoStream.Close();

            var cipherText = Convert.ToBase64String(cipherTextBytes);

            return cipherText;
        }

        public string Decrypt(string decryptText)
        {
            var cipherTextBytes = Convert.FromBase64String(decryptText);

            var decryptor = _symmetricAlgorithm.CreateDecryptor(_keyBytes, _ivBytes);

            var memoryStream = new MemoryStream(cipherTextBytes);

            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            var plainTextBytes = new byte[cipherTextBytes.Length];

            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

            memoryStream.Close();

            cryptoStream.Close();

            var plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

            return plainText;
        }
    }