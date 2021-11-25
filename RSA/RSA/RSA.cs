using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    public class RSA
    {
        public RSA()
        {

        }

        public long GCD(long a, long b)
        {
            long temp;
            while(b != 0)
            {
                temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
      
        public int[] EncryptMy(byte[] data, int p, int q)
        {
            if(IsTheNumberSimple(p)==false || IsTheNumberSimple(q) == false)
            {
                throw new ArgumentException("Numbers should be prime!");
            }

            int n = p * q;
            int f = (p - 1) * (q - 1);
            int d = Calculate_d(f);
            int e = Calculate_e(d, f);

            int[] bits = new int[data.Length];
            for (long i = 0; i < data.Length; i++)
            {
                BigInteger t = BigInteger.ModPow(data[i], e, n);

                bits[i] = (int)t;
            }
            return bits;
        }

        public byte[] Decrypt(int[] data, int p, int q)
        {
            var res = new byte[data.Length];

            int n = p * q;
            int f = (p - 1) * (q - 1);
            int d = Calculate_d(f);
            int e = Calculate_e(d, f);

            for (long i = 0; i < data.Length; i++)
            {
                BigInteger t = BigInteger.ModPow(data[i], d, n);

                res[i] = (byte)t;
            }

            return res;
        }
        public int Calculate_d(int f)
        {
            int d = 2;

            while(true)
            {
                if (GCD(d, f) == 1)
                    return d;
                d++;
            }
        }

        private int Calculate_e(int d, int f)
        {
            int e = 7;

            while (true)
            {
                if ((e * d) % f == 1)
                    break;
                else
                    e++;
            }

            return e;
        }

        private bool IsTheNumberSimple(int n)
        {
            if (n < 2)
                return false;

            if (n == 2)
                return true;

            for (int i = 2; i < n; i++)
                if (n % i == 0)
                    return false;

            return true;
        }
    }


}

