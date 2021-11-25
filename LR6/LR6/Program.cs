using System;
using System.Numerics;
using System.Text;

namespace LR6
{
    class Program
    {
        static void Main(string[] args)
        {
            GOST_3410 a = new GOST_3410();
            var text = "Players use long wooden hammers wooden balls along the grass and through!";
            var h = Encoding.UTF8.GetBytes(text);
            var text1 = "use long wooden hammers wooden balls along the grass and through!!";
            var h1 = Encoding.UTF8.GetBytes(text1);
            var s = a.Sign(h);
            Console.WriteLine(a.Verify(h1, s.Item1, s.Item2));
        }


    }
}
