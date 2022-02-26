using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.IO;

namespace ProjectEuler.ProblemSolutions
{
    public static partial class Solutions
    {
        /// <summary>
        /// Problem 30
        /// https://projecteuler.net/problem=30
        /// </summary>
        public static void RunFifthPowerDigitsProblem()
		{
			var numbersSummedByFifthPowerOfDigits = new List<int>();
			
			for (var i = 2; i < 999999; i++)
			{
				var digits = i.ToString().ToCharArray().ToList().Select(numChar => int.Parse(numChar.ToString()));
				var sum = digits.Sum(digit => Math.Pow(digit, 5));
				
				if (sum == i)
					numbersSummedByFifthPowerOfDigits.Add(i);
			}
			
			Console.WriteLine("Numbers Summable by Fifth Power of Digits: " + string.Join(", ", numbersSummedByFifthPowerOfDigits));
			Console.WriteLine("Sum: " + numbersSummedByFifthPowerOfDigits.Sum());
		}
		
        /// <summary>
        /// Problem 29
        /// https://projecteuler.net/problem=29
        /// </summary>
		public static void RunDistinctPowersProblem()
		{
			var distinctPowers = new HashSet<double>();
			
			for (var a = 2; a <= 100; a++)
			{
				for (var b = 2; b <= 100; b++)
				{
					distinctPowers.Add(Math.Pow(a, b));
				}
			}
			
			Console.WriteLine("Distinct Powers: " + distinctPowers.Count());
		}
		
		private enum SpiralDirection
        {
            Left,
            Right,
            Up,
            Down
        }
		
		private class Coordinate
		{
			public int X { get; set; }
			public int Y { get; set; }
			
			public Coordinate(int x, int y)
			{
				X = x;
				Y = y;
			}
			
			public override bool Equals(Object obj)
			{
				//Check for null and compare run-time types.
				if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
				{
					return false;
				}
				else
				{
					Coordinate c = (Coordinate)obj;
					return (X == c.X) && (Y == c.Y);
				}
			}

			public override int GetHashCode()
			{
				return (X << 2) ^ Y;
			}
		}
		
        /// <summary>
        /// Problem 28
        /// https://projecteuler.net/problem=28
        /// </summary>
		public static void RunNumberSpiralDiagonalsProblem()
		{
			// Helper used for initial conceptualizing of the problem.
			//BuildAndPrintSpiral(10000);
			
			// This problem can be solved mathematically. The top right corner of each layer of the spiral
			// is the length of the spiral layer squared, so a 3x3 spiral's top right number is 9, a 5x5's
			// top right corner is 25, and so on.
			// The top left corner is the top right corner - (the length of the spiral layer - 1), so a 3x3
			// spiral's top left corner is 7, a 5x5's top left corner is 21, and so on.
			// The bottom left corner follows the same pattern, where it's the top left corner - (the height
			// of the spiral layer - 1), so 3x3 = 5, 5x5 = 17, and so on.
			// Same goes for the bottom right corner.
			long totalDiagonalSum = 0;
			var maxSpiralLength = 1001;
			
			// Each iteration decrements by 2 because we're losing both a height and width.
			for (var i = maxSpiralLength; i > 2; i -= 2)
			{
				var topRightCorner = (long)Math.Pow(i, 2);
				var topLeftCorner = topRightCorner - i + 1;
				var bottomLeftCorner = topLeftCorner - i + 1;
				var bottomRightCorner = bottomLeftCorner - i + 1;
				
				//Console.WriteLine("Top Right: " + topRightCorner + " Top Left: " + topLeftCorner + " Bottom Left: " + bottomLeftCorner + " Bottom Right: " + bottomRightCorner);
				totalDiagonalSum += topRightCorner + topLeftCorner + bottomLeftCorner + bottomRightCorner;
			}
			
			// Add in the center number.
			totalDiagonalSum += 1;
			
			Console.WriteLine("Total Sum of Diagonals: " + totalDiagonalSum);
		}
		
