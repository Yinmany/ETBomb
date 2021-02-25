using System.Collections.Generic;
using Bomb;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class AnalyseResultTests
    {
        [Test]
        public void Test()
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

            CardsHelper.Sort(cards);

            var list = AnalyseResult.Analyse(cards);
            foreach (AnalyseResult analyseResult in list)
            {
                Debug.Log(analyseResult);
            }

            Debug.Log("================================");
            list.Sort();
            foreach (AnalyseResult analyseResult in list)
            {
                Debug.Log(analyseResult);
            }

            Assert.AreEqual(list[0].Weight, CardWeight._3);
            Assert.AreEqual(list[1].Weight, CardWeight._4);
            Assert.AreEqual(list[2].Weight, CardWeight._5);

            Assert.AreEqual(list[list.Count - 1].Weight, CardWeight._8);
        }
    }
}