namespace Algorithm_StringSearch.StringSearch
{
    public class SuffixTreeAlgorithmExecute
    {
        public void Execute()
        {
            string text = "bananas";
            var suffixTree = new SuffixTreeAlgorithm(text);

            Console.WriteLine("Suffix Tree built for text: " + text);

            string substring = "ana";
            Console.WriteLine($"Does the text contain the substring '{substring}'? " + suffixTree.ContainsSubstring(substring));
        }
    }

    public class SuffixTreeAlgorithm
    {
        private readonly string text;
        private readonly Node root;
        private Node activeNode;
        private int activeEdge;
        private int activeLength;
        private int remainingSuffixCount;
        private int leafEnd;
        private Node lastCreatedInternalNode;

        public SuffixTreeAlgorithm(string text)
        {
            this.text = text;
            root = new Node(-1, -1);
            activeNode = root;
            activeEdge = -1;
            activeLength = 0;
            remainingSuffixCount = 0;
            leafEnd = -1;
            lastCreatedInternalNode = null;

            BuildSuffixTree();
        }
        private void BuildSuffixTree()
        {
            for (int i = 0; i < text.Length; i++)
            {
                ExtendSuffixTree(i);
            }
        }

        private void ExtendSuffixTree(int pos)
        {
            leafEnd = pos;
            remainingSuffixCount++;
            lastCreatedInternalNode = null;

            while (remainingSuffixCount > 0)
            {
                if (activeLength == 0)
                {
                    activeEdge = pos;
                }

                char currentChar = text[activeEdge];
                if (!activeNode.Children.ContainsKey(currentChar))
                {
                    activeNode.Children[currentChar] = new Node(pos, leafEnd);
                    if (lastCreatedInternalNode != null)
                    {
                        lastCreatedInternalNode.SuffixLink = activeNode;
                        lastCreatedInternalNode = null;
                    }
                }
                else
                {
                    Node nextNode = activeNode.Children[currentChar];
                    int edgeLength = nextNode.End - nextNode.Start + 1;

                    if (activeLength >= edgeLength)
                    {
                        activeEdge += edgeLength;
                        activeLength -= edgeLength;
                        activeNode = nextNode;
                        continue;
                    }

                    if (text[nextNode.Start + activeLength] == text[pos])
                    {
                        activeLength++;
                        if (lastCreatedInternalNode != null)
                        {
                            lastCreatedInternalNode.SuffixLink = activeNode;
                            lastCreatedInternalNode = null;
                        }
                        break;
                    }

                    Node splitNode = new Node(nextNode.Start, nextNode.Start + activeLength - 1);
                    activeNode.Children[currentChar] = splitNode;
                    splitNode.Children[text[pos]] = new Node(pos, leafEnd);
                    nextNode.Start += activeLength;
                    splitNode.Children[text[nextNode.Start]] = nextNode;

                    if (lastCreatedInternalNode != null)
                    {
                        lastCreatedInternalNode.SuffixLink = splitNode;
                    }
                    lastCreatedInternalNode = splitNode;
                }

                remainingSuffixCount--;

                if (activeNode == root && activeLength > 0)
                {
                    activeLength--;
                    activeEdge = pos - remainingSuffixCount + 1;
                }
                else if (activeNode != root)
                {
                    activeNode = activeNode.SuffixLink;
                }
            }
        }

        public bool ContainsSubstring(string substring)
        {
            Node currentNode = root;
            int substringIndex = 0;

            while (substringIndex < substring.Length)
            {
                if (!currentNode.Children.ContainsKey(substring[substringIndex]))
                {
                    return false;
                }

                Node nextNode = currentNode.Children[substring[substringIndex]];
                int edgeLength = nextNode.End - nextNode.Start + 1;
                int lengthToMatch = Math.Min(edgeLength, substring.Length - substringIndex);

                if (text.Substring(nextNode.Start, lengthToMatch) != substring.Substring(substringIndex, lengthToMatch))
                {
                    return false;
                }

                substringIndex += lengthToMatch;
                currentNode = nextNode;
            }

            return true;
        }


        private class Node
        {
            public Dictionary<char, Node> Children { get; } = new Dictionary<char, Node>();
            public int Start { get; set; }
            public int End { get; set; }
            public Node SuffixLink { get; set; }

            public Node(int start, int end)
            {
                Start = start;
                End = end;
            }
        }
    }


}
