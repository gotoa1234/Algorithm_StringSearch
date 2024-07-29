namespace Algorithm_StringSearch.StringSearch
{
    public class TrieAlgorithmExecute
    {
        public void Execute()
        {
            string text = "HERE IS A SIMPLE EXAMPLE ITEM";
            TrieAlgorithm trie = new TrieAlgorithm(text);
            var pattern = "EXAMPLES";
            Console.WriteLine($@"Text：{text}");
            Console.WriteLine($@"Pattern：{pattern} 查詢結果：{trie.Search(pattern)}");
            pattern = "SIM";
            Console.WriteLine($@"Pattern：{pattern} 查詢結果：{trie.Search(pattern)}");
            Console.WriteLine($@"Pattern：{pattern} 前綴查詢結果：{trie.StartsWith("SIM")}");
        }
    }

    /// <summary>
    /// 字典樹搜尋演算法 (前綴樹 Prefix Tree)
    /// </summary>
    public class TrieAlgorithm
    {
        private readonly TrieNode root;

        /// <summary>
        /// 1. 建構子
        /// </summary>
        /// <param name="text"></param>
        public TrieAlgorithm(string text)
        {
            root = new TrieNode();
            var textSplite = text.Split(' ');
            foreach (var item in textSplite)
            {
                this.Insert(item);
            }
        }

        /// <summary>
        /// 1-2. 插入文本到字典樹中
        /// </summary>
        /// <param name="text">文本</param>
        public void Insert(string text)
        {
            // 1-3. 從根節點開始
            TrieNode currentNode = root;
            foreach (char c in text)
            {
                // 1-4. 若不存在，則新增節點
                if (!currentNode.Children.ContainsKey(c))
                {
                    currentNode.Children[c] = new TrieNode();
                }
                // 1-5. 移動到下一個節點
                currentNode = currentNode.Children[c];
            }
            // 1-6. 標記為字的結尾
            currentNode.IsEndOfWord = true;
        }

        /// <summary>
        /// 2. 搜尋字典樹中是否包含某個字
        /// </summary>
        /// <param name="pattern">查找字串</param>
        /// <returns></returns>
        public bool Search(string pattern)
        {            
            TrieNode currentNode = root;
            // 2-1. 從根節點開始
            foreach (char c in pattern)
            {
                if (!currentNode.Children.ContainsKey(c))
                {
                    // 2-2. 若不存在，返回 false
                    return false;
                }
                // 2-3. 移動到下一個節點
                currentNode = currentNode.Children[c];
            }            
            return currentNode.IsEndOfWord;
        }

        /// <summary>
        /// 3. 搜尋字典樹中是否包含某個前綴
        /// </summary>
        /// <param name="prefix">查找字串 (部分字串)</param>
        /// <returns></returns>
        public bool StartsWith(string prefix)
        {
            TrieNode currentNode = root;
            // 3-1. 從根節點開始
            foreach (char c in prefix)
            {
                if (!currentNode.Children.ContainsKey(c))
                {
                    // 3-2. 若不存在，返回 false
                    return false;
                }
                // 3-3. 移動到下一個節點
                currentNode = currentNode.Children[c];
            }
            // 3-4. 表示找到前綴
            return true;
        }
    }

    /// <summary>
    /// 樹結構
    /// </summary>
    public class TrieNode
    {
        /// <summary>
        /// 字元與子節點的映射
        /// </summary>
        public Dictionary<char, TrieNode> Children { get; private set; } 
            = new Dictionary<char, TrieNode>();

        /// <summary>
        /// 是否為字的結尾
        /// </summary>
        public bool IsEndOfWord { get; set; } = false;

        /// <summary>
        /// 建構子
        /// </summary>
        public TrieNode()
        {
        }
    }
}
