using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Numerics;

namespace ProjectEuler.ProblemSolutions
{
    public static partial class Solutions
    {
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