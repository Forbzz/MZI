using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LR7
{
    public class ECIE
    {
        public static List<Tuple<BigIntegerPoint, BigIntegerPoint>> Encryption(string message, EllipticCurve E, BigIntegerPoint PublicKey)
        {
            List<BigIntegerPoint> plainText = Utility.EmbededTextOnCurve(message, E);
            EllipticCurve curve = new EllipticCurve();
            ECDSA ecdsa = new ECDSA(curve);

            List<Tuple<BigIntegerPoint, BigIntegerPoint>> cyphertext = new List<Tuple<BigIntegerPoint, BigIntegerPoint>>();
            foreach (var point in plainText)
            {
                BigInteger k = Utility.RandomIntegerBelow(E.n);
                BigIntegerPoint m1 = ecdsa.ScalarMult(k, E.G);
                BigIntegerPoint m2 = ecdsa.PointAdd(point, ecdsa.ScalarMult(k, PublicKey));
                cyphertext.Add(new Tuple<BigIntegerPoint, BigIntegerPoint>(m1, m2));
            }
            return cyphertext;
        }

        public static string Decryption(List<Tuple<BigIntegerPoint, BigIntegerPoint>> cyphertext, BigInteger PrivateKey, EllipticCurve E)
        {
            StringBuilder plaintext = new StringBuilder();
            var curve = new EllipticCurve();
            var ecdsa = new ECDSA(curve);
            foreach (var text in cyphertext)
            {
                BigIntegerPoint S = ecdsa.PointAdd(text.Item2, ecdsa.NegatePoint(ecdsa.ScalarMult(PrivateKey, text.Item1)));
                plaintext.Append(Utility.GetTextFromProcessedNumber(S.X, 64));
            }
            return plaintext.ToString();
        }


    }
}
