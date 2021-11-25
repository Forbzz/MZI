using System;
using System.Text;

namespace LR2_MZI
{
    class Program
    {
        static void Main(string[] args)
        {
            uint[] k = new uint[] { 2, 3, 4, 5, 6, 7, 8, 9 };
            byte[] text = Encoding.UTF8.GetBytes("Sariel is basically the standard pre-emptive converter. Incredibly useful due to pre-emptive converters being that much in demand, but not really providing any particularly interesting value outside of it");
            uint[] arr = new uint[text.Length];
            for(int i = 0;i<text.Length;i++)
            {
                arr[i] = text[i];
            }
            
            var encrypt = STB.SimpleReplaceEncrypt(arr, k);
            var decrypt = STB.SimpleReplaceDecrypt(encrypt, k);
            byte[] result = new byte[decrypt.Length];
            for (int i = 0; i < text.Length; i++)
            {
                result[i] = (byte)decrypt[i];
            }

            Console.WriteLine(Encoding.UTF8.GetString(result));
        }
    }
}
