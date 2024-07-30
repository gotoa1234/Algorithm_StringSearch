using System.Runtime.InteropServices;

namespace Algorithm_StringSearch.StringSearch
{
    public class SuffixTreeAlgorithmExecute
    {
        public void Execute()
        {
            // 空間複雜度：𝑂(𝑛log⁡𝑛)
            string text = "bananas";
            // [建構]後綴樹的時間複雜度是 𝑂(𝑛^2) ※另一種 Ukkonen 算法只需 𝑂(𝑛)
            var suffixTree = new SuffixTreeAlgorithm(text);

            Console.WriteLine("後綴樹成功建立。");

            // [搜尋]時間複雜度：O(m)，其中 𝑚 m 是模式的長度
            string pattern = "ana";
            Console.WriteLine($"模式 '{pattern}' 是否存在: {suffixTree.Search(pattern)}");

            pattern = "nana";
            Console.WriteLine($"模式 '{pattern}' 是否存在: {suffixTree.Search(pattern)}");

            pattern = "apple";
            Console.WriteLine($"模式 '{pattern}' 是否存在: {suffixTree.Search(pattern)}");

/*

[建構過程的詳細說明]
1. 初始化：
   - 創建一個空的根節點。
   - 遍歷文本的每個後綴。

2. 添加後綴：
   - 對於每個後綴，從根節點開始。
   - 根據後綴的字符遍歷樹。
   - 如果當前節點的子節點中沒有找到相應字符，為此字符創建一個新節點。
   - 繼續此過程，直到整個後綴被添加到樹中。
   - 在終端節點更新後綴索引。       

[搜尋過程的詳細說明]
1. 從根節點開始：
   - 從後綴樹的根節點開始。
2. 遍歷樹：
   - 對於模式中的每個字符，檢查當前節點的子節點中是否存在相應的字符。
   - 如果子節點存在，則移動到該節點。
   - 如果子節點不存在，則該模式不在文本中。
3. 結果：
   - 如果找到模式中的所有字符，則該模式存在於文本中。
   - 否則，該模式不存在。

 */
        }
    }

    public class SuffixTreeAlgorithm
    {
        private readonly string _text;
        private readonly SuffixTreeNode _root;
        private SuffixTreeNode _activeNode;
        private int _activeEdge;
        private int _activeLength;
        private int _remainingSuffixCount;
        private int _leafEnd;
        private SuffixTreeNode _lastNewNode;
        private int _currentPos;


        /// <summary>
        /// 建構式
        /// </summary>        
        public SuffixTreeAlgorithm(string text)
        {
            _text = text;
            _root = new SuffixTreeNode(-1);
            _root.SuffixLink = _root;
            _activeNode = _root;
            _activeEdge = -1;
            _activeLength = 0;
            _remainingSuffixCount = 0;
            _leafEnd = -1;
            _lastNewNode = null;
            _currentPos = -1;

            // 建立後綴樹
            BuildSuffixTree();
        }

        /// <summary>
        /// 建立後綴樹
        /// </summary>
        private void BuildSuffixTree()
        {
            // 依序加入所有後綴
            for (int index = 0; index < _text.Length; index++)
            {
                ExtendSuffixTree(index);
            }
        }

        /// <summary>
        /// 添加後綴
        /// </summary>       
        private void ExtendSuffixTree(int pos)
        {
            _leafEnd = pos;
            _remainingSuffixCount++;
            _lastNewNode = null;

            while (_remainingSuffixCount > 0)
            {
                if (_activeLength == 0)
                    _activeEdge = pos;

                if (!_activeNode.Children.ContainsKey(_text[_activeEdge]))
                {
                    _activeNode.Children[_text[_activeEdge]] = new SuffixTreeNode(pos, _leafEnd);

                    if (_lastNewNode != null)
                    {
                        _lastNewNode.SuffixLink = _activeNode;
                        _lastNewNode = null;
                    }
                }
                else
                {
                    SuffixTreeNode next = _activeNode.Children[_text[_activeEdge]];
                    if (_activeLength >= next.EdgeLength(pos))
                    {
                        _activeEdge += next.EdgeLength(pos);
                        _activeLength -= next.EdgeLength(pos);
                        _activeNode = next;
                        continue;
                    }

                    if (_text[next.Start + _activeLength] == _text[pos])
                    {
                        if (_lastNewNode != null && _activeNode != _root)
                        {
                            _lastNewNode.SuffixLink = _activeNode;
                            _lastNewNode = null;
                        }

                        _activeLength++;
                        break;
                    }

                    int splitEnd = next.Start + _activeLength - 1;
                    SuffixTreeNode split = new SuffixTreeNode(next.Start, splitEnd);
                    _activeNode.Children[_text[_activeEdge]] = split;

                    split.Children[_text[pos]] = new SuffixTreeNode(pos, _leafEnd);
                    next.Start += _activeLength;
                    split.Children[_text[next.Start]] = next;

                    if (_lastNewNode != null)
                    {
                        _lastNewNode.SuffixLink = split;
                    }

                    _lastNewNode = split;
                }

                _remainingSuffixCount--;

                if (_activeNode == _root && _activeLength > 0)
                {
                    _activeLength--;
                    _activeEdge = pos - _remainingSuffixCount + 1;
                }
                else if (_activeNode != _root)
                {
                    _activeNode = _activeNode.SuffixLink;
                }
            }
        }


        // 搜尋
        public bool Search(string pattern)
        {
            SuffixTreeNode currentNode = _root;
            int length = 0;
            foreach (char ch in pattern)
            {
                if (!currentNode.Children.ContainsKey(ch))
                {
                    return false;
                }
                currentNode = currentNode.Children[ch];
                length++;
                if (length >= pattern.Length)
                {
                    return true;
                }
            }
            return true;
        }


        /// <summary>
        /// 後綴樹節點
        /// </summary>
        public class SuffixTreeNode
        {
            public Dictionary<char, SuffixTreeNode> Children { get; private set; }
            public SuffixTreeNode SuffixLink { get; set; }
            public int Start { get; set; }
            public int End { get; set; }
            public int SuffixIndex { get; set; }

            public SuffixTreeNode(int start, int end = -1)
            {
                Children = new Dictionary<char, SuffixTreeNode>();
                SuffixLink = null;
                Start = start;
                End = end;
                SuffixIndex = -1;
            }

            public int EdgeLength(int position)
            {
                return Math.Min(End, position + 1) - Start;
            }
        }
    }


}
