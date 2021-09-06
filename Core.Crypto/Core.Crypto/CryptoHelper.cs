using System;

namespace Core.Crypto
{
    /// <summary>
    /// Crypto Helper
    /// </summary>
    public class CryptoHelper
    {
        /// <summary>
        /// Convert encrypted hex string value into binary and return as Bytes
        /// </summary>
        /// <param name="HexStr"></param>
        /// <returns></returns>
        internal static byte[] Hex2Binary(string HexStr)
        {
            try
            {
                if (HexStr.Length % 2 != 0)
                {
                    HexStr = "0" + HexStr;
                }

                int HexLength = HexStr.Length;
                byte[] Valuebytes = new byte[HexLength / 2];

                for (int i = 0; i < HexLength; i += 2)
                {
                    Valuebytes[i / 2] = Convert.ToByte(HexStr.Substring(i, 2), 16);
                }

                return Valuebytes;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }
    }
}
