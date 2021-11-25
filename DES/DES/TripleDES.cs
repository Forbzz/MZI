using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES
{
    public static class TripleDES
    {
        public static string Encrypt(string data, DESDerivedKey key1, DESDerivedKey key2, DESDerivedKey key3) =>
            DESCrypto.Encrypt(DESCrypto.Encrypt(DESCrypto.Encrypt(data, key1), key2),key3);

        public static string Decrypt(string data, DESDerivedKey key1, DESDerivedKey key2, DESDerivedKey key3)
        {
            key1.Reverse();
            key2.Reverse();
            key3.Reverse();
            return  DESCrypto.Encrypt(DESCrypto.Encrypt(DESCrypto.Encrypt(data, key3), key2),key1);
        }
    }
}
