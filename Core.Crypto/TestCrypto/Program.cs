using System;

namespace TestCrypto
{
    class Program
    {
        static void Main(string[] args)
        {
            Core.Crypto.AESService service = new Core.Crypto.AESService("<key goes here>");

            Console.WriteLine($"Encrypted string: D869C2E0895B822465925C1C41F11339 Decrypted string: {service.Decrypt("D869C2E0895B822465925C1C41F11339")}");
        }
    }
}
