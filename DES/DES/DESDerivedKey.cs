using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DES
{
    public class DESDerivedKey
    {
        public List<string> rkb;
        public DESDerivedKey(string key)
        {
            int[] keyp = { 57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36, 63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4 };
            int[] shift_table = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
            int[] key_comp = { 14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4, 26, 8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32 };
            
            key = Utils.Permute(key, keyp, 56);
            string left = key.Substring(0, 28);
            string right = key.Substring(28);

            List<string> rkb = new List<string>();
            for(int i = 0; i < 16; i++)
            {
                left = ShiftLeft(left, shift_table[i]);
                right = ShiftLeft(right, shift_table[i]);
                string combine = string.Concat(left, right);
                string roundKey = Utils.Permute(combine, key_comp, 48);
                rkb.Add(roundKey);
            }

            this.rkb = rkb;
        }

        private string ShiftLeft(string ks,int shifts)
        {
            StringBuilder k = new StringBuilder(ks);
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < shifts; i++)
            {
                for (int j = 1; j < 28; j++)
                {
                    s.Append(k[j]);
                }
                s.Append(k[0]);
                k = new StringBuilder(s.ToString());
            }
            return k.ToString();
        }

        public void Reverse()
        {
            rkb.Reverse();
        }
    }
}
