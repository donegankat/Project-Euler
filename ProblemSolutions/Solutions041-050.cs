using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ProjectEuler.ProblemSolutions
{
    public static partial class Solutions
    {
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

        public class TriangleWord
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