		/// <summary>
		/// Helper method for debug purposes. Builds and prints a number spiral in a formatted order.
		/// </summary>
		private static void BuildAndPrintSpiral(int maxSpiralNumber)
		{
			var spiral = new Dictionary<Coordinate, int>();
			var x = 0;
			var y = 0;

			// Start the loop thinking that we're heading up, which will immediately tell us that we can
			// switch to going right.
			var currentDirection = SpiralDirection.Up;
			
			for (var i = 1; i <= maxSpiralNumber; i++)
			{
				spiral.Add(new Coordinate(x, y), i);
				
				if (currentDirection == SpiralDirection.Up)
				{
					// When we're going up, always look for the first opportunity to go right and take it
					// whenever possible.
					if (!spiral.ContainsKey(new Coordinate(x + 1, y)))
					{
						//Console.WriteLine(i + ": UP -> RIGHT");
						x++;
						currentDirection = SpiralDirection.Right;
					}
					else
						y++;
				}
				else if (currentDirection == SpiralDirection.Right)
				{
					// When we're going right, always look for the first opportunity to go down and take it
					// whenever possible.
					if (!spiral.ContainsKey(new Coordinate(x, y - 1)))
					{
						//Console.WriteLine(i + ": RIGHT -> DOWN");
						y--;
						currentDirection = SpiralDirection.Down;
					}
					else
						x++;
				}
				else if (currentDirection == SpiralDirection.Down)
				{
					// When we're going down, always look for the first opportunity to go left and take it
					// whenever possible.
					if (!spiral.ContainsKey(new Coordinate(x - 1, y)))
					{
						//Console.WriteLine(i + ": DOWN -> LEFT");
						x--;
						currentDirection = SpiralDirection.Left;
					}
					else
						y--;
				}
				else if (currentDirection == SpiralDirection.Left)
				{
					// When we're going left, always look for the first opportunity to go up and take it
					// whenever possible.
					if (!spiral.ContainsKey(new Coordinate(x, y + 1)))
					{
						//Console.WriteLine(i + ": LEFT -> UP");
						y++;
						currentDirection = SpiralDirection.Up;
					}
					else
						x--;
				}
			}
			
			Console.WriteLine("Dimensions: X: " + spiral.Keys.OrderByDescending(key => key.X).FirstOrDefault().X + " Y: " + spiral.Keys.OrderByDescending(key => key.Y).FirstOrDefault().Y);
			
			var currentLine = 0;
			var maxCoordCharacters = spiral.LastOrDefault().Value.ToString().Length;
            foreach (var coord in spiral.Keys.OrderByDescending(key => key.Y).ThenBy(key => key.X))
            {
                if (currentLine != coord.Y)
                {
                    Console.WriteLine();
                    currentLine = coord.Y;
                }
				
				var coordToPrint = spiral[coord].ToString();
				
				// If the length of the current spiral number is less than the highest number of characters in
				// the spiral, pad it with extra spaces so that all of the numbers line up when printed.
				for (var numSpaces = coordToPrint.Length; numSpaces < maxCoordCharacters; numSpaces++)
					coordToPrint = coordToPrint + " ";
				
                Console.Write(coordToPrint + "\t");
            }
		}
		
