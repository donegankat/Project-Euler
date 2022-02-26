using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ProjectEuler.ProblemSolutions
{
    public static partial class Solutions
    {
        /// <summary>
        /// Problem 60
        /// https://projecteuler.net/problem=60
        /// </summary>
        public static void RunConcatenatedPrimePairSetsProblem()
        {
            // This problem's solution is far from my best work, but I'm just so tired of looking at it.


            var primes = Helpers.GetPrimeNumbers(10000);
            var primesThatConcatenateToOtherPrimes = new Dictionary<long, HashSet<long>>();

            foreach (var prime in primes)
            {
                foreach (var primeToConcatenate in primes.Where(p => p != prime))
                {
                    if (Helpers.IsPrime(long.Parse($"{prime}{primeToConcatenate}")) && Helpers.IsPrime(long.Parse($"{primeToConcatenate}{prime}")))
                    {
                        if (!primesThatConcatenateToOtherPrimes.ContainsKey(prime))
                            primesThatConcatenateToOtherPrimes.Add(prime, new HashSet<long>{ prime, primeToConcatenate });
                        else
                            primesThatConcatenateToOtherPrimes[prime].Add(primeToConcatenate);
                    }
                }
            }
            
            var targetChainLength = 5;

            foreach (var prime in primesThatConcatenateToOtherPrimes)
            {
                foreach (var concatenatablePrime in prime.Value.Where(p => p != prime.Key))
                {
                    var concatenatedPrime = primesThatConcatenateToOtherPrimes[concatenatablePrime];
                    var primesInCommon = prime.Value.Intersect(concatenatedPrime);

                    var primeChain = FindConcatenatablePrimesInCommon(
                        primesThatConcatenateToOtherPrimes,
                        primesThatConcatenateToOtherPrimes.FirstOrDefault(p => p.Key == concatenatablePrime),
                        primesInCommon,
                        targetChainLength,
                        2 // Begin at depth 2 because we've already looked at both the original number and 1 other concatenatable number.
                    );

                    if (primeChain.Count() >= targetChainLength)
                    {
                        Console.WriteLine($"Final Intersection: {primeChain.Sum()}: {string.Join(", ", primeChain)}");
                        return;
                    }
                    else
                        continue;
                }
            }
        }

        private static IEnumerable<long> FindConcatenatablePrimesInCommon(
            Dictionary<long, HashSet<long>> allConcatenatablePrimes,
            KeyValuePair<long, HashSet<long>> currentConcatenatablePrime,
            IEnumerable<long> allPrimesInCommon,
            int maxChainLength,
            int currentDepth
        )
        {
            var clonedAllPrimesInCommon = new HashSet<long>(allPrimesInCommon);
            var newAllPrimesInCommon = clonedAllPrimesInCommon.Intersect(currentConcatenatablePrime.Value);

            if (currentDepth > maxChainLength || newAllPrimesInCommon.Count() < maxChainLength)
                return newAllPrimesInCommon;

            foreach (var commonPrime in newAllPrimesInCommon.Where(p => p > currentConcatenatablePrime.Key))
            {
                var concatenatedPrime = allConcatenatablePrimes.FirstOrDefault(p => p.Key == commonPrime);

                // No reason to continue recursing if our chain is already shorter than what we want.
                if (newAllPrimesInCommon.Count() >= maxChainLength)
                {
                    return FindConcatenatablePrimesInCommon(
                        allConcatenatablePrimes,
                        concatenatedPrime,
                        newAllPrimesInCommon,
                        maxChainLength,
                        currentDepth + 1
                    );
                }
            }

            if (currentDepth >= maxChainLength && newAllPrimesInCommon.Count() >= maxChainLength)
            {
                //Console.WriteLine($"Returning from Depth {currentDepth}: {string.Join(", ", newAllPrimesInCommon)}");
                return newAllPrimesInCommon.ToHashSet();
            }

            return new HashSet<long>();
        }

        /// <summary>
        /// Problem 59
        /// https://projecteuler.net/problem=59
        /// </summary>
        public static void RunXorDecryptionProblem()
        {
            var filePath = "Resources/p059_cipher.txt";
            var allEncryptedCharacters = File.ReadAllText(filePath).Trim().Split(",").Select(str => int.Parse(str)).ToArray();

            // Start at lower-cased 'a' (ASCII 97) and check through lower-cased 'z' (ASCII 122).
            for (var i = 97; i <= 122; i++)
            {
                for (var j = 97; j <= 122; j++)
                {
                    for (var k = 97; k <= 122; k++)
                    {
                        var password = new int[3] { i, j, k };
                        var repeatedPasswordKey = Enumerable
                              .Range(0, allEncryptedCharacters.Length)
                              .Select(x => password[x % password.Length]);
                        
                        var attemptedDecryption = allEncryptedCharacters.Zip(repeatedPasswordKey, (encrypted, keyChar) => (keyChar ^ encrypted)).ToArray();
                        var isDecryptionAttemptValid = attemptedDecryption.All(decrypted => (decrypted >= 32 && decrypted <= 93) || (decrypted >= 97 && decrypted <= 122));
                        
                        if (isDecryptionAttemptValid)
                        {
                            Console.WriteLine($"Successful Password: {(char)i}{(char)j}{(char)k}");
                            Console.WriteLine($"Decrypted File: {string.Join("", attemptedDecryption.Select(decrypted => (char)decrypted))}");
                            Console.WriteLine($"Sum of Decrypted ASCII Characters: {attemptedDecryption.Sum()}");
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Problem 58
        /// https://projecteuler.net/problem=58
        /// </summary>
        public static void RunSpiralPrimesProblem()
        {
            // This problem can be solved mathematically. The bottom right corner of each layer of the spiral
			// is the length of the spiral layer squared, so a 3x3 spiral's bottom right number is 9, a 5x5's
			// bottom right corner is 25, and so on.
			// The bottom left corner is the bottom right corner - (the length of the spiral layer - 1), so a 3x3
			// spiral's bottom left corner is 7, a 5x5's bottom left corner is 21, and so on.
			// The top left corner follows the same pattern, where it's the bottom left corner - (the height
			// of the spiral layer - 1), so 3x3 = 5, 5x5 = 17, and so on.
			// Same goes for the bottom right corner.
			var totalDiagonalPrimes = 0;
			var totalDiagonalNumbers = 1; // Count the 1 in the center, but it's not considered prime.
			var spiralSize = 1;
            do
            {
                spiralSize += 2; // Increment by 2 because we're adding both a width and a height.

                long bottomRightCorner = (long)Math.Pow(spiralSize, 2);
				long bottomLeftCorner = bottomRightCorner - spiralSize + 1;
				long topLeftCorner = bottomLeftCorner - spiralSize + 1;
				long topRightCorner = topLeftCorner - spiralSize + 1;

                if (Helpers.IsPrime(bottomRightCorner)) totalDiagonalPrimes++;
                if (Helpers.IsPrime(bottomLeftCorner)) totalDiagonalPrimes++;
                if (Helpers.IsPrime(topLeftCorner)) totalDiagonalPrimes++;
                if (Helpers.IsPrime(topRightCorner)) totalDiagonalPrimes++;

                totalDiagonalNumbers += 4;
            }
            while (((double)totalDiagonalPrimes / (double)totalDiagonalNumbers) * 100 >= 10);
			
            Console.WriteLine($"Spiral Side Length when Diagonal Prime Ratio Falls Below 10%: {spiralSize}");
        }

        /// <summary>
        /// Problem 57
        /// https://projecteuler.net/problem=57
        /// </summary>
        public static void RunSquareRootConvergentsProblem()
        {
            var numberOfFractionsWithMoreDigitsInNumerator = 0;

            // We can use the following formulas to find the fraction numerators and denominators:
            // Next numerator = current numerator + 2 * current denominator
            // Next denominator = current numerator + current denominator
            BigInteger numerator = 3;
            BigInteger denominator = 2;

            for (var i = 1; i < 1000; i++)
            {
                if (numerator.ToString().Length > denominator.ToString().Length)
                    numberOfFractionsWithMoreDigitsInNumerator++;

                BigInteger nextNumerator = numerator + 2 * denominator;
                denominator = numerator + denominator;
                numerator = nextNumerator;
            }

            Console.WriteLine($"Number of Fractions with More Numerator Digits: {numberOfFractionsWithMoreDigitsInNumerator}");
        }

        /// <summary>
        /// Problem 56
        /// https://projecteuler.net/problem=56
        /// </summary>
        public static void RunPowerfulDigitSumProblem()
        {
            BigInteger maxDigitSum = 0;

            for (var i = 1; i < 100; i++)
            {
                for (var j = 1; j < 100; j++)
                {
                    var pow = BigInteger.Pow(i, j);
                    BigInteger sumOfDigits = pow.ToString().ToCharArray().Sum(c => int.Parse(c.ToString()));

                    if (sumOfDigits > maxDigitSum)
                        maxDigitSum = sumOfDigits;
                }
            }

            Console.WriteLine($"Max Sum of Digits: {maxDigitSum}");
        }

        /// <summary>
        /// Problem 55
        /// https://projecteuler.net/problem=55
        /// </summary>
        public static void RunLychrelNumbersProblem()
        {
            var lychrelNumbers = new HashSet<int>();

            for (var i = 10; i < 10000; i++)
            {
                var isPalindromic = false;
                BigInteger currentNumber = i;

                for (var j = 0; j < 50; j++)
                {
                    BigInteger potentialPalindrome = currentNumber + BigInteger.Parse(string.Join("", currentNumber.ToString().Reverse()));
                    currentNumber = potentialPalindrome;

                    if (potentialPalindrome.ToString() == string.Join("", potentialPalindrome.ToString().Reverse()))
                    {
                        isPalindromic = true;
                        break;
                    }
                }

                if (!isPalindromic)
                    lychrelNumbers.Add(i);
            }

            Console.WriteLine($"Count of Lychrel Numbers: {lychrelNumbers.Count()}");
        }

        private class Card
        {
            public string Suit { get; set; }
            public int Rank { get; set; }

            public Card(string card)
            {
                var suitAndRank = card.ToCharArray();
                Rank = EvaluateRank(suitAndRank[0]);
                Suit = suitAndRank[1].ToString();
            }

            private int EvaluateRank(char rank)
            {
                if (int.TryParse(rank.ToString(), out int intRank))
                    return intRank;
                
                switch (rank)
                {
                    case 'T':
                        return 10;
                    case 'J':
                        return 11;
                    case 'Q':
                        return 12;
                    case 'K':
                        return 13;
                    case 'A':
                        return 14;
                }

                return 0;
            }
        }

        private class Hand
        {
            public IEnumerable<Card> Cards { get; set; }

            public HandResult Result { get; set; }

            public int? HandResultTieComparison { get; set; }

            public int HighestCardRank { get; set; }

            public Hand(IEnumerable<string> cardStrings)
            {
                Cards = cardStrings.Select(c => new Card(c)).OrderBy(c => c.Rank);
                HighestCardRank = Cards.LastOrDefault().Rank;
            }

            public void Evaluate()
            {
                var isFlush = Cards.GroupBy(c => c.Suit).Count() == 1;
                var isStraight = Cards.ElementAt(1).Rank == Cards.ElementAt(0).Rank + 1 &&
                        Cards.ElementAt(2).Rank == Cards.ElementAt(1).Rank + 1 &&
                        Cards.ElementAt(3).Rank == Cards.ElementAt(2).Rank + 1 &&
                        Cards.ElementAt(4).Rank == Cards.ElementAt(3).Rank + 1;

                if (isFlush)
                {
                    if (Cards.Any(c => c.Rank == 10) &&
                        Cards.Any(c => c.Rank == 11) &&
                        Cards.Any(c => c.Rank == 12) &&
                        Cards.Any(c => c.Rank == 13) &&
                        Cards.Any(c => c.Rank == 14))
                    {
                        Result = HandResult.RoyalFlush;
                        HandResultTieComparison = Cards.ElementAt(4).Rank;
                        return; // Nothing can supercede this result, so just return.
                    }

                    if (isStraight)
                    {
                        Result = HandResult.StraightFlush;
                        HandResultTieComparison = Cards.ElementAt(4).Rank;
                        return; // Nothing can supercede this result, so just return.
                    }
                }

                var rankGroups = Cards.GroupBy(c => c.Rank);
                var has3OfAKind = false;
                var hasOnePair = false;
                var hasTwoPairs = false;

                foreach (var rankGroup in rankGroups)
                {
                    if (rankGroup.Count() == 4)
                    {
                        Result = HandResult.FourOfAKind;
                        HandResultTieComparison = rankGroup.FirstOrDefault().Rank;
                        return; // Nothing can supercede this result, so just return.
                    }

                    if (rankGroup.Count() == 3)
                    {
                        // This result can be superceded. Don't return yet.
                        has3OfAKind = true;

                        if (HandResultTieComparison == null || HandResultTieComparison < rankGroup.FirstOrDefault().Rank)
                            HandResultTieComparison = rankGroup.FirstOrDefault().Rank;
                    }

                    if (rankGroup.Count() == 2)
                    {
                        if (hasOnePair)
                            hasTwoPairs = true;

                        // This result can be superceded. Don't return yet.
                        hasOnePair = true;

                        if (HandResultTieComparison == null || HandResultTieComparison < rankGroup.FirstOrDefault().Rank)
                            HandResultTieComparison = rankGroup.FirstOrDefault().Rank;
                    }
                }

                if (has3OfAKind && hasOnePair)
                {
                    Result = HandResult.FullHouse;
                    return;
                }

                if (isFlush)
                {
                    Result = HandResult.Flush;
                    HandResultTieComparison = Cards.LastOrDefault().Rank;
                    return;
                }

                if (isStraight)
                {
                    Result = HandResult.Straight;
                    HandResultTieComparison = Cards.LastOrDefault().Rank;
                    return;
                }

                if (has3OfAKind)
                {
                    Result = HandResult.ThreeOfAKind;
                    return;
                }

                if (hasTwoPairs)
                {
                    Result = HandResult.TwoPairs;
                    return;
                }

                if (hasOnePair)
                {
                    Result = HandResult.OnePair;
                    return;
                }

                Result = HandResult.HighCard;
                HandResultTieComparison = Cards.LastOrDefault().Rank;
            }
        }

        private enum HandResult
        {
            // Highest value card.
            HighCard,

            // Two cards of the same value.
            OnePair,

            // Two different pairs.
            TwoPairs,

            // Three cards of the same value.
            ThreeOfAKind,

            // All cards are consecutive values.
            Straight,
            
            // All cards of the same suit.
            Flush,

            // Three of a kind and a pair.
            FullHouse,

            // Four cards of the same value.
            FourOfAKind,
            
            // All cards are consecutive values of same suit.
            StraightFlush,
            
            // Ten, Jack, Queen, King, Ace, in same suit.
            RoyalFlush
        }

        /// <summary>
        /// Problem 54
        /// https://projecteuler.net/problem=54
        /// </summary>
        public static void RunPokerHandsProblem()
        {
            var filePath = "Resources/p054_poker.txt";
            var allPokerHands = File.ReadAllText(filePath).Trim().Split("\n");
            var handsWonByPlayer1 = 0;

            foreach (var hand in allPokerHands)
            {
                var cardsInHands = hand.Split(" ");
                var player1Hand = new Hand(cardsInHands.Take(5));
                var player2Hand = new Hand(cardsInHands.Skip(5).Take(5));

                player1Hand.Evaluate();
                player2Hand.Evaluate();

                if (player1Hand.Result > player2Hand.Result)
                    handsWonByPlayer1++;
                else if (player1Hand.Result == player2Hand.Result)
                {
                    if (player1Hand.HandResultTieComparison > player2Hand.HandResultTieComparison)
                        handsWonByPlayer1++;
                    else if (player1Hand.HandResultTieComparison == player2Hand.HandResultTieComparison)
                    {
                        if (player1Hand.HighestCardRank > player2Hand.HighestCardRank)
                            handsWonByPlayer1++;
                    }
                }
            }

            Console.WriteLine($"Hands Won by Player 1: {handsWonByPlayer1}");
        }

        /// <summary>
        /// Problem 53
        /// https://projecteuler.net/problem=53
        /// </summary>
        public static void RunCombinatoricSelectionsProblem()
        {
            var combinatoricSelectionsGreaterThan1Mil = 0;
            for (var i = 23; i <= 100; i++)
            {
                for (var j = 1; j <= i; j++)
                {
                    var iFactorial = Helpers.GetFactorial(i);
                    var jFactorial = Helpers.GetFactorial(j);
                    var diffFactorial = Helpers.GetFactorial(i - j);
                    var possibleCombinations = iFactorial / (jFactorial * diffFactorial);

                    if (possibleCombinations > 1000000)
                        combinatoricSelectionsGreaterThan1Mil++;
                }
            }

            Console.WriteLine($"Combinatoric Selectors with More Than 1 Million Combinations: {combinatoricSelectionsGreaterThan1Mil}");
        }

        /// <summary>
        /// Problem 52
        /// https://projecteuler.net/problem=52
        /// </summary>
        public static void RunPermutedMultiplesProblem()
        {
            var i = 1;
            while (true)
            {
                var iString = i.ToString();
                var areAllMultiplesPermutations = true;

                for (var j = 2; j <= 6; j++)
                {
                    var product = i * j;

                    if (iString.Length != product.ToString().Length)
                    {
                        areAllMultiplesPermutations = false;
                        break;
                    }

                    foreach (var digit in iString.ToCharArray())
                    {
                        if (product.ToString().IndexOf(digit) < 0)
                        {
                            areAllMultiplesPermutations = false;
                            break;
                        }
                    }

                    if (!areAllMultiplesPermutations)
                        break;
                }

                if (areAllMultiplesPermutations)
                {
                    Console.WriteLine($"Smallest Number with Permuted Multiples: {i}");
                    return;
                }

                i++;
            }
        }

        /// <summary>
        /// Problem 51
        /// https://projecteuler.net/problem=51
        /// </summary>
        public static void RunPrimeNumberFamilyProblem()
        {
            var upperLimit = 1000000;
            
            var primes = Helpers.GetPrimeNumbers(upperLimit);
            var primesToSearch = primes.Where(prime => prime > 10000).ToHashSet();

            foreach (var prime in primesToSearch)
            {
                var primeString = prime.ToString();
                
                foreach (var primeDigit in primeString.ToCharArray().Distinct())
                {
                    var primeFamily = new HashSet<long> {
                        prime
                    };

                    for (var i = 0; i <= 9; i++)
                    {
                        var newPrime = long.Parse(primeString.Replace(primeDigit.ToString(), i.ToString()));
                        if (primesToSearch.Contains(newPrime))
                        {
                            primeFamily.Add(newPrime);
                        }
                    }

                    if (primeFamily.Count() > 7)
                    {
                        Console.WriteLine($"Prime Family: {string.Join(", ", primeFamily)}");
                        return;
                    }
                }
            }
        }
    }
}