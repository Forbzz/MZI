using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace DES
{
    class Program
    {

        static void Main(string[] args)
        {
            TaskDES("text.txt");
            TaskGOST("text.txt");
        }

        public static void TaskDES(string filename)
        {
            string text = Read(filename);
            DESDerivedKey desKey = new DESDerivedKey(Utils.RandomKey());
            DESDerivedKey desKey1 = new DESDerivedKey(Utils.RandomKey());
            DESDerivedKey desKey2 = new DESDerivedKey(Utils.RandomKey());

            string binText = Utils.TextToBin(text);

            string cipher_data1 = SingeDES.Encrypt(binText, desKey);
            string decryptData1 = SingeDES.Decrypt(cipher_data1, desKey);
            Console.WriteLine($"SINGE DES:\n{Utils.BinToText(decryptData1)}");

            string cipher_data2 = DoubleDES.Encrypt(binText, desKey, desKey1);
            string decryptData2 = DoubleDES.Decrypt(cipher_data2, desKey, desKey1);
            Console.WriteLine($"DOUBLE DES:\n{Utils.BinToText(decryptData2)}");

            string cipher_data3 = TripleDES.Encrypt(binText, desKey, desKey1, desKey2);
            string decryptData3 = TripleDES.Decrypt(cipher_data3, desKey, desKey1, desKey2);
            Console.WriteLine($"TRIPLE DES:\n{Utils.BinToText(decryptData3)}");
        }

        public static void TaskGOST(string filename)
        {
            List<byte> full_encode = new List<byte>();
            List<byte> full_decode = new List<byte>();
            string text = Read(filename);

            byte[] testBytes = Encoding.UTF8.GetBytes(text);
            uint[] rkey = Utils.GenerateKey();
            byte[] encode;
            byte[] decode;

            int add = 8 - testBytes.Length % 8;
            var textBytes = testBytes.ToList();
            for (int i = 0; i < add; i++)
            {
                textBytes.Add(0);
            }
            
            GOST gost = new GOST(rkey);

            for (int i = 0; i < textBytes.Count / 8; i++)
            {
                var val = textBytes.Skip(i * 8).Take(8).ToArray();
                encode = gost.Encrypt(val);
                full_encode.AddRange(encode);
            }


            for (int i = 0; i < textBytes.Count / 8; i++)
            {
                var val = full_encode.Skip(i * 8).Take(8).ToArray();
                decode = gost.Decrypt(val);
                full_decode.AddRange(decode);

            }
            string encodedText = Encoding.UTF8.GetString(full_decode.ToArray());
            Console.WriteLine("GOST\n" + Encoding.UTF8.GetString(full_decode.ToArray()));
        }

        public static void Write(string data, string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                byte[] array = Encoding.UTF8.GetBytes(data);
                fs.Write(array, 0, array.Length);
            }
        }

        public static string Read(string filename)
        {

            using (FileStream fs = File.OpenRead(filename))
            {
                byte[] array = new byte[fs.Length];
                fs.Read(array, 0, array.Length);
                return Encoding.UTF8.GetString(array);
            }
        }

        
    }
}
