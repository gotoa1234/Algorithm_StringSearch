namespace Algorithm_StringSearch.StringSearch
{
    public class KMPAlgorithmExecute
    {
        public void Execute()
        {
            var method = new KMPAlgorithm();
            string target = "How did you get into software engineering?";
            string pattern = "software";
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
            int innerIndex = 0;// 最常公共前綴後綴長度

            for (int index = 1; index < pattern.Length; index++)
            {
                while (innerIndex > 0 && 
                       pattern[index] != pattern[innerIndex])
                {
                    innerIndex = matchTable[innerIndex - 1];
                }
                if (pattern[index] == pattern[innerIndex])
                {
                    innerIndex++;
                }
                matchTable[index] = innerIndex;
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
