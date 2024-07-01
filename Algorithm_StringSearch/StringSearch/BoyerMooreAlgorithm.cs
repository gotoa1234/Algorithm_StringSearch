namespace Algorithm_StringSearch.StringSearch
{
    public class BoyerMooreAlgorithmExecute
    {
        public void Execute()
        {
            var method = new BoyerMooreAlgorithm();
            string target = "abacaabaccabacabacab";
            string pattern = "abacab";
            
            Console.WriteLine("匹配位置:");
            //method.BoyerMooreSearch(target, pattern);
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

        /// <summary>
        /// 1. 第一種方式：壞字符表
        /// </summary>
        public class BadCharacterExample
        {
            private int[] badCharacterTable;
            private readonly int ALPHABET_SIZE = 256;
            private string pattern;
            public BadCharacterExample(string text, string pattern)
            {
                this.pattern = pattern;
                badCharacterTable = BuildBadCharacterTable(pattern);
            }

            private int[] BuildBadCharacterTable(string pattern)
            {
                int m = pattern.Length;
                int[] table = new int[ALPHABET_SIZE];

                for (int i = 0; i < ALPHABET_SIZE; i++)
                    table[i] = -1;

                for (int i = 0; i < m; i++)
                    table[(int)pattern[i]] = i;

                return table;
            }

            public int Search(string text)
            {
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
                        return s; // Pattern found at index s
                        s += (s + m < n) ? m - badCharacterTable[text[s + m]] : 1;
                    }
                    else
                    {
                        s += Math.Max(1, j - badCharacterTable[text[s + j]]);
                    }
                }

                return -1; // Pattern not found
            }

            public void PrintBadCharacterTable()
            {
                for (int i = 0; i < ALPHABET_SIZE; i++)
                {
                    if (badCharacterTable[i] != -1)
                        Console.WriteLine($"Char {((char)i)}: {badCharacterTable[i]}");
                }
            }
        }

        /// <summary>
        /// 2. 第二種方式：好後綴表
        /// </summary>
        public class GoodSuffixExample
        {

        }

        /// <summary>
        /// 3. 第三種方式：混合法
        /// </summary>
        public class BoyerMooreExample
        {

        }
    }
}
