using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Security.Cryptography;

namespace LR7
{
    public class ECDSA
    {
        public EllipticCurve Curve { get; }
        public ECDSA()
        {

        }

        public ECDSA(EllipticCurve curve)
        {
            Curve = curve;
        }

        public (BigInteger privateKey, BigIntegerPoint publicKey) GenerateKeyPair()
        {
            BigInteger privateKey = RandomIntegerBelow(Curve.n);
            BigIntegerPoint publicKey = ScalarMult(privateKey, Curve.G);
            return (privateKey, publicKey);
        }

        public BigInteger HashMessage(string message)
        {
            byte[] messageBytes = Encoding.Default.GetBytes(message);
            byte[] messageHash;

            using (SHA512Managed sha512M = new SHA512Managed())
            {
                messageHash = sha512M.ComputeHash(messageBytes);
            }

            BigInteger e = new BigInteger(messageHash);

            return e;
        }

        public (BigInteger r, BigInteger s) SignMessage(BigInteger privateKey, string message)
        {
            BigInteger z = HashMessage(message);

            BigInteger r = 0;
            BigInteger s = 0;

            while (r == 0 || s == 0)
            {
                BigInteger k = RandomIntegerBelow(Curve.n);
                BigIntegerPoint point = ScalarMult(k, Curve.G);

                r = point.X % Curve.n;

                s = ((z + r * privateKey) * InverseMod(k, Curve.n)) % Curve.n;
            }

            return (r, s);
        }

        public bool VerifySignature(BigIntegerPoint publicKey, string message,
            (BigInteger r, BigInteger s) signature)
        {
            BigInteger h = HashMessage(message);

            BigInteger w = InverseMod(signature.s, Curve.n);
            BigInteger u1 = h * w % Curve.n;
            BigInteger u2 = signature.r * w % Curve.n;


            BigIntegerPoint point = PointAdd(ScalarMult(u1, Curve.G), ScalarMult(u2, publicKey));
            return signature.r % Curve.n == point.X % Curve.n;
        }

        public BigIntegerPoint ScalarMult(BigInteger k, BigIntegerPoint point)
        {
            if (k < 0)
            {
                return ScalarMult(-k, NegatePoint(point));
            }

            BigIntegerPoint result = BigIntegerPoint.GetEmpty();
            BigIntegerPoint addend = point;

            while (k > 0)
            {
                if ((k & 1) > 0)
                {
                    result = PointAdd(result, addend);
                }

                addend = PointAdd(addend, addend);

                k >>= 1;
            }

            return result;
        }

        public BigIntegerPoint PointAdd(BigIntegerPoint point1, BigIntegerPoint point2)
        {
            if (point1.IsEmpty())
            {
                return point2;
            }

            if (point2.IsEmpty())
            {
                return point1;
            }

            BigInteger x1 = point1.X, y1 = point1.Y;
            BigInteger x2 = point2.X, y2 = point2.Y;

            if (x1 == x2 && y1 != y2)
            {
                return BigIntegerPoint.GetEmpty();
            }

            BigInteger m = BigInteger.Zero;
            if (x1 == x2)
            {
                m = (3 * x1 * x1 + Curve.a) * InverseMod(2 * y1, Curve.p);
            }
            else
            {
                m = (y1 - y2) * InverseMod(x1 - x2, Curve.p);
            }

            BigInteger x3 = m * m - x1 - x2;
            BigInteger y3 = y1 + m * (x3 - x1);
            //BigIntegerPoint result = new BigIntegerPoint(MathMod(x3, Curve.p), MathMod(-y3, Curve.p));
            BigIntegerPoint result = new BigIntegerPoint((x3 % Curve.p), (-y3 % Curve.p));

            return result;
        }

        public (BigInteger, BigInteger, BigInteger) Gcdex(BigInteger a, BigInteger b)
        {
            if (a == 0)
                return (b, 0, 1);
            (BigInteger gcd, BigInteger x, BigInteger y) = Gcdex(b % a, a);
            return (gcd, y - (b / a) * x, x);
        }

        public  BigInteger InverseMod(BigInteger a, BigInteger m)
        {
            if(a < 0)
            {
                return m -InverseMod(-a, m);
            }
            (BigInteger g, BigInteger x, BigInteger y) = Gcdex(a, m);
            return g > 1 ? 0 : (x % m + m) % m;
        }

        public BigIntegerPoint NegatePoint(BigIntegerPoint point)
        {
            if (point.IsEmpty())
            {
                return point;
            }

            BigInteger x = point.X;
            BigInteger y = point.Y;
            BigIntegerPoint result = new BigIntegerPoint(x, (-y % Curve.p));

            return result;
        }

        public static BigInteger RandomIntegerBelow(BigInteger N)
        {
            byte[] bytes = N.ToByteArray();
            BigInteger R;
            Random random = new Random();
            do
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= 0x7F;
                R = new BigInteger(bytes);
            } while (R >= N);

            return R;
        }
    }
}
