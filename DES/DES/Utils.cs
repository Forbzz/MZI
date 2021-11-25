using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES
{
    public static class Utils
    {
        public static uint[] GenerateKey()
        {
            Random rand = new Random();
            uint[] arr = new uint[8];
            for (int i = 0; i < 8; i++)
            {
                arr[i] = (uint)rand.Next(0, 100000);
            }
            return arr;

        }
        public static string RandomKey()
        {
            Random rand = new Random();
            StringBuilder key = new StringBuilder();
            for (int i = 0; i < 64; i++)
            {
                key.Append((char)('0' + rand.Next(2)));
            }
            return key.ToString();
        }

        public static string Permute(string k, int[] arr, int n)
        {
            StringBuilder per = new StringBuilder();
            for (int i = 0; i < n; i++)
            {
                per.Append(k[arr[i] - 1]);
            }
            return per.ToString();
        }

        public static string TextToBin(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            StringBuilder bin = new();
            foreach (byte b in bytes)
            {
                string t;
                t = (Convert.ToString(b, 2));
                while(t.Length < 8)
                {
                    t = "0" + t;
                }
                bin.Append(t);
            }

            return bin.ToString();
        }

        public static string BinToText(string s)
        {
            StringBuilder text = new StringBuilder();
            for (int i = 0; i <= s.Length - 8; i += 8)
            {
                text.Append((char)Convert.ToByte(s.Substring(i, 8), 2));
            }

            return text.ToString();
        }

    }
}
