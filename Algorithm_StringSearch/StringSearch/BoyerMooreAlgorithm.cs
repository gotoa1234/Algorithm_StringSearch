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
            method.BoyerMooreSearch(target, pattern);
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

        public void BadCharHeuristic(string pattern, int[] badChar)
        {
            int strLength = pattern.Length;
            for (int index = 0; index < 256; index++)
                badChar[index] = -1;

            for (int index = 0; index < strLength; index++)
                badChar[(int)pattern[index]] = index;
        }

        public void BoyerMooreSearch(string text, string pattern)
        {
            int searchLength = pattern.Length;
            int textLength = text.Length;

            int[] badChar = new int[256];
            BadCharHeuristic(pattern, badChar);

            int moveIndex = 0; // text 中的起始位置

            while (moveIndex <= textLength - searchLength)
            {
                int caculateIndex = searchLength - 1;

                // 從右到左比對 pattern 和 text
                while (caculateIndex >= 0 && pattern[caculateIndex] == text[moveIndex + caculateIndex])
                    caculateIndex--;

                if (caculateIndex < 0)
                {
                    Console.WriteLine($"Pattern 出現在位置 {moveIndex}");
                    moveIndex += (moveIndex + searchLength < textLength) ? searchLength - badChar[text[moveIndex + searchLength]] : 1;
                }
                else
                {
                    moveIndex += Math.Max(1, caculateIndex - badChar[text[moveIndex + caculateIndex]]);
                }
            }
        }
    }
}
