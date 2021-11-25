using System;
using System.IO;
using System.Text;

namespace LR4_MZI
{
    class Program
    {
        static void Main(string[] args)
        {

            byte[] text = FileRead("text.txt");
            ElGamal cipher = new ElGamal();
            (var A, var B) = cipher.Encypt(text);

            var decode = cipher.Decrypt(A, B);
            Console.WriteLine(Encoding.UTF8.GetString(decode));

        }

        public static byte[] FileRead(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data);
                return data;
            }
        }
    }
}
