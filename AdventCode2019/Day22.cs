using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            long Inverse(long a, long n)
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


            long deckSize = 119315717514047;
            long cardIndex = 2020;

            long result = Inverse(51, deckSize);
            long value = (result * cardIndex) % deckSize;
            long check = (value * 51) % deckSize;


            //long repeats = 101741582076661;

            //// Too long to actually create deck, but know which slot we want.
            //// Running backwards from end, we can work out which card is in that slot!

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
                        long inverse = Inverse(increment, deckSize);
                        cardIndex = (result * cardIndex) % deckSize;
                    }

                    cardIndex = (cardIndex + deckSize) % deckSize;
                }
                inputs.Add(cardIndex);
            }
            // card @ 8394737559856 ends in spot 2020 after 1 round
            //inputs.Reverse();
            Debug.WriteLine($"{String.Join(", ", inputs.Select(input => input.ToString()))}");

            Assert.AreEqual(result, 4825810);
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
    }
}
