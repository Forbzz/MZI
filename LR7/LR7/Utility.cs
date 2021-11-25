using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LR7
{
    public class Utility
    {
        public static Random Gen = new Random();
        public static BigInteger RandomIntegerBelow(BigInteger N)
        {
            byte[] bytes = N.ToByteArray();
            BigInteger R;

            do
            {
                Gen.NextBytes(bytes);
                bytes[bytes.Length - 1] &= 0x7F; 
                R = new BigInteger(bytes);
            }
            while (R > N || R < 1);
            return R;
        }

        public static BigInteger GetProcessedNumber(byte[] basenumber, int numberofbit)
        {
            int partition = (numberofbit / 8 - basenumber.Length);
            int firstArrayLength;
            int lastArrayLength;
            if (partition % 2 == 0)
            {
                firstArrayLength = partition / 2;
                lastArrayLength = partition / 2;
            }
            else
            {
                firstArrayLength = partition / 2 + 1;
                lastArrayLength = partition / 2;
            }
            byte[] firstRandomPart = new byte[firstArrayLength];
            byte[] lastRandomPart = new byte[lastArrayLength];

            Gen.NextBytes(firstRandomPart);
            Gen.NextBytes(lastRandomPart);
 
            lastRandomPart[lastRandomPart.Length - 1] &= 0x7F;
            IEnumerable<byte> number = firstRandomPart.Concat(basenumber).Concat(lastRandomPart);
            return new BigInteger(number.ToArray());
        }
        public static string GetTextFromProcessedNumber(BigInteger processedNum, int textbit)
        {
            byte[] bytes = processedNum.ToByteArray();
            int partition = (bytes.Length - textbit / 8);
            int firstArrayLength;

            if (partition % 2 == 0)
            {
                firstArrayLength = partition / 2;
            }
            else
            {
                firstArrayLength = partition / 2 + 1;
            }
            byte[] messageToByte = new byte[textbit / 8];
            for (int i = 0; i < textbit / 8; i++)
            {
                messageToByte[i] = bytes[i + firstArrayLength];
            }
            return Encoding.UTF8.GetString(messageToByte);
        }

        public static BigInteger SquareRoot(BigInteger n, BigInteger p)
        {
            if (p % 4 != 3)
            {
                return BigInteger.Zero;
            }

            n = n % p;
            BigInteger x = BigInteger.ModPow(n, (p + 1) / 4, p);
            if ((x * x) % p == n)
            {
                return x;
            }
            x = p - x;
            if ((x * x) % p == n)
            {
                return x;
            }
            return BigInteger.Zero;
        }

        public static List<BigIntegerPoint> EmbededTextOnCurve(string m, EllipticCurve E)
        {
            List<BigIntegerPoint> messagePoints = new List<BigIntegerPoint>();


            int lengthOfPoint = (int)((BigInteger.Log(E.p, 2)) / 8);
            //int lengthEmbeded = 64; 
            while (m.Length % 8 != 0)  
            {
                m += " ";
            }
            List<string> substring = Split(m, 8).ToList();
            //var arr = Encoding.UTF8.GetBytes(m);

            foreach (string s in substring)
            {
                BigInteger xP = 0, yP = 0;
                do
                {
                    do
                    {
                        xP = GetProcessedNumber(Encoding.UTF8.GetBytes(s), lengthOfPoint * 8);
                    } while (xP == 0);
                    yP = SquareRoot(BigInteger.Pow(xP, 3) + E.a * xP + E.b, E.p);
                } while (yP == 0);

                messagePoints.Add(new BigIntegerPoint(xP, yP));
            }

            return messagePoints;
        }
        private static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }
    }
}
