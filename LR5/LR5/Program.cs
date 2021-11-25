using System;
using System.IO;
using System.Linq;
using System.Text;

namespace LR5
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Read("text.txt");
            var resTest = MD5.Calculate(input);

            Console.WriteLine(resTest);
        }

        public static byte[] Read(string fileName)
        {
            using (FileStream fs = File.OpenRead(fileName))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data);
                return data;
            }
        }
    }
}
