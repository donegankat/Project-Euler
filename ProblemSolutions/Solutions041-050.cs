using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ProjectEuler.ProblemSolutions
{
    public static partial class Solutions
    {
        /// <summary>
        /// Problem 50
        /// https://projecteuler.net/problem=50
        /// </summary>
        public static void RunConsecutivePrimeSumsProblem()
        {
            var limit = 1000000;
            long maxPrimeSum = 0;
            long maxCountOfConsecutivePrimes = 0;
            var primes = Helpers.GetPrimeNumbers(limit);
            
            foreach (var prime in primes)
            {
                long currentSum = prime;
                var currentCount = 1;

                foreach (var prime2 in primes.Where(p => p > prime))
                {
                    var isCurrentSumPrime = primes.Contains(currentSum);
                    
                    if (isCurrentSumPrime || currentSum + prime2 > limit)
                    {
                        if (isCurrentSumPrime && currentCount > maxCountOfConsecutivePrimes)
                        {
                            maxCountOfConsecutivePrimes = currentCount;
                            maxPrimeSum = currentSum;
                        }

                        if (currentSum + prime2 > limit)
                            break;
                    }

                    currentSum += prime2;
                    currentCount++;
                }
            }

            Console.WriteLine($"Prime with Most Consecutive Summed Primes: {maxPrimeSum}");
            Console.WriteLine($"Number of Summed Primes: {maxCountOfConsecutivePrimes}");
        }

        /// <summary>
        /// Problem 49
        /// https://projecteuler.net/problem=49
        /// </summary>
        public static void RunPrimePermutationsProblem()
        {
            var increment = 3330;

            // We're only looking for 4-digit numbers, the max of which can only possibly be
            // 9999 - 3330 - 3330, a.k.a. 3339. We also know from the question that we can
            // skip everything up through the example provided in the question, which was
            // the sequence 1487, 4817, 8147.
            for (var i = 1488; i < 3339; i++)
            {
                var increment1 = i + increment;
                var increment2 = increment1 + increment;

                if (Helpers.IsPrime(i) &&
                    Helpers.IsPrime(increment1) &&
                    Helpers.IsPrime(increment2)
                )
                {
                    var digits = i.ToString().ToCharArray();
                    var allIncrementsArePermutations = true;

                    foreach (var digit in digits)
                    {
                        if (increment1.ToString().IndexOf(digit) < 0 ||
                            increment2.ToString().IndexOf(digit) < 0)
                        {
                            allIncrementsArePermutations = false;
                            break;
                        }
                    }

                    if (allIncrementsArePermutations)
                    {
                        Console.WriteLine($"Prime Permutations: {i}, {increment1}, {increment2}");
                        Console.WriteLine($"Concatenated: {i}{increment1}{increment2}");
                    }
                }
            }
        }

        /// <summary>
        /// Problem 48
        /// https://projecteuler.net/problem=48
        /// </summary>
        public static void RunSelfPowersProblem()
        {
            BigInteger totalSum = 0;
            var modulus = (BigInteger)Math.Pow(10, 10);

            for (var i = 1; i <= 1000; i++)
            {
                BigInteger pow = i;
                for (var j = 1; j < i; j++)
                {
                    pow *= i;
                    pow = pow % modulus; // Only keep the 10 least significant digits.
                }
                
                totalSum += pow;
            }

            Console.WriteLine($"Sum: {totalSum}");
            Console.WriteLine($"Last 10 Digits: {totalSum.ToString().Substring(totalSum.ToString().Length - 10)}");
        }

        /// <summary>
        /// Problem 47
        /// https://projecteuler.net/problem=47
        /// </summary>
        public static void RunConsecutiveDistinctPrimeFactorsProblem()
        {
            var previousPrimeFactors = new HashSet<int>();
            var primes = Helpers.GetPrimeNumbers(150000);

            for (var i = 100; i < 150000; i++)
            {
                if (CountDistinctPrimeFactors(i) == 4
                && CountDistinctPrimeFactors(i + 1) == 4
                && CountDistinctPrimeFactors(i + 2) == 4
                && CountDistinctPrimeFactors(i + 3) == 4)
                {
                    Console.WriteLine($"First Number in Consecutive Sequence with 4 Distinct Prime Factors: {i}");
                    return;
                }
            }
        }

        /// <summary>
        /// Returns the count of the distinct number of prime factors of the given number.
        /// 
        /// Adapted from: https://sikademy.com/career-training/iq-aptitude-and-vocational-training/project-euler-problems-and-complete-solution/project-euler-problem-47-distinct-primes-factors/
        /// </summary>
        private static int CountDistinctPrimeFactors(int n)
        {
            int count = 0;
            for (int i = 2, end = (int)Math.Sqrt(n); i <= end; i++)
            {
                if (n % i == 0)
                {
                    do n /= i;
                    while (n % i == 0);
                    count++;
                    end = (int)Math.Sqrt(n);
                }
            }

            if (n > 1)
                count++;

            return count;
        }

        /// <summary>
        /// Problem 46
        /// https://projecteuler.net/problem=46
        /// </summary>
        public static void RunGoldbachOddCompositeNonPrimeSquareProblem()
        {
            var primes = Helpers.GetPrimeNumbers(10000);

            for (var i = 3; i < 10000; i += 2)
            {
                // Prime numbers aren't composite numbers.
                if (Helpers.IsPrime(i))
                    continue;
                
                var didFitPattern = false;

                foreach (var prime in primes.Where(p => p < i))
                {
                    var remainder = i - prime;
                    if (remainder % 2 == 0 && Math.Sqrt(remainder / 2) % 1 == 0)
                    {
                        didFitPattern = true;
                        break;
                    }
                }

                if (!didFitPattern)
                {
                    Console.WriteLine($"Smallest Odd That Breaks the Pattern: {i}");
                    return;
                }
            }
        }

        /// <summary>
        /// Problem 45
        /// https://projecteuler.net/problem=45
        /// 
        /// Triangle, pentagonal, and hexagonal numbers are generated by the following formulae:
        /// Triangle     Tn=n(n+1)/2     1, 3, 6, 10, 15, ...
        /// Pentagonal   Pn=n(3n−1)/2    1, 5, 12, 22, 35, ...
        /// Hexagonal    Hn=n(2n−1)      1, 6, 15, 28, 45, ...
        /// It can be verified that T285 = P165 = H143 = 40755.
        /// Find the next triangle number that is also pentagonal and hexagonal.
        /// </summary>
        public static void RunTriangularPentagonalHexagonalProblem()
        {
            var triangularNumbers = new HashSet<long>();
            var pentagonalNumbers = new HashSet<long>();
            var hexagonalNumbers = new HashSet<long>();

            long n = 144;

            while (true)
            {
                long triangular = n * (n + 1) / 2;
                long pentagonal = n * ((3 * n) - 1) / 2;
                long hexagonal = n * ((2 * n) - 1);

                if (pentagonalNumbers.Contains(triangular) && hexagonalNumbers.Contains(triangular))
                {
                    Console.WriteLine($"Next Triangular, Pentagonal, and Hexagonal Number: {triangular}");
                    return;
                }

                triangularNumbers.Add(triangular);
                pentagonalNumbers.Add(pentagonal);
                hexagonalNumbers.Add(hexagonal);
                n++;
            }
        }

        /// <summary>
        /// Problem 44
        /// https://projecteuler.net/problem=44
        /// </summary>
        public static void RunPentagonalNumbersProblem()
        {
            var pentagonalNumbers = new HashSet<long>();
            var smallestPentagonalDifference = long.MaxValue;

            // Build a list of pentagonal numbers.
            // Pentagonal numbers formula: Pn=n(3n−1)/2
            for (var i = 1; i < 10000; i++)
            {
                long pentagonalResult = i * ((3 * i) - 1) / 2;

                pentagonalNumbers.Add(pentagonalResult);
            }

            var visitedPentagonals = new HashSet<long>();

            // Visit each pentagonal number and check its sum and difference with
            // every other pentagonal number to see whether both the sum and diff
            // are pentagonal.
            // Decent explanation of the thinking behind this approach:
            // https://euler.beerbaronbill.com/en/latest/solutions/44.html
            foreach (var pk in pentagonalNumbers)
            {
                foreach (var pj in visitedPentagonals.Reverse())
                {
                    var difference = pk - pj;
                    if (pentagonalNumbers.Contains(pk + pj) && visitedPentagonals.Contains(difference))
                    {
                        if (difference < smallestPentagonalDifference)
                            smallestPentagonalDifference = difference;
                    }
                }

                visitedPentagonals.Add(pk);
            }

            Console.WriteLine($"Smallest Difference Between Pentagonal Numbers: {smallestPentagonalDifference}");
        }

        /// <summary>
        /// Problem 43
        /// https://projecteuler.net/problem=43
        /// </summary>
        public static void RunSubstringDivisibilityProblem()
        {
            var allPandigitalNumbers = Helpers.GetWordPermutations("0123456789");
            var divisibleSubstringNumbers = new HashSet<long>();

            // Determine which pandigital numbers match the following formula (indexes are 1-based):
            // d2d3d4 = divisible by 2
            // d3d4d5 = divisible by 3
            // d4d5d6 = divisible by 5
            // d5d6d7 = divisible by 7
            // d6d7d8 = divisible by 11
            // d7d8d9 = divisible by 13
            // d8d9d10 = divisible by 17

            var divisors = new int[7] {2, 3, 5, 7, 11, 13, 17};
            foreach (var pandigitalNumber in allPandigitalNumbers)
            {
                var currentDivisorIndex = 0;
                var allSubstringsMatchFormula = true;

                for (var i = 1; i <= 7; i++)
                {
                    var numberString = $"{pandigitalNumber[i]}{pandigitalNumber[i + 1]}{pandigitalNumber[i + 2]}";
                    if (int.Parse(numberString) % divisors[currentDivisorIndex] != 0)
                    {
                        allSubstringsMatchFormula = false;
                        break;
                    }

                    currentDivisorIndex++;
                }

                if (allSubstringsMatchFormula)
                    divisibleSubstringNumbers.Add(long.Parse(pandigitalNumber));
            }

            Console.WriteLine($"Sum of Divisible Substring Pandigital Numbers: {divisibleSubstringNumbers.Sum()}");
        }

        private class TriangleWord
        {
            public string Word { get; set; }

            public int CharSum { get; set; }

            public TriangleWord(string word)
            {
                Word = word;

                // Subtract 64 from each character to convert from the ASCII code to the alphabetical
                // index for each letter.
                CharSum = word.ToCharArray().Sum(wordChar => wordChar - 64);
            }
        }

        /// <summary>
        /// Problem 42
        /// https://projecteuler.net/problem=42
        /// </summary>
        public static void RunCodedTriangleNumbersProblem()
        {
            var filePath = "Resources/p042_words.txt";
            var allPossibleTriangleWordsString = File.ReadAllText(filePath);
            var allPossibleTriangleWords = allPossibleTriangleWordsString.Replace("\"", "").Split(',').Select(word => new TriangleWord(word));
            var triangleWords = new List<TriangleWord>();

            var highestPossibleTriangleSum = allPossibleTriangleWords.OrderByDescending(word => word.CharSum).FirstOrDefault().CharSum;

            for (var i = 1; i <= highestPossibleTriangleSum; i++)
            {
                // See if the total sum of the letters in the word fit the equation: tn = ½n(n+1)
                var result = (0.5 * i) * (i + 1);
                var matchingTriangleWords = allPossibleTriangleWords.Where(triangleWord => triangleWord.CharSum == result);
                if (matchingTriangleWords != null && matchingTriangleWords.Any())
                    triangleWords.AddRange(matchingTriangleWords);
            }

            Console.WriteLine($"Triangle Words ({triangleWords.Count()}):\n{string.Join("\n", triangleWords.Select(triangleWord => triangleWord.Word))}");
        }

        /// <summary>
        /// Problem 41
        /// https://projecteuler.net/problem=41
        /// </summary>
        public static void RunLargestPandigitalPrimeProblem()
        {
            // Lazy way: Using the trick found here, we can determine that the highest possible
            // number that could possibly be prime would be 7654321, so we can eliminate all
            // pandigital numbers higher than that:
            // https://www.mathblog.dk/project-euler-41-pandigital-prime/
            // 
            // Excerpt:
            // A number is divisible by 3 if and only if the digit sum of the number is divisible by
            // 3. The digit sum is as the name implies the sum of the digits. We can rather easily
            // find the digit sum of pandigital numbers since we know the digits.
            // 1+2+3+4+5+6+7+8+9 = 45
            // 1+2+3+4+5+6+7+8 = 36
            // 1+2+3+4+5+6+7 = 28
            // 1+2+3+4+5+6 = 21
            // 1+2+3+4+5 = 15
            // 1+2+3+4 = 10
            // 1+2+3 = 6
            // 1+2 = 3
            // From here it is pretty clear that all pandigital numbers except 4 and 7 digit ones are
            // divisible by 3 and thus can’t be primes.

            for (var i = 7654321; i >= 0; i -= 2)
            {
                if (Helpers.IsPandigital(i, i.ToString().Length) && Helpers.IsPrime(i))
                {
                    Console.WriteLine($"Largest Pandigital Prime: {i}");
                    return;
                }
            }
        }
    }
}