using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Core.Crypto
{
    /// <summary>
    /// AES Service
    /// </summary>
    public class AESService
    {
        private readonly AesManaged _aesManaged = null;
        private readonly string _key = string.Empty;
        private readonly string _iv = string.Empty;
        private readonly CipherMode _cipherMode = CipherMode.CBC;
        private readonly PaddingMode _paddingMode = PaddingMode.PKCS7;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="key">Key for service</param>
        /// <param name="iv">IV for service</param>
        /// <param name="cipherMode">Cipher mode</param>
        /// <param name="paddingMode">Padding mode</param>
        public AESService(string key, string iv = "", CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            _key = key;
            _iv = iv;
            _cipherMode = cipherMode;
            _paddingMode = paddingMode;

            _aesManaged = CreateInstance();
        }

        /// <summary>
        /// Flag to fail silent
        /// </summary>
        public bool IgnoreFails { get; set; } = false;

        /// <summary>
        /// Decrypt value
        /// </summary>
        /// <param name="toDecrypt">Value to decrypt</param>
        /// <returns>Decryted value</returns>
        public string Decrypt(string toDecrypt)
        {
            var result = string.Empty;

            try
            {
                if (Validate(toDecrypt, out List<Exception> exceptions))
                {
                    var binaryString = CryptoHelper.Hex2Binary(toDecrypt);

                    if (_aesManaged is AesManaged)
                    {
                        var decryptBytes = _aesManaged
                            .CreateDecryptor()
                            .TransformFinalBlock(binaryString, 0, binaryString.Length);

                        result = Encoding.UTF8.GetString(decryptBytes);
                    }
                    else
                    {
                        throw new TypeInitializationException("AesManaged", new Exception("AesManaged initializtion failed"));
                    }
                }
                else
                {
                    var message = string.Empty;

                    foreach (var exception in exceptions)
                    {
                        message += exception.Message + ", ";
                    }

                    throw new ArgumentException(string.IsNullOrEmpty(message) ? "Failed to decrypt as due invalid config" : message);
                }
            }
            catch (Exception ex)
            {
                if (!IgnoreFails)
                {
                    throw ex;
                }
            }

            return result;
        }

        /// <summary>
        /// Create instance
        /// </summary>
        /// <returns>AesManaged instance</returns>
        private AesManaged CreateInstance()
        {
            AesManaged result = new AesManaged();

            result.Key = Encoding.UTF8.GetBytes(_key);
            result.Mode = _cipherMode;
            result.Padding = _paddingMode;

            if (!string.IsNullOrEmpty(_iv))
            {
                result.IV = Encoding.UTF8.GetBytes(_iv);
            }
            else
            {
                result.IV = Enumerable.Repeat((byte)0x00, 16).ToArray();
            }

            return result;
        }

        /// <summary>
        /// Validate before operation
        /// </summary>
        /// <param name="toValidate">Value to validate</param>
        /// <param name="exceptions">Exception collection</param>
        /// <returns>Is valid</returns>
        private bool Validate(string toValidate, out List<Exception> exceptions)
        {
            bool result = false;
            exceptions = new List<Exception>();

            if (string.IsNullOrEmpty(toValidate))
            {
                exceptions.Add(new Exception("Value is empty"));
            }

            if (string.IsNullOrEmpty(_key))
            {
                exceptions.Add(new Exception("Key is empty"));
            }

            if (exceptions == null || exceptions.Count == 0)
            {
                result = true;
            }

            return result;
        }
    }
}
