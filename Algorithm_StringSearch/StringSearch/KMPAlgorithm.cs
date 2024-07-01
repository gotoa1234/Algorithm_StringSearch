namespace Algorithm_StringSearch.StringSearch
{
    public class KMPAlgorithmExecute
    {
        /// <summary>
        /// KMP 字串搜尋演算法 - 進入點
        /// </summary>
        public void Execute()
        {
            var method = new KMPAlgorithm();
            string target1 = "CECDCEDCCDCECDCCDC";
            string target2 = "AACAADAACDCECDCECDCACDC";
            string pattern = "CDCECDC";
            List<int> matchPositions = method.KMPSearch(target1, pattern);
            Console.WriteLine($"文本：{target1} 匹配位置:");            
            foreach (int pos in matchPositions)
            {
                Console.WriteLine(pos);
            }
            Console.WriteLine("");
            Console.WriteLine($"文本：{target2} 匹配位置:");
            matchPositions = method.KMPSearch(target2, pattern);
            foreach (int pos in matchPositions)
            {
                Console.WriteLine(pos);
            }
        }
    }

    public class KMPAlgorithm
    {
        public KMPAlgorithm()
        {
                
        }

        /// <summary>
        /// 2. 計算部分匹配表 (正式名稱：失配函式)
        /// </summary>       
        public int[] ComputePrefixFunction(string pattern)
        {            
            var matchTable = new int[pattern.Length];
            int commonIndex = 0;// 最常公共前綴後綴長度

            // 2-1. 跳過自己跟自己比對，因此從第二個字符 (serachIndex = 1) 開始計算
            for (int serachIndex = 1; serachIndex < pattern.Length; serachIndex++)
            { 
                // 2-2. 檢查這次是否有匹配，若沒有匹配，則將 commonIndex 當前公共前綴索引倒退為前一次查找相同字符結果
                //      (若都無最後 commonIndex 會為 0)
                while (commonIndex > 0 && 
                       pattern[serachIndex] != pattern[commonIndex])
                {
                    commonIndex = matchTable[commonIndex - 1];
                }
                // 2-3. 本次若匹配到，累計最常公共前綴
                if (pattern[serachIndex] == pattern[commonIndex])
                {
                    commonIndex++;
                }
                // 2-4. 每次都將匹配結果紀錄
                matchTable[serachIndex] = commonIndex;
            }
            return matchTable;
        }

        /// <summary>
        /// KMP - 字串搜尋
        /// </summary>                
        public List<int> KMPSearch(string targetStr, string patternStr)
        {
            // 1. 建構變數
            int tragetLength = targetStr.Length;
            int patternLength = patternStr.Length;
            var matchTable = ComputePrefixFunction(patternStr);
            int currentIndex = 0;// 當前匹配的位置
            var resultMatch = new List<int>();

            // 3. PMT 用 Match表找出文本(targetStr) 的比對資料
            for (int index = 0; index < tragetLength; index++)
            {
                // 3-1. 比對字串這次是否有匹配，若沒有匹配，則將 Match表內公共前綴索引倒退為前一次查找相同字符結果
                while (currentIndex > 0 && 
                       targetStr[index] != patternStr[currentIndex])
                {
                    currentIndex = matchTable[currentIndex - 1];
                }

                // 3-2. 比對字串字符相同，繼續往前比對
                if (targetStr[index] == patternStr[currentIndex])
                {
                    currentIndex++;
                }

                // 3-3. 若當前查找全部匹配，則記錄匹配位置
                if (currentIndex == patternLength)
                {
                    // 此段為記錄索引，因此相減後+1
                    resultMatch.Add(index - patternLength + 1);
                    // 因為比對完成，強制將查找索引，往 Match 表前一個最常公共前綴查找
                    currentIndex = matchTable[currentIndex - 1];
                }
            }
            return resultMatch;
        }
    }
}