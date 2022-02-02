using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Numerics;

namespace ProjectEuler
{
    public static class Helpers
    {
        /// <summary>
        /// Returns the list of all numbers which divide evenly into the given number.
        /// </summary>
        public static List<long> GetEvenDivisors(long number)
        {
            List<long> divisors = new List<long>();
            divisors.Add(1);

            for (var i = 2; i <= number / 2; i++)
            {
                if (number % i == 0)
                    divisors.Add(i);
            }

            return divisors;
        }
        
        /// <summary>
        /// Returns whether or not the given number is prime.
        /// </summary>
        public static bool IsPrime(long number)
        {
            if (number < 2)
                return false;

            if (number % 2 == 0)
                return number == 2;

            if (number % 3 == 0)
                return number == 3;

            var i = 5;

            while (i <= Math.Floor(1 + Math.Sqrt(number)))
            {
                if (number % i == 0)
                    return false;
                if (number % (i + 2) == 0)
                    return false;
                i+= 6;
            }
            
            return true;
        }

        /// <summary>
        /// Returns the list of all prime numbers up to or below the given maximum.
        /// </summary>
        public static HashSet<long> GetPrimeNumbers(long maxNumber)
        {
            var primes = new HashSet<long>();
            primes.Add(2);

            for (var i = 3; i <= maxNumber; i += 2)
            {
                if (IsPrime(i))
                    primes.Add(i);
            }

            return primes;
        }

        /// <summary>
        /// Returns whether or not the given number is pandigital (meaning that it contains each of the numbers
        /// 1 through 9 exactly once).
        /// </summary>
        public static bool IsPandigital(int number)
        {
            return IsPandigital(number, 1, 9);
        }

        /// <summary>
        /// Returns whether or not the given number is pandigital (meaning that it contains each of the numbers
        /// 1 through the max digit exactly once).
        /// </summary>
        public static bool IsPandigital(int number, int maxPandigit = 9)
        {
            return IsPandigital(number, 1, maxPandigit);
        }

        /// <summary>
        /// Returns whether or not the given number is pandigital (meaning that it contains each of the numbers
        /// from the min digit through the max digit exactly once).
        /// </summary>
        public static bool IsPandigital(int number, int minPandigit, int maxPandigit)
        {
            var charsToLookFor = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            charsToLookFor = charsToLookFor.Skip(minPandigit).Take(maxPandigit).ToList();

            var strNumber = number.ToString();
            
            if (strNumber.Length != charsToLookFor.Count())
                return false;

            foreach (var num in strNumber.ToCharArray())
            {
                if (!charsToLookFor.Contains(num))
                    return false;
                
                charsToLookFor.Remove(num);
            }
            
            return charsToLookFor.Count == 0;
        }

        /// <summary>
		/// Returns all permutations for the given string.
		/// Adapted from: https://www.iditect.com/guide/csharp/csharp_howto_list_all_permutations_of_a_string.html
		/// </string>
		public static List<string> GetWordPermutations(string word, string prefix = "")
		{
			if (string.IsNullOrWhiteSpace(word))
				return new List<string>() { prefix };
			
			var result = new List<string>();
			
			// Each character need to be permutated.
			for (int i = 0; i < word.Length; i++)
			{
				// Remove current char from original word, append it to prefix, then permute recursively.
				result.AddRange(GetWordPermutations(word.Remove(i, 1), prefix + word[i]));
			}

			return result;
		}
    }
}