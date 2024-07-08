using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_StringSearch.StringSearch
{
    public class FiniteStateMachineAlgorithmExecute
    {
        public void Execute()
        {
            string text = "HERE IS A SIMPLE EXAMPLE";
            string pattern = "EXAMPLE";
            var fsm = new FiniteStateMachineAlgorithm();

            bool patternFound = fsm.FiniteStateMachineAlgoritSearch(text, pattern);

            if (patternFound)
            {
                Console.WriteLine("文本中找到了模式！");
            }
            else
            {
                Console.WriteLine("文本中没有找到模式。");
            }
        }
    }

    public class FiniteStateMachineAlgorithm
    {
        public FiniteStateMachineAlgorithm()
        {
               
        }

        public int[,] InitialPatternPreProcess(string pattern)
        {
            int statesCount = pattern.Length + 1; // 状态数量等于模式长度加一
            int alphabetSize = 256; // 假设字符集大小为ASCII字符集的大小

            // 初始化状态转移表
            var transitionTable = new int[statesCount, alphabetSize];

            // 构建状态转移表
            for (int state = 0; state < statesCount; state++)
            {
                for (int c = 0; c < alphabetSize; c++)
                {
                    // 默认转移到初始状态
                    transitionTable[state, c] = 0;
                }
            }

            // 根据模式构建状态转移表
            for (int state = 0; state < pattern.Length; state++)
            {
                transitionTable[state, pattern[state]] = state + 1;
            }

            return transitionTable;
        }

        public bool FiniteStateMachineAlgoritSearch(string text, string pattern)
        {
            var transitionTable = this.InitialPatternPreProcess(pattern);
            var currentState = 0;

            for (int i = 0; i < text.Length; i++)
            {
                currentState = transitionTable[currentState, text[i]];

                // 如果当前状态达到最终状态（模式长度），则找到了匹配
                if (currentState == pattern.Length)
                {
                    return true;
                }
            }

            // 在文本中没有找到完整的模式
            return false;
        }
    }
}