        /// <summary>
        /// Problem 27
        /// https://projecteuler.net/problem=27
        /// </summary>
		public static void RunQuadraticPrimesProblem()
		{
			// Formula: n^2 + an + b where |a| < 1000 and |b| <= 1000
			// Since n starts at 0, that means the first iteration works out to: 0^2 + 0a + b, a.k.a. b
			// And since we need sequential primes, that means that b must be prime since n = 0 is simply b.

			var maxLimit = 1000;
			
			var longestPrimeA = 0;
			var longestPrimeB = 0;
			var longestPrimeList = new List<long>();
			var possiblePrimeBs = Helpers.GetPrimeNumbers(maxLimit);
			
			Console.WriteLine("Possible Bs: " + string.Join(", ", possiblePrimeBs));
			
			for (var a = -maxLimit + 1; a < maxLimit; a++)
			{
				foreach (var b in possiblePrimeBs)
				{
					var n = 0;
					var primeList = new List<long>();
					
					while (true)
					{
						long result = (long)Math.Pow(n, 2) + (a * n) + b;
						
						if (Helpers.IsPrime(result))
						{
							primeList.Add(result);
						}
						else
						{
							break;
						}
						
						n++;
					}
					
					if (primeList.Count() > longestPrimeList.Count())
					{
						longestPrimeList = primeList;
						longestPrimeA = a;
						longestPrimeB = (int)b;
					}
				}
			}
			
			Console.WriteLine("Best A: " + longestPrimeA + " Best B: " + longestPrimeB + " Product: " + longestPrimeA * longestPrimeB);
			Console.WriteLine("Primes (" + longestPrimeList.Count() + "): " + string.Join(", ", longestPrimeList));
		}
		
        /// <summary>
        /// Problem 26
        /// https://projecteuler.net/problem=26
        /// </summary>
		public static void RunReciprocalCyclesProblem()
		{
			var numberWithLongestCycle = 1;
			var longestCycle = "";
			
			for (var i = 2; i < 1000; i++)
			{
				// Calculate out the repeating sequence through weird (but simple) math. Approach adapted from:
				// https://euler.stephan-brumme.com/26/
				var currentNum = (int)(1 / i);
				var currentSequence = "";
				var currentRemainder = 1 % i;
				var seenRemainders = new List<int>
				{
					currentRemainder
				};
				
				// At each step, we need both the integer result of our division (which becomes the next number in the sequence)
				// and the remainder from the division (which is multiplied by 10 and becomes the next numerator in the division).
				// When we see the same remainder again, we know we've hit the end of the repeating sequence.
				// Example for i = 7:
				// 1 / 7 = 0 (remainder 1)
				// (1 * 10) / 7 = 1 (remainder 3)
				// (3 * 10) / 7 = 4 (remainder 2)
				// (2 * 10) / 7 = 2 (remainder 6)
				// (6 * 10) / 7 = 8 (remainder 4)
				// (4 * 10) / 7 = 5 (remainder 5)
				// (5 * 10) / 7 = 7 (remainder 1)
				// Since we've see the remainder of 1 before, the sequence ends here, and our repeating pattern is: 142857
				while (true)
				{
					var nextNumerator = (currentRemainder * 10);
					
					currentNum = (int)(nextNumerator / i);
					currentRemainder = nextNumerator % i;
					currentSequence += currentNum.ToString();
					
					if (currentRemainder == 0)
						break;
					if (seenRemainders.Contains(currentRemainder))
						break;
					
					seenRemainders.Add(currentRemainder);
				}
				
				if (currentSequence.Length > 0)
				{
					//Console.WriteLine("Number: " + i + " Sequence: " + currentSequence);
					
					if (currentSequence.Length > longestCycle.Length)
					{
						longestCycle = currentSequence;
						numberWithLongestCycle = i;
					}
				}
			}
			
			Console.WriteLine("Number with Longest Repeating Sequence: " + numberWithLongestCycle);
			Console.WriteLine("Longest Repeating Sequence: " + longestCycle);
		}
		
