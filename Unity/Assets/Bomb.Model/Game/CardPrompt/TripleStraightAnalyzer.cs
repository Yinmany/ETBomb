using System;
using System.Collections.Generic;
using System.Linq;

namespace Bomb.CardPrompt
{
    public class TripleStraightAnalyzer: ThreeAndTwoAnalyzer
    {
        public override bool Check(CardType targetType)
        {
            return targetType == CardType.TripleStraight;
        }

        public override void Invoke(AnalysisContext context)
        {
            // 几个连3带2
            int targetNum = context.Target.Count / 5;

            // 3张的牌，大于3张的牌就是属于炸弹了。
            List<AnalyseResult> threeAnalyseResults = context.AnalyseResults.Where(f => f.Count == 3).ToList();

            // 牌不够
            if (targetNum > threeAnalyseResults.Count)
            {
                return;
            }

            // 带的牌
            var cards = this.GetTwo(context.AnalyseResults);

            for (int i = 0; i < threeAnalyseResults.Count / targetNum; i++)
            {
                List<Card> tmpCards = new List<Card>();

                // 每次往后找targetNum个牌
                for (int j = i; j < targetNum + i; j++)
                {
                    tmpCards.AddRange(threeAnalyseResults[j].Cards.GetRange(0, 3));
                }

                // 带的牌不够
                if (cards.Count < targetNum)
                {
                    // 添加最小用于逻辑的牌
                    for (int j = 0; j < targetNum * 2; j++)
                    {
                        tmpCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
                    }

                    if (!context.CheckPop(tmpCards))
                    {
                        continue;
                    }

                    tmpCards.RemoveRange(targetNum * 3 - 1, targetNum * 2);
                    context.Add(tmpCards);
                }
                else
                {
                    // 每两张都和
                    for (int j = 0; j < cards.Count / targetNum; j++)
                    {
                        List<Card> tmp2 = new List<Card>();
                        tmp2.AddRange(tmpCards);

                        for (int k = j; k < targetNum + j; k++)
                        {
                            (Card item1, Card item2) = cards[k];

                            // 添加的带的牌
                            tmp2.Add(item1);
                            tmp2.Add(item2);
                        }

                        if (context.CheckPop(tmp2))
                        {
                            context.Add(tmp2);
                        }
                    }
                }
            }
        }
    }
}