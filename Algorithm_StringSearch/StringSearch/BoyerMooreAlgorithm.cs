using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_StringSearch.StringSearch
{
    public class BoyerMooreAlgorithmExecute
    {
        public void Execute()
        {
            var method = new BoyerMooreAlgorithm();
            string target = "abacaabaccabacabacab";
            string pattern = "abacab";
            List<int> matchPositions = new List<int>();//method.BoyerMooreSearch(target, pattern);

            Console.WriteLine("匹配位置:");
            foreach (int pos in matchPositions)
            {
                Console.WriteLine(pos);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BoyerMooreAlgorithm
    {
        private readonly int ALPHABET_SIZE = 256;
        
        public BoyerMooreAlgorithm()
        {
            
        }

        private int[] BuildBadCharacterTable(string pattern)
        {
            int m = pattern.Length;
            int[] table = new int[ALPHABET_SIZE];

            for (int i = 0; i < ALPHABET_SIZE; i++)
                table[i] = -1;

            for (int i = 0; i < m - 1; i++)
                table[(int)pattern[i]] = i;

            return table;
        }

        private int[] BuildGoodSuffixTable(string pattern)
        {
            int m = pattern.Length;
            int[] table = new int[m];
            int[] borderPosition = new int[m + 1];
            int i = m, j = m + 1;
            borderPosition[i] = j;

            while (i > 0)
            {
                while (j <= m && pattern[i - 1] != pattern[j - 1])
                {
                    if (table[j] == 0) table[j] = j - i;
                    j = borderPosition[j];
                }
                i--;
                j--;
                borderPosition[i] = j;
            }

            j = borderPosition[0];
            for (i = 0; i <= m; i++)
            {
                if (table[i] == 0) table[i] = j;
                if (i == j) j = borderPosition[j];
            }

            return table;
        }

        public int BoyerMooreSearch(string text, string pattern)
        {
             int[] badCharacterTable = new int[200];
             int[] goodSuffixTable;

            int n = text.Length;
            int m = pattern.Length;
            int s = 0; // s is the shift of the pattern with respect to text

            while (s <= (n - m))
            {
                int j = m - 1;

                while (j >= 0 && pattern[j] == text[s + j])
                    j--;

                if (j < 0)
                {
                    return s;
                    s += (s + m < n) ? m - badCharacterTable[text[s + m]] : 1;
                }
                else
                {
                    s += Math.Max(1, j - badCharacterTable[text[s + j]]);
                }
            }

            return -1;
        }
    }
}
