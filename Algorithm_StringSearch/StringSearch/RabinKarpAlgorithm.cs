using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_StringSearch.StringSearch
{
    public class RabinKarpAlgorithmExecute
    {
        public void Execute()
        {

        }
    }


    public class RabinKarpAlgorithm
    {
        public RabinKarpAlgorithm()
        {
        }

        public void CountingAsedingSort(List<int> inputItem)
        {
            int[] counting = new int[100];
            for (int i = 0; i < inputItem.Count; i++)
            {
                counting[inputItem[i]]++;
            }

            for (int i = 0; i < counting.Length; i++)
            {
                if (counting[i] > 0)
                {
                    Console.WriteLine(i + " : " + counting[i]);
                }
            }
        }
    }
}
