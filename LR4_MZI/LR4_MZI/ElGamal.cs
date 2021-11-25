using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LR4_MZI
{
    public class ElGamal
    {
        public int X { get; set; }
        private int Y { get; set; }
        private int P { get; set; }
        public int G { get; set; }

        public ElGamal()
        {
            GenerateKeys();
        }

        public void GenerateKeys()
        {
            Random rand = new Random();
            P = GetPrimeP();
            G = GetPrimitiveRoot(P);
            X = rand.Next(1, P - 1);
            Y = (int)BigInteger.ModPow(G, X, P);

        }

        public (long, long[]) Encypt(byte[] data)
        {
            Random rand = new Random();
            var k = rand.Next(1, P - 1);
            long A;
            long[] B = new long[data.Length];
            A = (long)BigInteger.ModPow(G, k, P);
            for (int i = 0;i<data.Length;i++)
            {
                B[i] = (long)(BigInteger.Pow(Y, k) * data[i] % P);
            }

            return (A, B);
        }

        public byte[] Decrypt(long A, long[] B)
        {
            byte[] res = new byte[B.Length];
            for(int i = 0;i<res.Length;i++)
            {
                res[i] = (byte)(B[i] * BigInteger.Pow(A, P - 1 - X) % P);
            }

            return res;
        }

        private int GetPrimeP()
        {
            int max = 8000;
            List<int> primes = new List<int>();
            bool isPrime;

            for (int j = 1000; j < max; j++)
            {
                isPrime = true;
                for (int i = 2; i < j; i++)
                {
                    if (j % i == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }

                if (isPrime)
                {
                    primes.Add(j);
                }
            }

            Random rand = new Random();
            return primes[rand.Next(primes.Count)];

        }

        public int GetPrimitiveRoot(int p)
        {
            List<int> arr = new List<int>();
            int phi = p - 1, n = phi;
            for (int i = 2; i * i <= n; i++)
            {
                if (n % i == 0)
                {
                    arr.Add(i);
                    while (n % i == 0)
                    {
                        n /= i;
                    }
                }
            }

            if (n > 1)
            {
                arr.Add(n);
            }
            for (int g = 2; g <= p; g++)
            {
                bool ok = true;
                for (int i = 0; i < arr.Count() && ok; i++)
                {
                    ok &= BigInteger.ModPow(g, phi / arr[i], p) != 1;
                }
                if (ok)
                {
                    return g;
                }
            }
            throw new Exception("Cannot find primitive root");
        }
    }
}
