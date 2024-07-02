﻿// See https://aka.ms/new-console-template for more information
using Algorithm_StringSearch.StringSearch;

Console.WriteLine("====");

// 1. Rabin-Karp Algorithm
var alRabinKarp = new RabinKarpAlgorithmExecute();
alRabinKarp.Execute();

Console.WriteLine("====");

// 2. KMP Algorithm
var alKMP = new KMPAlgorithmExecute();
alKMP.Execute();

Console.WriteLine("====");

// 3. Boyer Moore Algorithm
var alBoyerMoore = new BoyerMooreAlgorithmExecute();
alBoyerMoore.Execute();

Console.WriteLine("Wait a minute.");