        /// <summary>
        /// Problem 25
        /// https://projecteuler.net/problem=25
        /// </summary>
		public static void RunFibonacciDigitsProblem()
		{
			// This problem can be solved in 2 ways: by brute force and by a mathematical formula. This method will solve with both approaches.
			// Brute force approach:
			var maxNumberLength = 1000;
			BigInteger previousFibonacciNumber = 1;
			BigInteger currentFibonacciNumber = 1;
			var index = 2; // Since we started with the first 2 numbers already (1 and 1), our starting point in the loop will be 2.
			
			while (currentFibonacciNumber.ToString().Length < maxNumberLength)
			{
				var newFibonacciNumber = previousFibonacciNumber + currentFibonacciNumber;
				previousFibonacciNumber = currentFibonacciNumber;
				currentFibonacciNumber = newFibonacciNumber;
				index++;
			}
			
			Console.WriteLine("(Brute Force) Index of Fibonacci Number with " + maxNumberLength + " Digits: " + index + ", Number: " + currentFibonacciNumber);
			
			// Mathematical approach:
			// The C# formula for calculating a Fibonacci number at position `n` is:
			//     var goldenRatio = (1 + Math.Sqrt(5)) / 2;
			//     Math.Round(Math.Pow(goldenRatio, n) / Math.Sqrt(5))
			// However, using this formula to try to brute force the numbers all the way up to the 1000th digit is too much for C#'s double to
			// handle when calling Math.Pow().
			// We know that the target is to find the index of the first Fibonacci number that's greater than or equal to 10^999, so we can
			// solve for the index mathematically.
			// I have not done any math even remotely approaching this level of complexity in well over a decade, so this formula is adapted from:
			// https://acollectionofelectrons.wordpress.com/2017/06/27/1000-digit-fibonacci-number/
			var indexOfFibonacciNumber = Math.Round(((999 * Math.Log(10)) + ((double)1/2 * Math.Log(5))) / (Math.Log(1 + Math.Sqrt(5)) - Math.Log(2)));
			
			Console.WriteLine("(Mathematically) Index of Fibonacci Number with 1000 Digits: " + indexOfFibonacciNumber);
		}
		
        /// <summary>
        /// Problem 24
        /// https://projecteuler.net/problem=24
        /// </summary>
		public static void RunLexicographicalPermutationsProblem()
		{
			var stringToPermute = "0123456789";
			var indexOfDesiredLexicographicalPermutation = 1000000;
			
			var totalPermutations = 1;
			for (var i = stringToPermute.Length; i > 0; i--)
			{
				totalPermutations *= i;
			}
			
			var permutationsPerStartingCharacter = totalPermutations / stringToPermute.Length;
			Console.WriteLine("Total Permutations Available: " + totalPermutations);
			Console.WriteLine("Number of Permutations for Each Starting Character: " + permutationsPerStartingCharacter);
			
			// Now that we know the total number of permutations the string can produce and we know how many permutations each
			// starting character will generate, we can figure out mathematically which character we need to start with and only
			// find the permutations for that starting character in order to more efficiently reach the desired lexicographical
			// permutation we're after.
			var characterToStartWith = 0;
			var permutationsToTakeToReachLimit = 0;
			for (var i = 0; i < stringToPermute.Length; i++)
			{
				if (permutationsPerStartingCharacter * i > indexOfDesiredLexicographicalPermutation)
				{
					characterToStartWith = i - 1;
					permutationsToTakeToReachLimit = indexOfDesiredLexicographicalPermutation - (permutationsPerStartingCharacter * (i - 1));
					break;
				}
			}
			
			if (characterToStartWith > 0)
				stringToPermute = stringToPermute[characterToStartWith] + stringToPermute.Remove(characterToStartWith, 1);
			
			Console.WriteLine("Starting with String: '" + stringToPermute + "' and Taking " + permutationsToTakeToReachLimit + " Permutations");
			
			// Since we already know that taking JUST the permutations of the character we're starting with will be enough to get us to
			// the desired permutation, we only need to fetch the permutations that begin with that charachter and can cut out all the
			// rest of the permutations we don't need. This saves a lot of time and memory.
			var result = Helpers.GetWordPermutations(stringToPermute.Substring(1), stringToPermute[0].ToString());
			
			Console.WriteLine("Permutation at Desired Lexicographical Order Index: " + result[permutationsToTakeToReachLimit - 1]);
		}

