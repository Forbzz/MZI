using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LR7
{
    public class BigIntegerPoint
    {
        public BigInteger X { get; set; }
        public BigInteger Y { get; set; }

        public BigIntegerPoint(BigInteger x, BigInteger y)
        {
            X = x;
            Y = y;
        }

        public static BigIntegerPoint GetEmpty()
        {
            return new BigIntegerPoint(0, 0);
        }

        public bool IsEmpty() => X == BigInteger.Zero && Y == BigInteger.Zero;

        //public static BigIntegerPoint Multiply(BigIntegerPoint P, BigInteger n, BigInteger a, BigInteger p)
        //{
        //    BigIntegerPoint Q = new BigIntegerPoint(0, 0);
        //    BigIntegerPoint R = P;

        //    string n_bin = Utility.byteArrayToBinaryString(n.ToByteArray());
        //    for (int i = n_bin.Length - 1; i >= 0; i--)
        //    {
        //        if (n_bin[i] == '1')
        //        {
        //            Q = Add(Q, R, p);
        //        }
        //        R = Doubling(R, a, p);
        //    }
        //    return Q;
        //}

        //public static BigIntegerPoint Add(BigIntegerPoint a, BigIntegerPoint b, BigInteger p)
        //{
        //    if (a.X == 0 && a.Y == 0)
        //    {
        //        return b;
        //    }
        //    else if (b.X == 0 && b.Y == 0)
        //    {
        //        return a;
        //    }
        //    else if (a.X == b.X)
        //    {
        //        return new BigIntegerPoint(0, 0);
        //    }
        //    else
        //    {
        //        BigInteger lamda = Utility.Modulo((b.Y - a.Y) * Utility.Reverse_modulo(b.X - a.X, p), p);
        //        BigInteger cx = Utility.Modulo(lamda * lamda - a.X - b.X, p);
        //        BigInteger cy = Utility.Modulo(lamda * (a.X - cx) - a.Y, p);
        //        return new BigIntegerPoint(cx, cy);
        //    }
        //}

        //public static BigIntegerPoint Doubling(BigIntegerPoint P, BigInteger a, BigInteger p)
        //{
        //    BigInteger lamda = Utility.Modulo((3 * P.X * P.X + a) % p * Utility.Reverse_modulo(2 * P.Y, p), p);
        //    BigInteger cx = Utility.Modulo(lamda * lamda - 2 * P.X, p);
        //    BigInteger cy = Utility.Modulo(lamda * (P.X - cx) - P.Y, p);
        //    return new BigIntegerPoint(cx, cy);
        //}

        //public static BigIntegerPoint Subtract(BigIntegerPoint P, BigIntegerPoint Q, BigInteger p)
        //{
        //    BigIntegerPoint R = Add(P, Reverse(Q, p), p);
        //    return R;
        //}

        //public static BigIntegerPoint Reverse(BigIntegerPoint P, BigInteger p)
        //{

        //    BigInteger Qx = P.X;
        //    BigInteger Qy = Utility.Modulo(-P.Y, p);
        //    return new BigIntegerPoint(Qx, Qy);
        //}
    }
}
