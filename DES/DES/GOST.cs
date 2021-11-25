using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES
{
    public class GOST
    {
        const int BLOCK_SIZE_BYTES = 8;

        readonly byte[,] s =
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

        private uint[] key = new uint[8];



        public GOST(uint[] key)
        {
            this.key = key;
        }

        private uint Substitute(uint value)
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

        private uint F(uint block, uint subKey)
        {
            block = (uint)((block + subKey) % Math.Pow(2,32));   
            block = Substitute(block);
            block = (block << 11) | (block >> 21);  
            return block;
        }

        public byte[] Encrypt(byte[] inputBlock)
        {
            uint left = BitConverter.ToUInt32(inputBlock, 0);
            uint right = BitConverter.ToUInt32(inputBlock, 4);
            byte[] result = new byte[8];

            for (int i = 0; i < 24; i++)
            {
                uint fResult = left ^ F(right, key[i % 8]);
                left = right;
                right = fResult;
            }

            for (int i = 24, j = 7; i < 31; i++, j--)
            {
                uint fResult = left ^ F(right, key[j]);
                left = right;
                right = fResult;
            }
            left ^= F(right, key[0]);

            Array.Copy(BitConverter.GetBytes(left), 0, result, 0, 4);
            Array.Copy(BitConverter.GetBytes(right), 0, result, 4, 4);
            return result;
        }

        public byte[] Decrypt(byte[] inputBlock)
        {
            uint left = BitConverter.ToUInt32(inputBlock, 0);
            uint right = BitConverter.ToUInt32(inputBlock, 4);
            byte[] result = new byte[BLOCK_SIZE_BYTES];
            for (int i = 31, j = 0; i >= 24; i--, j++)
            {
                uint fResult = left ^ F(right, key[j]);
                left = right;
                right = fResult;
            }
            for (int i = 23; i > 0; i--)
            {
                uint fResult = left ^ F(right, key[i % 8]);
                left = right;
                right = fResult;
            }
            left ^= F(right, key[0]);
            Array.Copy(BitConverter.GetBytes(left), 0, result, 0, 4);
            Array.Copy(BitConverter.GetBytes(right), 0, result, 4, 4);
            return result;
        }

    }
}
