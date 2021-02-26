using System.Collections.Generic;
using Bomb;
using NUnit.Framework;

namespace Tests
{
    public class PopCardHelperTests
    {
        /// <summary>
        /// 单牌，炸弹
        /// </summary>
        [Test]
        public void SingleTest1()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();
            // 单牌
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsFalse(result);
        }

        /// <summary>
        /// 单牌，炸弹
        /// </summary>
        [Test]
        public void SingleTest2()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            // 单牌
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// 单 炸弹
        /// </summary>
        [Test]
        public void SingleTest3()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            // 单牌
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// 单 炸弹
        /// </summary>
        [Test]
        public void SingleTest4()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            // 单牌
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.SJoker });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.LJoker });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// 单 炸弹
        /// </summary>
        [Test]
        public void SingleTest5()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            // 单牌
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.SJoker });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.LJoker });

            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsFalse(result);
        }

        /// <summary>
        /// 单牌，炸弹
        /// </summary>
        [Test]
        public void SingleTest6()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            // 单牌
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// 单牌，炸弹
        /// </summary>
        [Test]
        public void SingleTest7()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            // 单出王，玩法中根据规则来频断。
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.SJoker });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsTrue(result);
        }

        [Test]
        public void DoubleTest1()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });

            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            // handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            // handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            // handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.SJoker });
            // handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.LJoker });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsTrue(result);
        }

        [Test]
        public void DoubleTest2()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });

            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            // handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            // handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._4 });
            // handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.SJoker });
            // handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.LJoker });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsTrue(result);
        }

        [Test]
        public void DoubleTest3()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });

            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsTrue(result);
        }

        [Test]
        public void DoubleTest4()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });

            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.SJoker });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsTrue(result);
        }

        [Test]
        public void OnlyThreeTest1()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });

            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsTrue(result);
        }

        [Test]
        public void OnlyThreeTest2()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });

            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.SJoker });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.SJoker });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.LJoker });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.LJoker });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsTrue(result);
        }

        [Test]
        public void DoubleStraightTest1()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.Q });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.Q });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });

            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsTrue(result);
        }

        [Test]
        public void DoubleStraightTest2()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.Q });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.Q });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });

            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsFalse(result);
        }

        [Test]
        public void ThreeAndTwoTest1()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.Q });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.Q });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.Q });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });

            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsTrue(result);
        }

        [Test]
        public void ThreeAndTwoTest2()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });

            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsTrue(result);
        }

        [Test]
        public void TripleStraightTest1()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.Q });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.Q });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.Q });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });
            
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.Q });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.Q });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });

            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._2 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsTrue(result);
        }

        [Test]
        public void StraightTest1()
        {
            List<Card> deskCards = new List<Card>();
            List<Card> handCards = new List<Card>();

            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._9 });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._10 });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.J });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.Q });
            deskCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });

            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._10 });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.J });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.Q });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.K });
            handCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight.A });

            CardType handCardType = CardType.None;
            handCards.TryGetCardType(out handCardType);
            CardType deskCardType = CardType.None;
            deskCards.TryGetCardType(out deskCardType);

            bool result = handCards.Pop(handCardType, deskCards, deskCardType);
            Assert.IsTrue(result);
        }
        
        
    }
}