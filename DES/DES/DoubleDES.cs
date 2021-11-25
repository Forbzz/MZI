using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES
{
    public static class DoubleDES
    {
        public static string Encrypt(string data, DESDerivedKey key1, DESDerivedKey key2) =>
            DESCrypto.Encrypt(DESCrypto.Encrypt(data, key1), key2);

        public static string Decrypt(string data, DESDerivedKey key1, DESDerivedKey key2)
        {
            key1.Reverse();
            key2.Reverse();
            return DESCrypto.Encrypt(DESCrypto.Encrypt(data, key2), key1);
        }
            
    }
}
