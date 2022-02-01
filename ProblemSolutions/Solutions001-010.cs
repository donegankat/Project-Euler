using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ProjectEuler.ProblemSolutions
{
    public static partial class Solutions
    {
        /// <summary>
        /// Problem 10
        /// https://projecteuler.net/problem=10
        /// </summary>
        public static void RunSumOfPrimesProblem()
        {
            var upperLimit = 2000001;
            var primes = Helpers.GetPrimeNumbers(upperLimit);
            
            Console.WriteLine("Sum of primes: " + primes.Sum());
        }
        
        /// <summary>
        /// Problem 9
        /// https://projecteuler.net/problem=9
        /// </summary>
        public static void RunPythagoreanTripletProblem()
        {
            for (var a = 2; a < 500; a++)
            {
                for (var b = a + 1; b < 500; b++)
                {
                    for (var c = b + 1; c < 500; c++)
                    {
                        if ((a * a) + (b * b) == (c * c) && a + b + c == 1000)
                        {
                            Console.WriteLine("Triplet: " + a + ", " + b + ", " + c);
                            Console.WriteLine("Product: " + (a * b * c));
                            return;
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Problem 8
        /// https://projecteuler.net/problem=8
        /// </summary>
        public static void RunThousandDigitNumberProblem()
        {
            var thousandDigitNum = "7316717653133062491922511967442657474235534919493496983520312774506326239578318016984801869478851843858615607891129494954595017379583319528532088055111254069874715852386305071569329096329522744304355766896648950445244523161731856403098711121722383113622298934233803081353362766142828064444866452387493035890729629049156044077239071381051585930796086670172427121883998797908792274921901699720888093776657273330010533678812202354218097512545405947522435258490771167055601360483958644670632441572215539753697817977846174064955149290862569321978468622482839722413756570560574902614079729686524145351004748216637048440319989000889524345065854122758866688116427171479924442928230863465674813919123162824586178664583591245665294765456828489128831426076900422421902267105562632111110937054421750694165896040807198403850962455444362981230987879927244284909188845801561660979191338754992005240636899125607176060588611646710940507754100225698315520005593572972571636269561882670428252483600823257530420752963450";
            var adjacentNumberProducts = new List<long>();
            
            for (var i = 0; i < thousandDigitNum.Length - 12; i++)
            {
                long adjacentNumProduct = int.Parse(thousandDigitNum[i].ToString());
                //Console.WriteLine("Index: " + i + " Number: " + adjacentNumProduct);
                
                for (var j = 1; j < 13; j++)
                {
                    adjacentNumProduct *= int.Parse(thousandDigitNum[i + j].ToString());
                    //Console.WriteLine("Index: " + i + j + " Number: " + thousandDigitNum[i + j] + " Product: " + adjacentNumProduct);
                }
                
                adjacentNumberProducts.Add(adjacentNumProduct);
            }
            
            Console.WriteLine("Largest product of 13 adjacent numbers: " + adjacentNumberProducts.OrderByDescending(product => product).FirstOrDefault());
            Console.WriteLine(string.Join(", ", adjacentNumberProducts));
        }
        
        /// <summary>
        /// Problem 7
        /// https://projecteuler.net/problem=7
        /// </summary>
        public static void RunTenThousandAndFirstPrimeNumberProblem()
        {
            var primeNumbers = new List<long>();
            primeNumbers.Add(2);
            
            var numberToCheck = 3;
            
            while (primeNumbers.Count() < 10001)
            {
                if (Helpers.IsPrime(numberToCheck))
                    primeNumbers.Add(numberToCheck);
                
                numberToCheck += 2;
            }
            
            Console.WriteLine("10,001th prime number: " + primeNumbers.LastOrDefault());
            
            for (var i = primeNumbers.Count(); i > 0; i--)
            {
                Console.WriteLine("Index " + i + ": " + primeNumbers[i - 1]);
            }
        }

        /// <summary>
        /// Problem 6
        /// https://projecteuler.net/problem=6
        /// </summary>
        public static void RunSquareSumProblem()
        {
            long sumOfNumbers = 0;
            long sumOfSquares = 0;
            
            for (var i = 1; i <= 100; i++) {
                sumOfNumbers += i;
                sumOfSquares += (i * i);
            }
            
            Console.WriteLine("Difference of sums: " + Math.Abs((sumOfNumbers * sumOfNumbers) - sumOfSquares));
        }
        
        /// <summary>
        /// Problem 5
        /// https://projecteuler.net/problem=5
        /// </summary>
        public static void RunSmallestMultipleProblem()
        {
            var possibleMultiple = 2520;
            while (true)
            {
                for (var i = 20; i > 1; i--)
                {
                    if (possibleMultiple % i != 0)
                    {
                        possibleMultiple += 20;
                        break;
                    }
                    else if (i == 2)
                    {
                        Console.WriteLine("Multiple found: " + possibleMultiple);
                        return;
                    }
                }
            }
        }
    
		public class Palindrome
		{
			public long Product { get; set; }
			public int Factor1 { get; set; }
			public int Factor2 { get; set; }

			public Palindrome(long product, int f1, int f2)
			{
				Product = product;
				Factor1 = f1;
				Factor2 = f2;
			}
		}
		
        /// <summary>
        /// Problem 4
        /// https://projecteuler.net/problem=4
        /// </summary>
        public static void RunPalindromeProblem()
        {
            Console.WriteLine("Largest palindrome: " + GetPalindromes().OrderByDescending(palindrome => palindrome.Product).FirstOrDefault().Product);
        }
		
        private static List<Palindrome> GetPalindromes()
        {
            var palindromes = new List<Palindrome>();
            
            for (var i = 999; i > 99; i--)
            {
                for (var j = i; j > 99; j--)
                {
                    long product = i * j;

                    if (product.ToString() == string.Join("", product.ToString().Reverse()))
                    {
                        palindromes.Add(new Palindrome(product, i, j));
                    }
                }
            }
            
            return palindromes;
        }
        
		public class Factor
		{
			public long Number { get; set; }
			public bool IsPrime { get; set; }

			public Factor(long number, bool isPrime)
			{
				Number = number;
				IsPrime = isPrime;
			}
		}
		
        /// <summary>
        /// Problem 3
        /// https://projecteuler.net/problem=3
        /// </summary>
        public static void RunFactorProblem()
        {
            long number = 600851475143;
            var factors = GetFactorsAndPrimeStatus(number);
            
            Console.WriteLine("All factors: " + string.Join(", ", factors.Select(factor => factor.IsPrime ? factor.Number.ToString() + "*" : factor.Number.ToString())));
            Console.WriteLine("Largest prime factor: " + factors.OrderByDescending(factor => factor.Number).FirstOrDefault(factor => factor.IsPrime).Number);
        }
		
        private static List<Factor> GetFactorsAndPrimeStatus(long number)
        {
            var factors = new List<Factor>();
            
            while (number % 2 == 0) {
                if (!factors.Any(factor => factor.Number == 2))
                    factors.Add(new Factor(2, true));
                    
                number /= 2;
            }
            
            for (var i = 3; i < Math.Sqrt(number); i++) {
                if (number % i == 0) {
                    var factorsOfFactor = GetFactorsAndPrimeStatus(i);
                    factors.Add(new Factor(i, !factorsOfFactor.Any()));
                }
            }
            
            return factors;
        }

        /// <summary>
        /// Problem 2
        /// https://projecteuler.net/problem=2
        /// </summary>
        public static void RunEvenFibonacciNumbersProblem()
        {
            var evenValuedFibonacciNumbers = new HashSet<int>();
            var currentFibonacci = 2;
            var previousFibonacci = 1;

            // Add the first even-valued Fibonacci.
            evenValuedFibonacciNumbers.Add(currentFibonacci);

            while (currentFibonacci < 4000000)
            {
                var newCurrentFibonacci = currentFibonacci + previousFibonacci;
                previousFibonacci = currentFibonacci;
                currentFibonacci = newCurrentFibonacci;

                if (newCurrentFibonacci % 2 == 0)
                    evenValuedFibonacciNumbers.Add(newCurrentFibonacci);
            }

            Console.WriteLine($"Even Fibonaccis: {string.Join(", ", evenValuedFibonacciNumbers)}");
            Console.WriteLine($"Sum of Even Fibonaccis: {evenValuedFibonacciNumbers.Sum()}");
        }

        /// <summary>
        /// Problem 1
        /// https://projecteuler.net/problem=1
        /// </summary>
        public static void RunMultiplesOf3and5Problem()
        {
            var multiplesOf3and5 = new HashSet<int>();

            for (var i = 3; i < 1000; i++)
            {
                if (i % 3 != 0 && i % 5 != 0)
                    continue;

                multiplesOf3and5.Add(i);
            }

            Console.WriteLine($"Multiples of 3 and 5: {string.Join(", ", multiplesOf3and5)}");
            Console.WriteLine($"Sum of Multiples of 3 and 5: {multiplesOf3and5.Sum()}");
        }
    }
}