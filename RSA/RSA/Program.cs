using System;
using System.IO;
using System.Text;

namespace RSA
{
    class Program
    {
        static string text = "text.txt";

        static void Main(string[] args)
        {
            RSA rSA = new();
            var input = Read(text);

            var enc = rSA.EncryptMy(input, 1091,1049);

            var dec = rSA.Decrypt(enc, 1091, 1049);

            Console.WriteLine($"Decrypt:{ Encoding.UTF8.GetString(dec)}");

        }
        static byte[] Read(string path)
        {
            FileInfo f = new FileInfo(path);
            byte[] data = new byte[f.Length];
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                fs.Read(data, 0, (int)f.Length);
            }
            return data;
        }
        static void Write(byte[] data, string path)
        {
            using FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            {
                fs.Write(data, 0, data.Length);
            }
        }
    }
}
