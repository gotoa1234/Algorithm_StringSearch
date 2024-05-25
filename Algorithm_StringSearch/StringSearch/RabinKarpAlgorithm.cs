using static Algorithm_StringSearch.StringSearch.RabinKarpAlgorithm;

namespace Algorithm_StringSearch.StringSearch
{
    public class RabinKarpAlgorithmExecute
    {
        public void Execute()
        {
            // ASCII 中 A=65, F=70, K=75
            string text = "AAAFFFKKK";
            string pattern = "AAF";
            new Method_RollingHash(text, pattern);
            new Method_RabinFingerprint(text, pattern);
        }
    }


    public class RabinKarpAlgorithm
    {
        public RabinKarpAlgorithm()
        {
        }

        /// <summary>
        /// 第一種：旋轉哈希的實現
        /// </summary>
        public class Method_RollingHash
        {
            /* 演算:
             * Step1 : 得到 pattern 65+70+70 = 205 的 Hash 值 (AFF)， 並且得到 text 三個字元 65+65+65 = 195 的 Hash 值 (AAA)
               Step2 : 進行滑動窗口處理，如果 Hash 相等，則進一步比較，如果不相等，則更新 Hash 值
               Step3 : 更新 text Hash值 ，從 195 變成 65+65+70 = 200 (AAA -> AAF)
               Step4 : 進行滑動窗口處理，如果 Hash 相等，則進一步比較，如果不相等，則更新 Hash 值
               Step5 : 更新 text Hash值 ，從 200 變成 65+70+70 = 205 (AAF -> AFF)
               Step6 : 進行滑動窗口處理，此時 Hash 相等，進一步比較，完全匹配，返回索引
             */

            /// <summary>
            /// 建構式
            /// </summary>
            /// <param name="text">原始資料</param>
            /// <param name="pattern">查詢的字串</param>
            public Method_RollingHash(string text, string pattern)
            {
                int result = RabinKarpSearch(text, pattern);

                if (result != -1)
                {
                    Console.WriteLine($"Pattern found at index {result}");
                }
                else
                {
                    Console.WriteLine("Pattern not found");
                }
            }

            /// <summary>
            /// 拉賓卡普搜索算法 - 旋轉哈希 (Rabin-Karp Algorithm)
            /// </summary>
            /// <param name="text">原始資料</param>
            /// <param name="pattern">查詢的字串</param>
            /// <returns></returns>
            private int RabinKarpSearch(string text, string pattern)
            {
                // 1. 預處理：計算文字和查詢對象的 Hash
                int textLength = text.Length;
                int targetLength = pattern.Length;
                int patternHash = CalculateRollingHash(pattern, 0, targetLength);
                int textHash = CalculateRollingHash(text, 0, targetLength);

                // 2. 滑動窗口處理 ※最多滑動 textLength - targetLength 次
                for (int index = 0; index <= textLength - targetLength; index++)
                {
                    // 3-1. 匹配，如果 Hash 相等，則進一步比較
                    if (patternHash == textHash)
                    {
                        int findIndex = 0;
                        // 3-2. 進一步比較，比對字元是否相等
                        // ※旋轉哈希有可能碰撞，所以需要進一步比對 EX: ABCD 與 DCBA 相同 Hash
                        for (; findIndex < targetLength; findIndex++)
                        {
                            if (text[index + findIndex] != pattern[findIndex])
                                break;
                        }
                        // 4. 如果完全匹配，返回索引
                        if (findIndex == targetLength)
                            return index;
                    }
                    // 3-3. 不匹配，更新 Hash 值
                    if (index < textLength - targetLength)
                    {
                        textHash = UpdateRollingHash(textHash, text[index], text[index + targetLength]);
                    }
                }
                // 5. 沒找到，返回 -1
                return -1;
            }

            /// <summary>
            /// 計算 Hash 值 (累加字符的 ASCII 值)
            /// </summary>
            /// <param name="str">傳入的字串</param>
            /// <param name="start">起始索引</param>
            /// <param name="length">總長度</param>
            /// <returns></returns>
            private int CalculateRollingHash(string str, int start, int length)
            {
                int hash = 0;
                for (int index = start; index < start + length; index++)
                {
                    hash += str[index]; 
                }
                return hash;
            }

