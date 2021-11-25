using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LR6
{
    public class GOST_3410
    {
        private readonly byte[] _H = { 11, 25, 34, 41, 78, 24, 76, 78, 21, 38, 45, 46, 15, 51, 61, 71, 18, 25, 21, 11, 5, 6, 7, 8, 9, 10, 86, 24, 144, 125, 122, 59 };
        public GOST_3410()
        {
            GenerateKeys();
        }
        private BigInteger P { get; set; }
        private BigInteger Q { get; set; }
        private BigInteger X { get; set; }
        public BigInteger Y { get; set; }
        public BigInteger G { get; set; }

        static (BigInteger, BigInteger, BigInteger) Gcdex(BigInteger a, BigInteger b)
        {
            if (a == 0)
                return (b, 0, 1);
            (BigInteger gcd, BigInteger x, BigInteger y) = Gcdex(b % a, a);
            return (gcd, y - (b / a) * x, x);
        }

        static BigInteger InvMod(BigInteger a, BigInteger m)
        {
            (BigInteger g, BigInteger x, BigInteger y) = Gcdex(a, m);
            return g > 1 ? 0 : (x % m + m) % m;
        }

        public void GenerateKeys()
        {
            P = BigInteger.Parse("00000000" + "EE8172AE" + "8996608F" + "B69359B8" + "9EB82A69" +
                   "854510E2" + "977A4D63" + "BC97322C" + "E5DC3386" +
                   "EA0A12B3" + "43E9190F" + "23177539" + "84583978" +
                   "6BB0C345" + "D165976E" + "F2195EC9" + "B1C379E3",
                   System.Globalization.NumberStyles.HexNumber);
            Q = BigInteger.Parse("00000000" + "98915E7E" + "C8265EDF" + "CDA31E88" + "F24809DD" +
                "B064BDC7" + "285DD50D" + "7289F0AC" + "6F49DD2D",
                System.Globalization.NumberStyles.HexNumber);

            X = RandBigInt(Q);

            G = BigInteger.Parse("00000000" + "9E960315" + "00C8774A" + "869582D4" + "AFDE2127" +
                   "AFAD2538" + "B4B6270A" + "6F7C8837" + "B50D50F2" +
                   "06755984" + "A49E5093" + "04D648BE" + "2AB5AAB1" +
                   "8EBE2CD4" + "6AC3D849" + "5B142AA6" + "CE23E21c",
                   System.Globalization.NumberStyles.HexNumber);

            Y = BigInteger.ModPow(G, X, P);
        }
        public (byte[], byte[]) Sign(byte[] data)
        {
            var gost3411 = new GOST_3411_94();
            var h = gost3411.GetHashe(data, _H);
            var value = new BigInteger(h);

            var k = RandBigInt(Q);
            var r = BigInteger.ModPow(G, k, P) % Q;
            var s = (k * value + X * r) % Q;
            return (r.ToByteArray(), s.ToByteArray());
        }

        public bool Verify(byte[] data, byte[] r, byte[] s)
        {
            var gost3411 = new GOST_3411_94();
            var h = gost3411.GetHashe(data, _H);
            var value = new BigInteger(h);

            var w = InvMod(value, Q);
 
            var u1 = new BigInteger(s) * w % Q;
            var u2 = ((Q - new BigInteger(r)) * w) % Q;

            return ((BigInteger.ModPow(G, u1, P) * BigInteger.ModPow(Y, u2, P) % P) % Q == new BigInteger(r));
        }

        private BigInteger RandBigInt(BigInteger stop)
        {
            byte[] bytes = stop.ToByteArray();
            Random random = new Random();
            BigInteger R;
            do
            {
                random.NextBytes(bytes);
                var num = new BigInteger(bytes);
                bytes[bytes.Length - 1] &= 0x7F;

                R = new BigInteger(bytes);
            } while (R >= stop);

            return R;
        }

    }
}

