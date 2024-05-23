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
            var temp = new RabinKarpAlgorithm();
        }
    }


    public class RabinKarpAlgorithm
    {
        public RabinKarpAlgorithm()
        {
            string text = "ABCCDDAEFG";
            string pattern = "CDD";
            int prime = 101; // A prime number

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

        public static int RabinKarpSearch(string text, string pattern, int prime)
        {
            int n = text.Length;
            int m = pattern.Length;
            int i, j;
            int p = 0; // pattern hash value
            int t = 0; // text hash value
            int h = 1;
            int d = 256; // Number of characters in the input alphabet

            // Calculate the value of h (h = d^(m-1) % prime)
            for (i = 0; i < m - 1; i++)
            {
                h = (h * d) % prime;
            }

            // Calculate the hash value of the pattern and first window of text
            for (i = 0; i < m; i++)
            {
                p = (d * p + pattern[i]) % prime;
                t = (d * t + text[i]) % prime;
            }

            // Slide the pattern over text one by one
            for (i = 0; i <= n - m; i++)
            {
                // Check the hash values of the current window of text and pattern
                if (p == t)
                {
                    // If the hash values match, then only check characters one by one
                    for (j = 0; j < m; j++)
                    {
                        if (text[i + j] != pattern[j])
                        {
                            break;
                        }
                    }

                    // If p == t and pattern[0...m-1] == text[i...i+m-1]
                    if (j == m)
                    {
                        return i; // Pattern found at index i
                    }
                }

                // Calculate hash value for next window of text
                if (i < n - m)
                {
                    t = (d * (t - text[i] * h) + text[i + m]) % prime;

                    // We might get negative value of t, converting it to positive
                    if (t < 0)
                    {
                        t = (t + prime);
                    }
                }
            }

            return -1; // Pattern not found
        }
    }
}
