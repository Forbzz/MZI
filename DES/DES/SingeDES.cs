using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES
{
    public static class SingeDES
    {
        public static string Encrypt(string data, DESDerivedKey key) =>
            DESCrypto.Encrypt(data, key);

        public static string Decrypt(string data, DESDerivedKey key)
        {
            key.Reverse();
            return DESCrypto.Encrypt(data, key);
        }
    }
}
