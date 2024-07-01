// See https://aka.ms/new-console-template for more information
using Algorithm_StringSearch.StringSearch;

// 1. Rabin-Karp Algorithm
var alRabinKarp = new RabinKarpAlgorithmExecute();
alRabinKarp.Execute();

// 2. KMP Algorithm
var alKMP = new KMPAlgorithmExecute();
alKMP.Execute();

// 3. Boyer Moore Algorithm
var alBoyerMoore = new BoyerMooreAlgorithmExecute();
alBoyerMoore.Execute();

Console.WriteLine("Wait a minute.");