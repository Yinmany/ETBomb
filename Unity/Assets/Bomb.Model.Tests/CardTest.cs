using System.Collections.Generic;
using Bomb;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public static class CardTestExtensions
    {
        public static void Log(this IList<Card> cards)
        {
            foreach (Card card in cards)
            {
                Debug.Log(card);
            }
        }
    }

    public class CardTest
    {
        [Test]
        public void TestSpawn()
        {
            List<Card> cards = new List<Card>(54);

            // 生成一副新牌
            CardsHelper.Spawn(cards);

            cards.Log();

            // 54张牌
            Assert.AreEqual(cards.Count, 54);
        }

        [Test]
        public void TestShuffle()
        {
            List<Card> cards = new List<Card>(54);
            // 生成一副新牌
            CardsHelper.Spawn(cards);

            CardsHelper.Shuffle1(cards);

            cards.Log();
        }

        [Test]
        public void TestShuffle2()
        {
            List<Card> cards = new List<Card>(54);

            // 生成一副新牌
            CardsHelper.Spawn(cards);

            // 只保留8张牌测试
            // cards.RemoveRange(8, cards.Count - 8);

            CardsHelper.Shuffle2(cards);

            cards.Log();
        }

        [Test]
        public void TestShuffle3()
        {
            List<Card> cards = new List<Card>(54);
            // 生成一副新牌
            CardsHelper.Spawn(cards);

            CardsHelper.Shuffle1(cards);
            CardsHelper.Shuffle2(cards);
            cards.Log();
        }

        [Test]
        public void TestShuffle4()
        {
            // 两幅牌
            List<Card> cards = new List<Card>(108);

            // 生成一副新牌
            CardsHelper.Spawn(cards);
            // 生成一副新牌
            CardsHelper.Spawn(cards);

            CardsHelper.Shuffle1(cards);
            CardsHelper.Shuffle2(cards);
            cards.Log();
        }

        [Test]
        public void TestSort()
        {
            List<Card> cards = new List<Card>(54);
            // 生成一副新牌
            CardsHelper.Spawn(cards);
            CardsHelper.Shuffle(cards);
            CardsHelper.Sort(cards);
            cards.Log();
        }

        [Test]
        public void TestBoomSort()
        {
            List<Card> cards = new List<Card>();
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._5 });
            cards.Add(new Card { Color = CardColor.Diamond, Weight = CardWeight._5 });
            cards.Add(new Card { Color = CardColor.Heart, Weight = CardWeight._5 });
            cards.Add(new Card { Color = CardColor.Spade, Weight = CardWeight._5 });
            cards.Add(new Card { Color = CardColor.Spade, Weight = CardWeight._5 });
            cards.Add(new Card { Color = CardColor.Spade, Weight = CardWeight._5 });

            cards.Add(new Card { Color = CardColor.None, Weight = CardWeight.SJoker });
            cards.Add(new Card { Color = CardColor.None, Weight = CardWeight.LJoker });

            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            cards.Add(new Card { Color = CardColor.Diamond, Weight = CardWeight._2 });
            cards.Add(new Card { Color = CardColor.Heart, Weight = CardWeight._2 });
            cards.Add(new Card { Color = CardColor.Spade, Weight = CardWeight._2 });
            cards.Add(new Card { Color = CardColor.Spade, Weight = CardWeight._2 });

            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._6 });
            cards.Add(new Card { Color = CardColor.Diamond, Weight = CardWeight._6 });
            cards.Add(new Card { Color = CardColor.Heart, Weight = CardWeight._6 });
            cards.Add(new Card { Color = CardColor.Spade, Weight = CardWeight._6 });
            cards.Add(new Card { Color = CardColor.Spade, Weight = CardWeight._6 });
            cards.Add(new Card { Color = CardColor.Spade, Weight = CardWeight._6 });

            CardsHelper.BoomSort(cards);
            cards.Log();
            Debug.Log("==============================================");
            CardsHelper.BoomSort(cards, false);
            cards.Log();

            Assert.AreEqual(cards[cards.Count - 1].Weight, CardWeight._6);
            Assert.AreEqual(cards[cards.Count - 7].Weight, CardWeight._5);
        }

        [Test]
        public void TestWeightSort()
        {
            List<Card> cards = new List<Card>();
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._8 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._7 });

            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._5 });
            cards.Add(new Card { Color = CardColor.Diamond, Weight = CardWeight._5 });
            cards.Add(new Card { Color = CardColor.Heart, Weight = CardWeight._5 });
            cards.Add(new Card { Color = CardColor.Spade, Weight = CardWeight._9 });
            cards.Add(new Card { Color = CardColor.Spade, Weight = CardWeight._10 });

            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            cards.Add(new Card { Color = CardColor.Diamond, Weight = CardWeight._4 });
            cards.Add(new Card { Color = CardColor.Heart, Weight = CardWeight._4 });
            cards.Add(new Card { Color = CardColor.Spade, Weight = CardWeight._9 });
            cards.Add(new Card { Color = CardColor.Spade, Weight = CardWeight._10 });

            CardsHelper.WeightSort(cards);
            cards.Log();

            Assert.AreEqual(cards[0].Weight, CardWeight._3);
            Assert.AreEqual(cards[1].Weight, CardWeight._3);
            Assert.AreEqual(cards[2].Weight, CardWeight._3);

            Assert.AreEqual(cards[cards.Count - 2].Weight, CardWeight._7);
            Assert.AreEqual(cards[cards.Count - 1].Weight, CardWeight._8);
        }

        [Test]
        public void Test()
        {
            List<Card> cards = new List<Card>();
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            CardsHelper.TryGetCardsType(cards, out CardsType type);
            Assert.AreEqual(type, CardsType.Single);

            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            CardsHelper.TryGetCardsType(cards, out type);
            Assert.AreEqual(type, CardsType.Double);

            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            CardsHelper.TryGetCardsType(cards, out type);
            Assert.AreEqual(type, CardsType.None);

            // 移除最后一个
            cards.RemoveAt(cards.Count - 1);
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            CardsHelper.TryGetCardsType(cards, out type);
            Assert.AreEqual(type, CardsType.OnlyThree);
        }

        [Test]
        public void TestBoom()
        {
            List<Card> cards = new List<Card>();
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            CardsHelper.TryGetCardsType(cards, out CardsType type);
            Assert.AreEqual(type, CardsType.Boom);

            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            CardsHelper.TryGetCardsType(cards, out type);
            Assert.AreEqual(type, CardsType.Boom);

            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.SJoker });
            CardsHelper.TryGetCardsType(cards, out type);
            Assert.AreEqual(type, CardsType.Boom);

            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.LJoker });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.LJoker });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.SJoker });
            CardsHelper.TryGetCardsType(cards, out type);
            Assert.AreEqual(type, CardsType.Boom);

            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            CardsHelper.TryGetCardsType(cards, out type);
            Assert.AreEqual(type, CardsType.None);
        }

        [Test]
        public void TestDoubleStraight()
        {
            List<Card> cards = new List<Card>();
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._5 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._5 });
            CardsHelper.TryGetCardsType(cards, out CardsType type);
            Assert.AreEqual(type, CardsType.None);

            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            CardsHelper.TryGetCardsType(cards, out type);
            Assert.AreEqual(type, CardsType.DoubleStraight);
        }

        [Test]
        public void TestThreeAndTwo()
        {
            List<Card> cards = new List<Card>();
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._5 });
            CardsHelper.TryGetCardsType(cards, out CardsType type);
            Assert.AreEqual(type, CardsType.None);

            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            CardsHelper.TryGetCardsType(cards, out type);
            Assert.AreEqual(type, CardsType.ThreeAndTwo);
        }

        [Test]
        public void TestStraight()
        {
            List<Card> cards = new List<Card>();
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._5 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._6 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._7 });
            CardsHelper.TryGetCardsType(cards, out CardsType type);
            Assert.AreEqual(type, CardsType.Straight);

            cards.Clear();
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._10 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.J });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.Q });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            CardsHelper.TryGetCardsType(cards, out type);
            Assert.AreEqual(type, CardsType.Straight);

            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            CardsHelper.TryGetCardsType(cards, out type);
            Assert.AreEqual(type, CardsType.None);
        }

        [Test]
        public void TestTripleStraight()
        {
            List<Card> cards = new List<Card>();
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });

            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            cards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            CardsHelper.TryGetCardsType(cards, out CardsType type);
            Assert.AreEqual(type, CardsType.TripleStraight);
        }
        
        
    }
}