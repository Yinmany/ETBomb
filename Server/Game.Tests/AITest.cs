using System.Collections.Generic;
using System.Linq;
using Bomb;
using NUnit.Framework;

namespace Game.Tests
{
    public class AITest
    {
        [Test]
        public void Test()
        {
            // 分析炸弹
        }

        private void Analysis(List<Card> cards)
        {
            List<AnalyseResult> results = AnalyseResult.Analyse(cards);
            
            // 王
            var jokers = cards.Where(f => CardsHelper.IsJoker(f)).ToList();
            
            // 最大的8炸
            if (jokers.Count == 4)
            {
                
            }
            
            // 炸弹
            var booms = results.Where(f => !CardsHelper.IsJoker(f.Weight) && f.Count > 3).ToList();
            
            
        }
    }
}