using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace AdventCode2019
{
    [TestClass]
    public class Day22
    {
        string [] input = Utils.StringsFromFile("day22.txt").ToArray();

        [TestMethod]
        public void Problem1()
        {
            int count = 10007;

            var deck = Enumerable.Range(0, count);

            //var input = new string[]
            //{
            //   "deal into new stack      ",
            //   "cut -2                   ",
            //   "deal with increment 7    ",
            //   "cut 8                    ",
            //   "cut -4                   ",
            //   "deal with increment 7    ",
            //   "cut 3                    ",
            //   "deal with increment 9    ",
            //   "deal with increment 3    ",
            //   "cut -1                   ",
            //};

            deck = Shuffle(deck);

            int result = deck.ToList().IndexOf(2019);

            Assert.AreEqual(result, 7545);
        }

        [TestMethod]
        public void Problem2()
        {



            long deckSize = 119315717514047;
            long cardIndex = 2020;
            long repeats  = 101741582076661;
            long result = 0;

            // Can find what index leads to 2020 using reverse index x' = (a.x + b)
            // but need to repeat n times!
            // y = (a.x + b)^n
            // iterated functions: https://en.wikipedia.org/wiki/Iterated_function
            // f(a.x + b)^n = a^n.x + b . (a^n - 1) / (a - 1) 
            // And as using modulus math, that divide is ModInv

            (var a, var b) = CalculateReverseIndexFormula(deckSize);

            var modpow = BigInteger.ModPow(a, repeats, deckSize);
            var bDash = b * (modpow - 1) * ModInv((long) a - 1, deckSize); // divide in modulus!
            var aDash = modpow;

            result = (long)((aDash * cardIndex + bDash) % deckSize);

            Assert.AreEqual(result, 12706692375144);
        }

        //// Too long to actually create deck, but know which slot we want.
        //// Running backwards from end, we can work out which card is in that slot!
        private List<long> NaiveAttempt(long deckSize, ref long cardIndex, long result)
        {
            var inputs = new List<long> { cardIndex };

            for (int i = 0; i < 10; i++)
            {
                Debug.WriteLine($"{i} : {cardIndex}");
                foreach (string line in input.Reverse())
                {
                    //Debug.WriteLine($"{line}");
                    if (line.StartsWith("cut"))
                    {
                        long cutOffset = long.Parse(line.Substring(4));
                        if (cutOffset > 0)
                        {
                            if (cardIndex < cutOffset)// moving with cut
                            {
                                cardIndex += (deckSize - cutOffset);
                            }
                            else // moving back by cut amount
                            {
                                cardIndex -= cutOffset;
                            }
                        }
                        else if (cutOffset < 0)
                        {
                            cutOffset += deckSize;

                            if (cardIndex >= cutOffset)
                            {
                                cardIndex -= cutOffset;
                            }
                            else
                            {
                                cardIndex += (deckSize - cutOffset);
                            }
                        }
                    }
                    else if (line.StartsWith("deal into"))
                    {
                        // reverse
                        cardIndex = deckSize - cardIndex;
                    }
                    else // "deal with increment"
                    {
                        int increment = int.Parse(line.Substring(20));
                        long inverse = ModInv(increment, deckSize);
                        cardIndex = (result * cardIndex) % deckSize;
                    }

                    cardIndex = (cardIndex + deckSize) % deckSize;
                }
                inputs.Add(cardIndex);
            }

            return inputs;
        }

        private IEnumerable<int> Shuffle(IEnumerable<int> deck)
        {
            int count = deck.Count();

            foreach (string line in input)
            {
                //Debug.WriteLine($"{line}");
                if (line.StartsWith("cut"))
                {
                    int cutOffset = int.Parse(line.Substring(4));
                    if (cutOffset > 0)
                    {
                        var cut = deck.Take(cutOffset);
                        deck = deck.Skip(cutOffset).Concat(cut);
                    }
                    else if (cutOffset < 0)
                    {
                        cutOffset += count;
                        var cut = deck.Skip(cutOffset);
                        deck = cut.Concat(deck.Take(cutOffset));
                    }
                }
                else if (line.StartsWith("deal into"))
                {
                    deck = deck.Reverse();
                }
                else // "deal with increment"
                {
                    int increment = int.Parse(line.Substring(20));
                    var output = new int[count];
                    int i = 0;
                    foreach (var card in deck)
                    {
                        int index = (increment * i) % count;
                        output[index] = card;
                        i++;
                    }

                    deck = output;
                }
            }

            return deck;
        }


        private (long a, long b) CalculateIndexFormula(long deckSize)
        {
            long a = 1;
            long b = 0;

            foreach (string line in input)
            {
                long oldA = a;
                long oldB = b;

                if (line.StartsWith("cut"))
                {
                    long cutOffset = long.Parse(line.Substring(4));

                    // (ax + b) - c
                    b = (b - cutOffset + deckSize) % deckSize;
                }
                else if (line.StartsWith("deal into"))
                {
                    // len - 1 - (ax + b) equiv -1 - (ax+b)
                    a = (-a + deckSize) % deckSize;
                    b = (-b - 1 + deckSize) % deckSize;
                }
                else // "deal with increment"
                {
                    long increment = long.Parse(line.Substring(20));
                    // inc * (ax + b)
                    a = (a * increment) % deckSize;
                    b = (b * increment) % deckSize;
                }

                //Debug.WriteLine($"{line} : ({oldA}, {oldB}) => ({a}, {b})");

            }

            return (a, b);
        }

        private (BigInteger a, BigInteger b) CalculateReverseIndexFormula(long deckSize)
        {
            BigInteger a = 1;
            BigInteger b = 0;

            foreach (string line in input.Reverse())
            {
                BigInteger oldA = a;
                BigInteger oldB = b;

                if (line.StartsWith("cut"))
                {
                    long cutOffset = long.Parse(line.Substring(4));

                    // (ax + b) + c
                    b = (b + cutOffset + deckSize) % deckSize;
                }
                else if (line.StartsWith("deal into"))
                {
                    // len - 1 - (ax + b) equiv -1 - (ax+b)
                    a = (-a + deckSize) % deckSize;
                    b = (-b - 1 + deckSize) % deckSize;
                }
                else // "deal with increment"
                {
                    long increment = long.Parse(line.Substring(20));
                    // inc * (ax + b)

                    long inverse = ModInv(increment, deckSize);
                    //long value = (result * cardIndex) % deckSize;

                    a = (a * inverse) % deckSize;
                    b = (b * inverse) % deckSize;
                }

                //Debug.WriteLine($"{line} : ({oldA}, {oldB}) => ({a}, {b})");

            }

            return (a, b);
        }

        long ModInv(long a, long n)
        {
            long t = 0;
            long newt = 1;
            long r = n;
            long newr = a;
            while (newr != 0)
            {
                long quotient = r / newr;
                (t, newt) = (newt, t - quotient * newt);
                (r, newr) = (newr, r - quotient * newr);
            }

            if (r > 1) return -1;

            if (t < 0) t += n;
            return t;
        }
    }
}
