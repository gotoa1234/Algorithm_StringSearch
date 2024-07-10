namespace Algorithm_StringSearch.StringSearch
{
    public class FiniteStateMachineAlgorithmExecute
    {
        public void Execute()
        {

            //Example 1 :
            string text = "HERE IS A SIMPLE EXAMPLE ";
            string pattern = "EXAMPLE";
            new FiniteStateMachineAlgorithm.FiniteStateMachineOrigineArrayExample(text, pattern);
            new FiniteStateMachineAlgorithm.FiniteStateMachineHashExample(text, pattern);

            //Example 2 :
            text = "我的世世世界 ";
            pattern = "世世界";
            //new FiniteStateMachineAlgorithm.FiniteStateMachineOrigineArrayExample(text, pattern);//不支援中文
            new FiniteStateMachineAlgorithm.FiniteStateMachineHashExample(text, pattern);

            //Example 3 :
            text = "AAACB ";
            pattern = "AAB";
            new FiniteStateMachineAlgorithm.FiniteStateMachineOrigineArrayExample(text, pattern);
            new FiniteStateMachineAlgorithm.FiniteStateMachineHashExample(text, pattern);

            //Example 4 :
            text = "AAAB ";
            pattern = "AAB";
            new FiniteStateMachineAlgorithm.FiniteStateMachineOrigineArrayExample(text, pattern);
            new FiniteStateMachineAlgorithm.FiniteStateMachineHashExample(text, pattern);
        }
    }

    public class FiniteStateMachineAlgorithm
    {
        /// <summary>
        /// 傳統方法：用陣列做狀態轉移表
        /// 空間複雜度：O(m * |Σ|) (m: 模式長度, |Σ|: 字元集大小)
        /// </summary>
        public class FiniteStateMachineOrigineArrayExample
        {
            public FiniteStateMachineOrigineArrayExample(string text, string pattern)
            {
                Console.WriteLine();
                Console.WriteLine(" 1. 陣列 做狀態轉移表 ");
                Console.WriteLine($@"文本：{text}");
                Console.WriteLine($@"查找：{pattern}");
                var patternFound = this.FiniteStateMachineAlgoritSearch(text, pattern);
                if (patternFound)
                {
                    Console.WriteLine("文本中[找到了]模式！");
                }
                else
                {
                    Console.WriteLine("文本中[沒有找到]模式。");
                }
            }

            /// <summary>
            /// 3. 建立 Array[] 的狀態移轉表
            /// </summary>        
            public int[,] InitialPatternPreProcess(string pattern)
            {
                int statesCount = pattern.Length + 1;//狀態數量 = 模式長度 + 1
                int alphabetSize = 256; // 假設現在比對為 Ascii 碼

                // 3-1. 初始化狀態轉移表
                var transitionTable = new int[statesCount, alphabetSize];

                // 3-2. 賦值狀態轉移資料
                for (int state = 0; state < pattern.Length; state++)
                {
                    // 3-3. 依照ASCII 碼，將字元對應到狀態
                    for (char c = (char)0; c < alphabetSize; c++)
                    {
                        var getState = GetNextState(pattern, state, c);
                        transitionTable[state, c] = getState;
                    }                    
                }
                return transitionTable;
            }

            /// <summary>
            /// 3-3. 依照ASCII 碼，將字元對應到狀態
            /// </summary>                        
            private int GetNextState(string pattern, int state, char c)
            {
                // 完全匹配 - 當前 pattern 字元與 ASCII 碼相同
                if (state < pattern.Length && c == pattern[state])
                {
                    return state + 1;
                }

                // 部分匹配
                for (int ns = state; ns > 0; ns--)
                {
                    // 找到最長的部分匹配
                    if (pattern[ns - 1] == c)
                    {
                        // 檢查是否為部分匹配
                        // pattern[0] 開始到 pattern[ns - 1] 為部分匹配
                        for (int index = 0; index < ns - 1; index++)
                        {
                            if (pattern[index] != pattern[state - ns + 1 + index])
                            {
                                return 0;
                            }
                        }
                        return ns;                        
                    }
                }

                return 0;
            }

            /// <summary>
            /// 1. 執行演算法 - 搜尋字串
            /// </summary>        
            public bool FiniteStateMachineAlgoritSearch(string text, string pattern)
            {
                // 2. 初始化狀態轉移表
                var transitionTable = this.InitialPatternPreProcess(pattern);

                // 4. 搜尋字串
                var currentState = 0;
                for (int index = 0; index < text.Length; index++)
                {
                    currentState = transitionTable[currentState, text[index]];

                    // 4-1. 找到匹配 - 當前狀態達到最終狀態（模式長度）
                    if (currentState == pattern.Length)
                    {
                        return true;
                    }
                }

                // 4-2. 沒找到 - 當前狀態未達到最終狀態
                return false;
            }
        }

        /// <summary>
        /// 優化方法：用 Hash 做狀態轉移表 - 支援中文匹配
        /// 空間複雜度：O(m) (m: 模式長度)
        /// </summary>
        public class FiniteStateMachineHashExample
        {
            public FiniteStateMachineHashExample(string text, string pattern)
            {
                Console.WriteLine();
                Console.WriteLine(" 2. Hash 做狀態轉移表 ");
                Console.WriteLine($@"文本：{text}");
                Console.WriteLine($@"查找：{pattern}");
                var patternFound = this.FiniteStateMachineAlgoritSearch(text, pattern);
                if (patternFound)
                {
                    Console.WriteLine("文本中[找到了]模式！");
                }
                else
                {
                    Console.WriteLine("文本中[沒有找到]模式。");
                }
            }

            /// <summary>
            /// 3. 建立 Hash 的狀態移轉表
            /// </summary>        
            public Dictionary<(int, int), int> InitialPatternPreProcess(string pattern)
            {
                var transitionTable = new Dictionary<(int, int), int>();
                // 3-1. 為完全匹配的字符建立狀態轉移
                for (int state = 0; state < pattern.Length; state++)
                {
                    transitionTable[(state, pattern[state])] = state + 1;
                }
                return transitionTable;
            }

            /// <summary>
            /// 4-2. 取得部分匹配狀態
            /// </summary>            
            private int GetPartialState(string pattern, int state, char c)
            {
                // 尋找匹配的最長前綴
                for (int ns = state; ns > 0; ns--)
                {
                    if (pattern[ns - 1] == c && 
                        pattern.Substring(0, ns - 1) == pattern.Substring(state - ns + 1, ns - 1))
                    {
                        return ns;
                    }
                }
                return 0;
            }
            
            /// <summary>
            /// 1. 執行演算法 - 搜尋字串
            /// </summary>        
            public bool FiniteStateMachineAlgoritSearch(string text, string pattern)
            {
                // 2. 初始化狀態轉移表
                var transitionTable = this.InitialPatternPreProcess(pattern);

                // 4. 搜尋字串
                var currentState = 0;
                for (int index = 0; index < text.Length; index++)
                {
                    char currentChar = text[index];

                    // 4-1. 狀態轉移表中沒有對應的狀態，取得部分匹配狀態
                    if (!transitionTable.ContainsKey((currentState, currentChar)))
                    {
                        int partialState = GetPartialState(pattern, currentState, currentChar);
                        transitionTable[(currentState, currentChar)] = partialState;
                    }

                    currentState = transitionTable[(currentState, currentChar)];
                    
                    // 5-1. 找到匹配 - 當前狀態達到最終狀態（模式長度）
                    if (currentState == pattern.Length)
                    {
                        return true;
                    }
                }
                // 5-2. 沒找到 - 當前狀態未達到最終狀態
                return false;
            }
        }
    }
}

