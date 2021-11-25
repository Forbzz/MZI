using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR6
{
    public class GOST_28147_89
    {
        public static byte[] Encrypt(byte[] data, byte[] key)
        {
            var res = new byte[data.Length];
            for (int i = 0; i < data.Length / 8; i++)
            {
                var left = BitConverter.ToUInt32(data.Skip(i * 8).Take(4).ToArray());
                var right = BitConverter.ToUInt32(data.Skip(i * 8 + 4).Take(4).ToArray());
                
                for (int i1 = 0; i1 < 24; i1++)
                {
                    uint fResult = left ^ F(right, key[i1 % 8]);
                    left = right;
                    right = fResult;
                }

                for (int i1 = 24, j = 7; i1 < 31; i1++, j--)
                {
                    uint fResult = left ^ F(right, key[j]);
                    left = right;
                    right = fResult;
                }
                left ^= F(right, key[0]);

                var R1 = BitConverter.GetBytes(right);
                var L1 = BitConverter.GetBytes(left);
                for (int k = i * 8; k < i * 8 + 4; k++)
                {
                    res[k] = R1[k - i * 8];
                }
                for (int k = i * 8 + 4; k < i * 8 + 8; k++)
                {
                    res[k] = L1[k - i * 8 - 4];
                }
            }
            return res;
        }
        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            var res = new byte[data.Length];
            for (int i = 0; i < data.Length / 8; i++)
            {
                var left = BitConverter.ToUInt32(data.Skip(i * 8).Take(4).ToArray());
                var right = BitConverter.ToUInt32(data.Skip(i * 8 + 4).Take(4).ToArray());

                for (int i1 = 31, j = 0; i1 >= 24; i1--, j++)
                {
                    uint fResult = left ^ F(right, key[j]);
                    left = right;
                    right = fResult;
                }
                for (int i1 = 23; i1 > 0; i1--)
                {
                    uint fResult = left ^ F(right, key[i1 % 8]);
                    left = right;
                    right = fResult;
                }
                left ^= F(right, key[0]);

                var R1 = BitConverter.GetBytes(right);
                var L1 = BitConverter.GetBytes(left);
                for (int k = i * 8; k < i * 8 + 4; k++)
                {
                    res[k] = R1[k - i * 8];
                }
                for (int k = i * 8 + 4; k < i * 8 + 8; k++)
                {
                    res[k] = L1[k - i * 8 - 4];
                }
            }
            return res;
        }

        private static uint Substitute(uint value)
        {
            byte index, sBlock;
            uint result = 0;
            for (int i = 0; i < 8; i++)
            {
                index = (byte)(value >> (4 * i) & 0x0f);
                sBlock = s[i, index];
                result |= (uint)sBlock << (4 * i);
            }
            return result;
        }

        private static uint F(uint block, uint subKey)
        {
            block = (uint)((block + subKey) % Math.Pow(2, 32));
            block = Substitute(block);
            block = (block << 11) | (block >> 21);
            return block;
        }

        readonly static byte[,] s =
        {
            { 4, 10, 9, 2, 13, 8, 0, 14, 6, 11, 1, 12, 7, 15, 5, 3 },
            { 14, 11, 4, 12, 6, 13, 15, 10, 2, 3, 8, 1, 0, 7, 5, 9 },
            { 5, 8, 1, 13, 10, 3, 4, 2, 14, 15, 12, 7, 6, 0, 9, 11 },
            { 7, 13, 10, 1, 0, 8, 9, 15, 14, 4, 6, 12, 11, 2, 5, 3 },
            { 6, 12, 7, 1, 5, 15, 13, 8, 4, 10, 9, 14, 0, 3, 11, 2 },
            { 4, 11, 10, 0, 7, 2, 1, 13, 3, 6, 8, 5, 9, 12, 15, 14 },
            { 13, 11, 4, 1, 3, 15, 5, 9, 0, 10, 14, 7, 6, 8, 2, 12 },
            { 1, 15, 13, 0, 5, 7, 10, 4, 9, 2, 3, 14, 6, 11, 8, 12 }
        };

        
    }
}
