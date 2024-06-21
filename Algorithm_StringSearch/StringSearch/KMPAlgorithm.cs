namespace Algorithm_StringSearch.StringSearch
{
    public class KMPAlgorithmExecute
    {
        public void Execute()
        {
            var method = new KMPAlgorithm();
            string target = "inxinyiz inxinyin?";
            string pattern = "inxinyinxiny";
            List<int> matchPositions = method.KMPSearch(target, pattern);

            Console.WriteLine("匹配位置:");
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
        /// 計算部分匹配表
        /// </summary>       
        public int[] ComputePrefixFunction(string pattern)
        {            
            var matchTable = new int[pattern.Length];
            int commonIndex = 0;// 最常公共前綴後綴長度

            // 跳過自己跟自己比對，因此從第二個字符 (serachIndex = 1) 開始計算
            for (int serachIndex = 1; serachIndex < pattern.Length; serachIndex++)
            {
                while (commonIndex > 0 && 
                       pattern[serachIndex] != pattern[commonIndex])
                {
                    commonIndex = matchTable[commonIndex - 1];
                }
                if (pattern[serachIndex] == pattern[commonIndex])
                {
                    commonIndex++;
                }
                matchTable[serachIndex] = commonIndex;
            }
            return matchTable;
        }

        /// <summary>
        /// KMP - 字串搜尋
        /// </summary>                
        public List<int> KMPSearch(string targetStr, string patternStr)
        {
            int tragetLength = targetStr.Length;
            int patternLength = patternStr.Length;
            var matchTable = ComputePrefixFunction(patternStr);
            int currentIndex = 0;// 當前匹配的位置
            var matchPositions = new List<int>();

            for (int index = 0; index < tragetLength; index++)
            {
                while (currentIndex > 0 && 
                       targetStr[index] != patternStr[currentIndex])
                {
                    currentIndex = matchTable[currentIndex - 1];
                }
                if (targetStr[index] == patternStr[currentIndex])
                {
                    currentIndex++;
                }
                if (currentIndex == patternLength)
                {
                    matchPositions.Add(index - patternLength + 1);  // 記錄匹配位置
                    currentIndex = matchTable[currentIndex - 1];
                }
            }

            return matchPositions;
        }
    }



}
