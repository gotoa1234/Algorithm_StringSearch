namespace Algorithm_StringSearch.StringSearch
{
    public class BoyerMooreAlgorithmExecute
    {
        public void Execute()
        {
            string text = "HERE IS A SIMPLE EXAMPLE";
            string pattern = "EXAMPLE";
            Console.WriteLine($@"原始文本：{text}");
            Console.WriteLine($@"搜尋字串：{pattern}");
            new BoyerMooreAlgorithm.BadCharacterExample(text, pattern);
            new BoyerMooreAlgorithm.GoodSuffixExample(text, pattern);
            new BoyerMooreAlgorithm.BoyerMooreExample(text, pattern);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BoyerMooreAlgorithm
    {        
        public BoyerMooreAlgorithm()
        {
        }

        /// <summary>
        /// 1. 第一種方式：壞字符表
        /// </summary>
        public class BadCharacterExample
        {
            public BadCharacterExample(string text, string pattern)
            {
                Console.WriteLine("1. 第一種方式：壞字符表");
                Search(text, pattern);
                Console.WriteLine();
                Console.WriteLine();
            }

            /// <summary>
            /// 1-3. 建立壞字元表
            /// </summary>            
            private Dictionary<char, int> BadCharHeuristic(string pattern)
            {
                var result= new Dictionary<char, int>();
                for (int index = 0; index < pattern.Length; index++)
                    result[pattern[index]] = index;
                return result;
            }

            /// <summary>
            /// 1-2. 執行演算法 - 搜尋指定字串
            /// </summary>           
            private void Search(string text, string pattern)
            {
                int m = pattern.Length;
                int n = text.Length;

                //1-3. 找出壞字元表
                var badchar = BadCharHeuristic(pattern);

                //1-4. 開始搜尋
                int currentIndex = 0;// 起始位置
                int moveIndex = 0;
                while (currentIndex <= (n - m))// 不可超過最大長度
                {
                    //1-5. 從後往前比對
                    moveIndex = m - 1;

                    // 1-6. 若有不同，則移動到壞字元表的位置
                    while (moveIndex >= 0 && pattern[moveIndex] == text[currentIndex + moveIndex])
                        moveIndex--;

                    if (moveIndex == -1)// 1-7. 若比對到最後，則表示找到
                    {
                        Console.WriteLine("Pattern occurs at index " + currentIndex);
                        currentIndex += (currentIndex + m < n) ? m - badchar.GetValueOrDefault(text[currentIndex + m], -1) : 1;
                    }
                    else// 1-8. 若有不同，則移動到壞字元表的位置
                    {
                        currentIndex += Math.Max(1, moveIndex - badchar.GetValueOrDefault(text[currentIndex + moveIndex], -1));
                    }
                }
            }
        }

        /// <summary>
        /// 2. 第二種方式：好後綴表
        /// </summary>
        public class GoodSuffixExample
        {
            public GoodSuffixExample(string text, string pattern)
            {
                Console.WriteLine("2. 第二種方式：好後綴表");
                Search(text, pattern);
                Console.WriteLine();
                Console.WriteLine();
            }

            /// <summary>
            /// 2-2-2.  當模式串中部分後綴匹配文本串時的情況。這有助於在失敗匹配時確定模式串應該向前移動多少
            /// </summary>
            private void PreprocessStrongSuffix(int[] shift, int[] bpos, string patten)
            {
                int pattenLength = patten.Length;
                int index = pattenLength;
                int currentIndex = pattenLength + 1;// 索引最右邊開始
                bpos[index] = currentIndex;
                while (index > 0)
                {
                    while (currentIndex <= pattenLength && 
                           patten[index - 1] != patten[currentIndex - 1])
                    {
                        if (shift[currentIndex] == 0)
                            shift[currentIndex] = currentIndex - index;
                        currentIndex = bpos[currentIndex];
                    }
                    index--; currentIndex--;
                    bpos[index] = currentIndex;
                }
            }

            /// <summary>
            /// 2-3-2. 計算出可跳過的位置數
            /// </summary>
            private void PreprocessCase2(int[] shift, int[] bpos, int m)
            {
                int index, moveIndex;
                moveIndex = bpos[0];
                for (index = 0; index <= m; index++)
                {
                    if (shift[index] == 0)
                        shift[index] = moveIndex;
                    if (index == moveIndex)
                        moveIndex = bpos[moveIndex];
                }
            }

            /// <summary>
            /// 2-1. 執行演算法 - 搜尋指定字串
            /// </summary>
            private void Search(string text, string pattern)
            {
                int currentIndex = 0, moveIndex = 0;
                int m = pattern.Length;
                int n = text.Length;

                int[] bpos = new int[m + 1];//用於記錄模式串中每個位置的下一個可能的後綴位置
                int[] shift = new int[m + 1];//用於記錄強後綴匹配失敗時應該跳過的位置數量

                // 2-2-1. 紀錄每個位置的下一個可能的後綴位置
                PreprocessStrongSuffix(shift, bpos, pattern);
                // 2-3-1. 計算出可跳過的位置數
                PreprocessCase2(shift, bpos, m);

                while (currentIndex <= n - m)
                {
                    // 2-4. 從後往前比對
                    moveIndex = m - 1;
                    
                    // 2-5. 逐步匹配
                    while (moveIndex >= 0 && pattern[moveIndex] == text[currentIndex + moveIndex])
                        moveIndex--;

                    // 2-6. 找到時紀錄
                    if (moveIndex == -1)
                    {
                        Console.WriteLine("Pattern occurs at index " + currentIndex);
                        currentIndex += shift[0];
                    }
                    else// 2-7. 未找到時使用好後綴表跳過對應位置數
                        currentIndex += shift[moveIndex + 1];
                }
            }
        }

        /// <summary>
        /// 3. 第三種方式：混合法 (完整最基本版本的 Boyer-Moore)
        /// </summary>
        public class BoyerMooreExample
        {
            public BoyerMooreExample(string text, string pattern)
            {
                Console.WriteLine("2. 第三種方式：混合法 (完整最基本版本的 Boyer-Moore)");
                Search(text, pattern);
                Console.WriteLine();
                Console.WriteLine();
            }

            private Dictionary<char, int> BadCharHeuristic(string pattern)
            {
                var result = new Dictionary<char, int>();
                for (int index = 0; index < pattern.Length; index++)
                    result[pattern[index]] = index;
                return result;
            }

            private void PreprocessStrongSuffix(int[] shift, int[] bpos, string pat, int m)
            {
                int i = m, j = m + 1;
                bpos[i] = j;
                while (i > 0)
                {
                    while (j <= m && pat[i - 1] != pat[j - 1])
                    {
                        if (shift[j] == 0)
                            shift[j] = j - i;
                        j = bpos[j];
                    }
                    i--; j--;
                    bpos[i] = j;
                }
            }

            private void PreprocessCase2(int[] shift, int[] bpos, int m)
            {
                int i, j;
                j = bpos[0];
                for (i = 0; i <= m; i++)
                {
                    if (shift[i] == 0)
                        shift[i] = j;
                    if (i == j)
                        j = bpos[j];
                }
            }

            public void Search(string text, string pattern)
            {
                int m = pattern.Length;
                int n = text.Length;
                
                int[] bpos = new int[m + 1];
                int[] shift = new int[m + 1];

                for (int i = 0; i < m + 1; i++) shift[i] = 0;

                var badchar = BadCharHeuristic(pattern);
                PreprocessStrongSuffix(shift, bpos, pattern, m);
                PreprocessCase2(shift, bpos, m);

                int s = 0;
                while (s <= (n - m))
                {
                    int j = m - 1;

                    while (j >= 0 && pattern[j] == text[s + j])
                        j--;

                    if (j < 0)
                    {
                        Console.WriteLine("Pattern occurs at index " + s);
                        s += shift[0];
                    }
                    else
                        s += Math.Max(shift[j + 1], j - badchar.GetValueOrDefault(text[s + j], -1));
                }
            }
        }
    }
}
