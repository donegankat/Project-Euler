using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ProjectEuler.ProblemSolutions
{
    public static partial class Solutions
    {
		/// <summary>
        /// Problem 40
        /// https://projecteuler.net/problem=40
        /// </summary>
		public static void RunChampernownesConstantDecimalConcatenationProblem()
		{
			var decimalString = ".";
			var nextDigitToFind = 1;
			var totalProduct = 1;

			// Find: d1 × d10 × d100 × d1000 × d10000 × d100000 × d1000000
			// There are WAY more efficient and intelligent ways to solve this than via brute force,
			// but honestly I'm just so tired right now that I don't care. I can wait the few mins it
			// takes for this approach to run.
			// See something like this if you actually care about solving this problem correctly:
			// https://blog.dreamshire.com/solutions/project_euler/project-euler-problem-040-solution/
			for (var i = 1; i <= 1000000; i++)
			{
				decimalString += i.ToString();

				if (decimalString.Length > nextDigitToFind)
				{
					totalProduct *= int.Parse(decimalString[nextDigitToFind].ToString());
					nextDigitToFind *= 10;

					if (nextDigitToFind > 1000000)
						break;
				}
			}

			Console.WriteLine($"Total Product: {totalProduct}");
		}
		
		public class Triangle
		{
			public int A { get; set; }
			public int B { get; set; }
			public int C { get; set; }
			public int Perimeter { get; set; }

			public Triangle(int a, int b, int c)
			{
				A = a;
				B = b;
				C = c;
				Perimeter = a + b + c;
			}

			public Triangle(int a, int b, int c, int perimeter)
			{
				A = a;
				B = b;
				C = c;
				Perimeter = perimeter;
			}

			public override string ToString()
			{
				return $"[{A}, {B}, {C} | {Perimeter}]";
			}
		}

		/// <summary>
        /// Problem 39
        /// https://projecteuler.net/problem=39
        /// </summary>
		public static void RunIntegerRightTrianglesProblem()
		{
			var rightTriangles = new List<Triangle>();

			// The smallest right triangle is {3, 4, 5}
			for (var a = 3; a < 500; a++)
			{
				for (var b = a + 1; b < 500; b++)
				{
					var c = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

					if (c % 1 == 0)
					{
						var perimeter = a + b + (int)c;

						if (perimeter <= 1000)
							rightTriangles.Add(new Triangle(a, b, (int)c, perimeter));
						else
							break;
					}
				}
			}

			Console.WriteLine($"Right Triangles: {string.Join("\n", rightTriangles.OrderBy(triangle => triangle.Perimeter).Select(triangle => triangle.ToString()))}");
			Console.WriteLine($"Perimeter with Most Solutions: {rightTriangles.GroupBy(triangle => triangle.Perimeter).OrderByDescending(triangleGroup => triangleGroup.Count()).FirstOrDefault().Key}");
		}

		/// <summary>
        /// Problem 38
        /// https://projecteuler.net/problem=38
        /// </summary>
		public static void RunPandigitalMultiplesProblem()
		{
			// 987654321
			var largestPandigitalStartingNumber = 0;
			var largestPandigitalConcatenation = 0;

			for (var i = 1; i < 100000; i++)
			{
				var concatenatedString = new StringBuilder();

				for (var j = 1; j <= 9; j++)
				{
					concatenatedString.Append((i * j).ToString());

					if (concatenatedString.Length >= 9)
						break;
				}

				if (concatenatedString.Length > 9)
					continue;

				var parsedConcatenation = int.Parse(concatenatedString.ToString());

				if (Helpers.IsPandigital(parsedConcatenation) && parsedConcatenation > largestPandigitalStartingNumber)
				{
					largestPandigitalStartingNumber = i;
					largestPandigitalConcatenation = parsedConcatenation;
				}
			}

			Console.WriteLine($"Largest Pandigital Number to Concatenate: {largestPandigitalStartingNumber}");
			Console.WriteLine($"Largest Concatenated Pandigital Number: {largestPandigitalConcatenation}");
		}

        /// <summary>
        /// Problem 37
        /// https://projecteuler.net/problem=37
        /// </summary>
        public static void RunTruncatablePrimesProblem()
		{
			var truncatablePrimes = new HashSet<long>();
			var primes = Helpers.GetPrimeNumbers(1000000);
			
			foreach (var prime in primes.Where(p => p > 10))
			{
				var primeStr = prime.ToString();
				var isTruncatablePrime = true;
				
				for (var i = 0; i < primeStr.Length; i++)
				{
					if (!primes.Contains(long.Parse(primeStr.Substring(i))) || !primes.Contains(long.Parse(primeStr.Substring(0, primeStr.Length - i))))
					{
						isTruncatablePrime = false;
						break;
					}
				}
				
				if (isTruncatablePrime)
					truncatablePrimes.Add(prime);
				
				if (truncatablePrimes.Count() >= 11)
					break;
			}
			
			Console.WriteLine("Truncatable Primes: " + string.Join(", ", truncatablePrimes));
			Console.WriteLine("Sum of Truncatable Primes: " + truncatablePrimes.Sum());
		}
		
        /// <summary>
        /// Problem 36
        /// https://projecteuler.net/problem=36
        /// </summary>
		public static void RunPalindromicBinaryNumbersProblem()
		{
			var palindromicBinaryNumbers = new HashSet<int>();
			
			for (var i = 0; i < 1000000; i++)
			{
				var binaryBase2 = Convert.ToString(i, 2);
				var binaryBase10 = Convert.ToString(i, 10);
				
				if (binaryBase2 == string.Join("", binaryBase2.Reverse()) && binaryBase10 == string.Join("", binaryBase10.Reverse()))
				{
					palindromicBinaryNumbers.Add(i);
				}
			}
			
			Console.WriteLine("Palindromic Numbers: " + string.Join(", ", palindromicBinaryNumbers));
			Console.WriteLine("Sum of Palindromic Numbers: " + palindromicBinaryNumbers.Sum());
		}
		
        /// <summary>
        /// Problem 35
        /// https://projecteuler.net/problem=35
        /// </summary>
		public static void RunCircularPrimesProblem()
		{
			var maxLimit = 1000000;
			var primes = Helpers.GetPrimeNumbers(maxLimit);
			var circularPrimes = new HashSet<long>();
			
			foreach (var prime in primes)
			{
				var primeStr = prime.ToString();
				var allRotationsArePrime = true;
				var rotations = new List<long>();
				
				// We may have already added this prime to the list when we found all of a previous prime's rotations.
				if (circularPrimes.Contains(prime))
					continue;
				
				for (var i = 0; i < primeStr.Length; i++)
				{
					var rotatedPrime = long.Parse(primeStr.Substring(i) + primeStr.Substring(0, i));
					
					if (!primes.Contains(rotatedPrime))
					{
						allRotationsArePrime = false;
						break;
					}
					
					rotations.Add(rotatedPrime);
				}
				
				if (allRotationsArePrime)
				{
					foreach (var rotation in rotations)
					{
						circularPrimes.Add(rotation);
					}
				}
			}
			
			//Console.WriteLine("Primes Below Limit: " + string.Join(", ", primes));
			Console.WriteLine("Circular Primes Below Limit: " + string.Join(", ", circularPrimes));
			Console.WriteLine("Count of Circular Primes Below Limit: " + circularPrimes.Count());
		}
		
        /// <summary>
        /// Problem 34
        /// https://projecteuler.net/problem=34
        /// </summary>
		public static void RunSumOfNumbersWithFactorializedDigitsProblem()
		{
			var numbersSummableByTheFactorialsOfTheirDigits = new List<int>();
			var basicFactorials = new List<int>
			{
				1 // 0! = 1
			};

			for (var i = 1; i <= 9; i++)
			{
				basicFactorials.Add(basicFactorials[i - 1] * i);
			}

			Console.WriteLine("Basic Factorials: " + string.Join(", ", basicFactorials));
			
			// Using the formula 10^(d-1) <= n < 10^d, we can find that n can be no more than 7 digits.
			// However, 9! * 7 = 2,540,160, so we know the upper limit is no greater than that.
			// See https://www.xarg.org/puzzle/project-euler/problem-34/
			// The site linked above technically reduces the max limit even further, but it felt like cheating
			// for me to use too much of their mathematical simplification since I didn't figure it out myself.
			for (int i = 10; i <= 2540160; i++)
			{
				int sum = 0;
				
				foreach (var digit in i.ToString().ToCharArray())
				{
					sum += basicFactorials[int.Parse(digit.ToString())];
				}
				
				if (sum == i)
					numbersSummableByTheFactorialsOfTheirDigits.Add(i);
			}
			
			Console.WriteLine("Numbers Summable by the Factorials of Their Digits: " + string.Join(", ", numbersSummableByTheFactorialsOfTheirDigits));
			Console.WriteLine("Sum: " + numbersSummableByTheFactorialsOfTheirDigits.Sum());
		}

        /// <summary>
        /// Problem 33
        /// https://projecteuler.net/problem=33
        /// </summary>
		public static void RunDigitCancellingFractionsProblem()
		{
			var digitCancellingFractions = new List<Tuple<double, double>>();
			for (double denominator = 99; denominator >= 10; denominator--)
			{
				
				for (double numerator = denominator - 1; numerator >= 10; numerator--)
				{
					if (denominator == numerator)
						continue;
					
					if ((denominator % 10 == 0 && numerator % 10 == 0))
						continue;
					
					var denominatorStr = denominator.ToString();
					var numeratorStr = numerator.ToString();
					
					var matchingDigit = denominatorStr.ToCharArray().FirstOrDefault(iChar => numeratorStr.ToCharArray().Contains(iChar)).ToString();

					// No shared digits, so they can't cancel out.
					if (matchingDigit == null)
						continue;
					
					double startingResult = numerator / denominator;
					
					var numeratorMatchIndex = numeratorStr.IndexOf(matchingDigit);
					var denominatorMatchIndex = denominatorStr.IndexOf(matchingDigit);
					
					if (numeratorMatchIndex < 0 || denominatorMatchIndex < 0)
						continue;
					
					var cancelledNumerator = double.Parse(numeratorStr.Remove(numeratorMatchIndex, 1));
					var cancelledDenominator = double.Parse(denominatorStr.Remove(denominatorMatchIndex, 1));
					
					if (cancelledNumerator == 0 || cancelledDenominator == 0)
						continue;
					
					double cancelledResult = cancelledNumerator / cancelledDenominator;
					
					//Console.WriteLine(string.Format("Checking: {0} / {1} = {2} | {3} / {4} = {5} | Cancelled digit: {6}", numerator, denominator, startingResult, cancelledNumerator, cancelledDenominator, cancelledResult, matchingDigit));

					if (startingResult == cancelledResult)
					{
						Console.WriteLine(string.Format("FOUND: {0} / {1} = {2} / {3} == {4} | Cancelled digit: {5}", numerator, denominator, cancelledNumerator, cancelledDenominator, startingResult, matchingDigit));
						digitCancellingFractions.Add(new Tuple<double, double>(numerator, denominator));
					}
				}
			}
			
			Console.WriteLine();
			Console.WriteLine("Cancelled Digit Fractions:\n" + string.Join("\n", digitCancellingFractions.Select(dcf => dcf.Item1 + " / " + dcf.Item2)));
			Console.WriteLine();
			
			double productOfProducts = digitCancellingFractions.Aggregate((double)1, (total, next) => total *= (next.Item1 / next.Item2));
			Console.WriteLine("Product of Products: " + productOfProducts);
			Console.WriteLine("Lowest common term fraction: 1 / " + 1 / productOfProducts);
		}
		
		private class PandigitalProduct
		{
			public int Multiplicand;
			public int Multiplier;
			public int Product;
			
			public PandigitalProduct(int multiplicand, int multiplier, int product)
			{
				Multiplicand = multiplicand;
				Multiplier = multiplier;
				Product = product;
			}
			
			public static bool IsPandigitalProduct(int multiplicand, int multiplier, int product)
			{
				var charsToLookFor = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
				var strMultiplicand = multiplicand.ToString();
				var strMultiplier = multiplier.ToString();
				var strProduct = product.ToString();
				
				foreach (var num in strProduct.ToCharArray())
				{
					if (!charsToLookFor.Contains(num))
						return false;
					
					charsToLookFor.Remove(num);
				}
				
				foreach (var num in strMultiplier.ToCharArray())
				{
					if (!charsToLookFor.Contains(num))
						return false;
					
					charsToLookFor.Remove(num);
				}
				
				foreach (var num in strMultiplicand.ToCharArray())
				{
					if (!charsToLookFor.Contains(num))
						return false;
					
					charsToLookFor.Remove(num);
				}
				
				return charsToLookFor.Count == 0;
			}
		}
		
        /// <summary>
        /// Problem 32
        /// https://projecteuler.net/problem=32
        /// </summary>
		public static void RunPandigitalProductsProblem()
		{
			var pandigitalProducts = new List<PandigitalProduct>();
			
			for (var i = 1; i < 99; i++)
			{
				for (var j = 2; j < 98765432; j++)
				{
					var product = i * j;
					
					if (PandigitalProduct.IsPandigitalProduct(i, j, product) && !pandigitalProducts.Any(prod => prod.Product == product))
					{
						pandigitalProducts.Add(new PandigitalProduct(i, j, product));
					}
					else if (product.ToString().Length + j.ToString().Length + i.ToString().Length > 9)
						// If we're already longer than we possibly can be to be pandigital, break this loop and continue to the next iteration of i.
						break;
				}
			}
			
			Console.WriteLine("Pandigital Products:\n" + string.Join("\n", pandigitalProducts.Select(prod => prod.Multiplicand + " x " + prod.Multiplier + " = " + prod.Product)));
			Console.WriteLine("Sum of Pandigital Products: " + pandigitalProducts.Sum(prod => prod.Product));
		}
		
        /// <summary>
        /// Problem 31
        /// https://projecteuler.net/problem=31
        /// </summary>
		public static void RunCoinSumsProblem()
		{
			// 1p, 2p, 5p, 10p, 20p, 50p, £1 (100p), and £2 (200p).
			var coins = new List<int> { 1, 2, 5, 10, 20, 50, 100, 200 };
			
			var twoPounds = 200;
			var numberOfWaysToMakeTwoPounds = 0;
			
			numberOfWaysToMakeTwoPounds += AddCoin(coins, twoPounds, coins.Count() - 1, new List<int>());
			
			Console.WriteLine("Number of Ways to Make 2 Pounds: " + numberOfWaysToMakeTwoPounds);
		}
		
		private static int AddCoin(List<int> coins, int currentSum, int coinIndex, List<int> coinsAddedSoFar)
		{
			if (currentSum == 0)
			{
				//Console.WriteLine("Found: " + string.Join(", ", coinCombo));
				return 1;
			}
			if (currentSum < 0)
				return 0;
			
			if (coinIndex < 0)
				return 0;
			
			// Lists pass by reference so we have to make a copy.
			// We really only keep track of this list for debug purposes.
			var coinsAddedSoFarUnchanged = new List<int>();
			coinsAddedSoFarUnchanged.AddRange(coinsAddedSoFar);
			
			var coinsAddedSoFarPlusThisCoin = new List<int>();
			coinsAddedSoFarPlusThisCoin.AddRange(coinsAddedSoFar);
			coinsAddedSoFarPlusThisCoin.Add(coins[coinIndex]);
			
			return AddCoin(coins, currentSum - coins[coinIndex], coinIndex, coinsAddedSoFarPlusThisCoin) + AddCoin(coins, currentSum, coinIndex - 1, coinsAddedSoFarUnchanged);
		}
    }
}