        /// <summary>
        /// Problem 23
        /// https://projecteuler.net/problem=23
        /// </summary>
        public static void RunAbundantNumbersProblem()
        {
            var upperLimit = 28123;
            var abundantNumbers = new List<int>();
			var sumsOfAbundantNumbers = new HashSet<int>();
			var numbersNotSummableByAbundantNumbers = new HashSet<int>();

            for (var i = 1; i <= upperLimit; i++)
            {
                var factors = Helpers.GetEvenDivisors(i);

                if (factors.Sum() > i)
				{
					// Add the number to the list of abundant numbers first.
					abundantNumbers.Add(i);
					
					// Now that it's in the list, sum the number against each of the existing abundant numbers.
					// Because we added the abundant number to the list before we did this, this will include the
					// sum of the number + itself.
					foreach (var abundantNum in abundantNumbers)
					{
						var newSum = abundantNum + i;
						if (newSum > upperLimit)
							break;
						
						// The HashSet will auto-deduplicate.
						sumsOfAbundantNumbers.Add(newSum);
					}
				}
				
				if (!sumsOfAbundantNumbers.Contains(i))
					numbersNotSummableByAbundantNumbers.Add(i);
            }
			
            //Console.WriteLine("Abundant Numbers: (" + abundantNumbers.Count() + ") " + string.Join(", ", abundantNumbers));
			//Console.WriteLine("Sums of Abundant Numbers: (" + sumsOfAbundantNumbers.Count() + ") " + string.Join(", ", sumsOfAbundantNumbers));
			//Console.WriteLine("Not Summable by Abundant Numbers: (" + numbersNotSummableByAbundantNumbers.Count() + ") " + string.Join(", ", numbersNotSummableByAbundantNumbers));
			Console.WriteLine("Sum of Numbers Not Summable by Abundant Numbers: " + numbersNotSummableByAbundantNumbers.Sum());
        }

        /// <summary>
        /// Problem 22
        /// https://projecteuler.net/problem=22
        /// </summary>
        public static void RunNameScoresProblem()
        {
            var filePath = "Resources/p022_names.txt";
            var firstNames = File.ReadAllText(filePath);
            var sortedFirstNames = firstNames.Replace("\"", "").Split(',').OrderBy(name => name).ToArray();
			var allNameScores = new List<long>();

            for (var i = 0; i < sortedFirstNames.Length; i++)
            {
                var name = sortedFirstNames[i];
                var nameChars = name.ToCharArray();
                var nameCharScore = nameChars.Sum(ch => ch - 64);
				long finalNameScore = nameCharScore * (i + 1);
				
				//Console.WriteLine("Name: " + name + " Index: " + i + 1 + " Score: " + finalNameScore);
				allNameScores.Add(finalNameScore);
            }
			
			Console.WriteLine($"Total of All Name Scores: {allNameScores.Sum()}");
        }

        /// <summary>
        /// Problem 21
        /// https://projecteuler.net/problem=21
        /// </summary>
        public static void RunAmicableNumbersProblem()
        {
            var amicablePairs = new List<long>();

			for (var i = 4; i < 10000; i++)
			{
				if (amicablePairs.Contains(i))
					continue;
				
				long sumOfDivisors = Helpers.GetEvenDivisors(i).Sum();
				
				if (sumOfDivisors != i)
				{
					long sumOfSumOfDivisors = Helpers.GetEvenDivisors(sumOfDivisors).Sum();

					if (sumOfSumOfDivisors == i)
					{
						amicablePairs.Add(i);
						amicablePairs.Add(sumOfDivisors);
					}
				}
			}
			
            Console.WriteLine($"Amicable Pairs: {string.Join(", ", amicablePairs)}");
            Console.WriteLine($"Sum of Amicable Pairs: {amicablePairs.Sum()}");
        }
    }
}