using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR6
{
    public class GOST_3411_94
    {
        private int[] KappaIndexes = { 0, 1, 2, 3, 12, 15 };
        private byte[] ExtendData(byte[] data)
        {
            var dif = 32 - (data.Length) % 32;
            var res = new byte[data.Length + dif];
            //Array.Reverse(data);
            data.CopyTo(res, res.Length - data.Length);
            return res;
        }

        private byte[] GetC3() => new byte[32] { 255, 0, 255, 255, 0, 0, 0, 255, 255,
                0, 0, 255, 0, 255, 255, 0, 0, 255, 0, 255, 0, 255, 0, 255, 255,
                0, 255, 0, 255, 0, 255, 0 };

        public byte[] GetHashe(byte[] data, byte[] H)
        {
            data = ExtendData(data);

            for (int i = 0; i < data.Length / 32; i++)
            {
                var M = data.Skip(32 * i).Take(32).ToArray();
                var keys = GenerateKeys(H, M);
                H = Ksi(M, H, keys);
            }

            return H;
        }
        private byte[] Ksi(byte[] M, byte[] H, byte[][] keys)
        {
            var S = E(H, keys);
            Array.Reverse(S);
            int i = 0;
            var X = S;
            while (i < 12)
            {
                X = Kappa(X);
                i++;
            }
            X = XOR(M, X);
            X = Kappa(X);
            i = 0;
            while (i < 61)
            {
                X = Kappa(X);
                i++;
            }
            return X;
        }
        private byte[] Kappa(byte[] data)
        {
            var sData = GetShortArrFromByteArr(data);

            var result = new ushort[sData.Length];
            sData.CopyTo(result, 0);

            var res = new ushort[sData.Length];

            res[0] = (ushort)(sData[0] ^ sData[1] ^ sData[2] ^ sData[3] ^ sData[12] ^ sData[15]);

            for (int i = 1; i < sData.Length; i++)
            {
                res[i] = sData[^(i)];
            }
            Array.Reverse(res);


            return GetByteArrFromShortArr(res);
        }

        private ushort[] GetShortArrFromByteArr(byte[] data)
        {
            var res = new ushort[data.Length / 2];
            for (int i = 0; i < data.Length / 2; i++)
            {
                res[i] = BitConverter.ToUInt16(data, i * 2);
            }
            return res;
        }

        private byte[] GetByteArrFromShortArr(ushort[] data)
        {
            var res = new byte[data.Length * 2];
            for (int i = 0; i < data.Length; i++)
            {
                BitConverter.GetBytes(data[i]).CopyTo(res, i * 2);
            }
            return res;
        }


        public byte[] E(byte[] H, byte[][] keys)
        {
            byte[][] S = new byte[4][];
            for (int i = 0, k = 0; i < 4; i++, k++)
            {
                var data = H.Skip(i * 8).Take(8).ToArray();
                S[i] = GOST_28147_89.Encrypt(data, keys[k]);
            }

            byte[] res = new byte[H.Length];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < S[i].Length; j++)
                {
                    res[i * 8 + j] = S[i][j];
                }
            }
            return res;
        }
        public byte[][] GenerateKeys(byte[] H, byte[] M)
        {
            byte[][] C = new byte[4][];
            C[0] = new byte[32];
            C[1] = new byte[32];
            C[2] = GetC3();
            C[3] = new byte[32];
            byte[][] K = new byte[4][];
            byte[] U = new byte[H.Length], V = new byte[M.Length];

            H.CopyTo(U, 0);
            M.CopyTo(V, 0);
            var W = XOR(U, V);
            K[0] = P(W);
            for (int i = 1; i < 4; i++)
            {
                U = XOR(A(U), C[i]);
                V = A(A(V));
                W = XOR(U, V);
                K[i] = P(W);
            }
            return K;
        }
        private byte[] P(byte[] data)
        {
            var res = new byte[data.Length];
            for (int i = 0; i < 4; i++)
            {
                for (int k = 1; k < 9; k++)
                {
                    res[8 * i + k - 1] = data[i + 4 * (k - 1)];
                }
            }
            return res;
        }
        private byte[] A(byte[] data)
        {
            var x = new List<byte>[4];
            var res = new List<byte>();

            for (int i = 0; i < 4; i++)
            {
                x[i] = data.Skip(i * 8).Take(8).ToList();
            }
            res.AddRange(XOR(x[0], x[1]));
            res.AddRange(x[3]);
            res.AddRange(x[2]);
            res.AddRange(x[1]);
            return res.ToArray();
        }
        public List<byte> XOR(List<byte> a, List<byte> b)
        {
            for (int i = 0; i < a.Count; i++)
            {
                a[i] ^= b[i];
            }
            return a;
        }
        public byte[] XOR(byte[] a, byte[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i] ^= b[i];
            }
            return a;
        }
    }
}