            /// <summary>
            /// 沒找到時進入 - 更新旋轉 Hash 值 
            /// <para>※只有一個字元的變動</para>
            /// </summary>    
            /// <param name="oldHash">當前舊有的 Hash 值</param>
            /// <param name="oldChar">舊的單一字元</param>
            /// <param name="newChar">新的單一字元</param>
            /// <returns></returns>
            private int UpdateRollingHash(int oldHash, char oldChar, char newChar)
            {
                return oldHash - oldChar + newChar;
            }
        }

        /// <summary>
        /// 第二種：拉賓指紋的實現
        /// </summary>
        public class Method_RabinFingerprint
        {
            /* 演算:
 * Step1 : 得到 pattern = 76 的 Hash 值 (AFF)， 並且得到 text 三個字元 = 3的 Hash 值 (AAA)
   Step2 : 進行滑動窗口處理，如果 Hash 相等，則進一步比較，如果不相等，則更新 Hash 值
   Step3 : 更新 text Hash值 ，從 3 變成 8 (AAA -> AAF)
   Step4 : 進行滑動窗口處理，如果 Hash 相等，則進一步比較，如果不相等，則更新 Hash 值
   Step5 : 更新 text Hash值 ，從 8 變成 76 (AAF -> AFF)
   Step6 : 進行滑動窗口處理，此時 Hash 相等，進一步比較，完全匹配，返回索引
 */

            /// <summary>
            /// 建構式
            /// </summary>
            /// <param name="text">原始資料</param>
            /// <param name="pattern">查詢的字串</param>
            public Method_RabinFingerprint(string text, string pattern)
            {
                // 使用質數 prime 進行模運算
                int prime = 101; 

                int result = RabinKarpSearch(text, pattern, prime);

                if (result != -1)
                {
                    Console.WriteLine($"Pattern found at index {result}");
                }
                else
                {
                    Console.WriteLine("Pattern not found");
                }
            }

            /// <summary>
            /// 計算拉賓指紋
            /// </summary>
            /// <param name="str">傳入的字串</param>
            /// <param name="start">起始索引</param>
            /// <param name="length">總長度</param>
            /// <param name="prime">質數</param>
            /// <return></return>
            private int CalculateRabinFingerprint(string str, int start, int length, int prime)
            {
                int hash = 0;
                for (int index = start; index < start + length; index++)
                {
                    // 使用合適的質數取餘數
                    hash = (hash * 256 + str[index]) % prime; 
                }
                return hash;
            }

            /// <summary>
            /// 拉賓卡普搜索算法 - 拉賓指紋 (Rabin-Karp Algorithm)
            /// </summary>
            /// <param name="text">原始資料</param>
            /// <param name="pattern">查詢的字串</param>
            /// <param name="prime">使用自定義的質數</param>
            /// <returns></returns>
            private int RabinKarpSearch(string text, string pattern, int prime)
            {
                // 1. 預處理：計算文字和查詢對象的 Hash
                int textLength = text.Length;
                int targetLength = pattern.Length;
                int patternHash = CalculateRabinFingerprint(pattern, 0, targetLength, prime);
                int textHash = CalculateRabinFingerprint(text, 0, targetLength, prime);

                // 2. 滑動窗口處理 ※最多滑動 textLength - patternLength 次
                for (int index = 0; index <= textLength - targetLength; index++)
                {
                    // 3-1. 匹配，如果 Hash 相等，則進一步比較
                    if (patternHash == textHash)
                    {
                        int findeIndex;
                        // 3-2. 進一步比較，比對字元是否相等
                        for (findeIndex = 0; findeIndex < targetLength; findeIndex++)
                        {
                            if (text[index + findeIndex] != pattern[findeIndex])
                                break;
                        }
                        if (findeIndex == targetLength)
                            return index;
                    }
                    // 3-3. 不匹配，更新 Hash 值
                    if (index < textLength - targetLength)
                    {
                        // 3-4. 更新文本窗口的哈希值
                        // ※ 舊Hash * 基數(這裡256) + 新字符 - 舊字符 * [基數(256)的目標字串長度平方]
                        // ※ Base 一定要選擇當前字符串大的值，例如 A-Z 共 26個字母，建議32或64，避免 Hash 碰撞
                        // ※ 此外 Base 愈小效能愈佳，但以現代電腦來說，差異不大
                        textHash = (textHash * 256 + 
                                    text[index + targetLength] - 
                                    text[index] * (int)Math.Pow(256, targetLength)
                                   ) % prime;
                        // 3-5. 負數轉正
                        if (textHash < 0)
                            textHash = (textHash + prime);
                    }
                }
                // 5. 沒找到，返回 -1
                return -1;
            }
        }
    }
}
