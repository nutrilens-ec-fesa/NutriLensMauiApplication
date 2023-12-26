using System.Security.Cryptography;
using System.Text;

namespace CryptographyLibrary
{
    /// <summary>
    /// Enumerator de tipos de algoritmos de criptografia
    /// </summary>
    public enum EnumCryptAlgorithm
    {
        /// <summary>
        /// MD5
        /// </summary>
        caMD5,
        /// <summary>
        /// SHA1
        /// </summary>
        caSHA1,
        /// <summary>
        /// SHA256
        /// </summary>
        caSHA256,
        /// <summary>
        /// SHA384
        /// </summary>
        caSHA384,
        /// <summary>
        /// SHA512
        /// </summary>
        caSHA512
    };

    public enum EnumCryptKeys
    {
        UserInfoKey
    }


    /// <summary>
    /// Classe auxiliar de criptografia
    /// </summary>
    public static class CryptographyManager
    {
        private static string GetKeyByEnumCrypt(EnumCryptKeys enumCryptKey)
        {
            switch (enumCryptKey)
            {
                case EnumCryptKeys.UserInfoKey:
                    return "8728B520-A452-4590-A601-BC0FDE7D6F24";
                default:
                    throw new Exception("Invalid Key");
            }
        }

        const string ENCRYPTKEY = "E!09#x*%&aTe$lks";

        /// <summary>
        /// Obtém o algoritmo de criptografia pelo enumerador
        /// </summary>
        /// <param name="value">Enumerador do tipo de criptografia</param>
        /// <returns>Algoritmo desejado</returns>
        private static HashAlgorithm GetAlgorithm(EnumCryptAlgorithm value)
        {
            HashAlgorithm result = null;
            switch (value)
            {

                case EnumCryptAlgorithm.caMD5:
                    result = MD5.Create();
                    break;

                case EnumCryptAlgorithm.caSHA1:
                    result = SHA1.Create();
                    break;

                case EnumCryptAlgorithm.caSHA256:
                    result = SHA256.Create();
                    break;

                case EnumCryptAlgorithm.caSHA384:
                    result = SHA384.Create();
                    break;

                case EnumCryptAlgorithm.caSHA512:
                    result = SHA512.Create();
                    break;
            }
            return result;
        }

        /// <summary>
        /// Criptografa uma string utilizando o algoritmo desejado
        /// </summary>
        /// <param name="value">valor a ser criptografado</param>
        /// <param name="algorithm">enumerador do tipo de criptografia desejado</param>
        /// <returns>texto criptografado</returns>
        public static string Crypt(string value, EnumCryptAlgorithm algorithm)
        {
            HashAlgorithm alg = GetAlgorithm(algorithm);

            byte[] encryptedValue = alg.ComputeHash(Encoding.UTF8.GetBytes(value));

            var sb = new StringBuilder();

            foreach (byte caracter in encryptedValue)
                sb.Append(caracter.ToString("X2"));

            return sb.ToString();
        }

        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static string HashPassword(string password, byte[] salt)
        {
            // Combine the password and salt, and compute the hash
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combinedBytes = new byte[passwordBytes.Length + salt.Length];
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, combinedBytes, passwordBytes.Length, salt.Length);

            byte[] hashedBytes = SHA512.HashData(combinedBytes);

            // Convert the hashed bytes to a hexadecimal string
            StringBuilder hashedPassword = new StringBuilder();
            foreach (byte b in hashedBytes)
            {
                hashedPassword.Append(b.ToString("x2"));
            }

            return hashedPassword.ToString();
        }

        public static string GetHashPassword(string password, out byte[] salt)
        {
            // Generate a random salt
            salt = GenerateSalt();

            // Hash the password with the salt
            return HashPassword(password, salt);
        }

        public static string GetHashPassword(string password)
        {
            // Hash the password with the salt
            return HashPassword(password, new byte[] { 0xFF, 0xFF });
        }

        /// <summary>
        /// Verifica se um valor criptografado coincide com um outro valor a ser criptografado
        /// </summary>
        /// <param name="valueOrigin">Valor sem criptografia</param>
        /// <param name="valueCrypt">Valor criptografado</param>
        /// <param name="algorithm">Algoritmo</param>
        /// <returns>True se o valor coincidir</returns>
        public static bool CheckCrypt(string valueOrigin, string valueCrypt, EnumCryptAlgorithm algorithm)
        {
            if (string.IsNullOrEmpty(valueOrigin))
                throw new NullReferenceException("Valor de conferência em branco.");
            else if (string.IsNullOrEmpty(valueCrypt))
                throw new NullReferenceException("Valor criptografado em branco.");
            else
                return Crypt(valueOrigin, algorithm) == valueCrypt;
        }

        /// <summary>
        /// Retorna o conteudo encriptografado como string
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static string EncryptData(string Message)
        {
            byte[] Results;
            UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(ENCRYPTKEY));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToEncrypt = UTF8.GetBytes(Message);
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return Convert.ToBase64String(Results);
        }

        /// <summary>
        /// Retorna o content criptografado como string a partir de um EnumCryptKey
        /// </summary>
        /// <param name="decryptedContent">Conteúdo a ser criptografado</param>
        /// <param name="enumCryptKey">Chave de criptografia</param>
        /// <returns></returns>
        public static string EncryptData(string decryptedContent, EnumCryptKeys enumCryptKey)
        {
            byte[] Results;
            UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(GetKeyByEnumCrypt(enumCryptKey)));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToEncrypt = UTF8.GetBytes(decryptedContent);
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return Convert.ToBase64String(Results);
        }

        /// <summary>
        /// Retorna o conteúdo dado o conteúdo criptografado e um EnumCryptKey
        /// </summary>
        /// <param name="encryptedContent">Conteúdo a ser descriptografado</param>
        /// <param name="enumCryptKey">Chave de criptografia</param>
        /// <returns></returns>
        public static string DecryptData(string encryptedContent, EnumCryptKeys enumCryptKey)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(GetKeyByEnumCrypt(enumCryptKey)));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToDecrypt = Convert.FromBase64String(encryptedContent);
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return UTF8.GetString(Results);
        }

        /// <summary>
        /// Retorna o conteudo encriptografado como byte array
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static byte[] EncryptDataByteArr(string Message)
        {
            byte[] Results;
            UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(ENCRYPTKEY));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToEncrypt = UTF8.GetBytes(Message);
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            return Results;
        }

        public static string DecryptData(string Message, string key)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(key));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToDecrypt = Convert.FromBase64String(Message);
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return UTF8.GetString(Results);
        }

        public static string GetSHA1(string strPlain)
        {
            UnicodeEncoding UE = new UnicodeEncoding();

            byte[] HashValue, MessageBytes = UE.GetBytes(strPlain);

            SHA1Managed SHhash = new SHA1Managed();

            string strHex = "";

            HashValue = SHhash.ComputeHash(MessageBytes);

            foreach (byte b in HashValue)
            {
                strHex += String.Format("{0:x2}", b);
            }

            return strHex;
        }
    }
}