using System;
using System.Collections.Generic;
using System.Linq;

namespace Bomb.Handler
{
    public class ThreeAndTwoHandler: ICardPromptPiplineHandler
    {
        public virtual bool Check(CardsType targetType)
        {
            return targetType == CardsType.ThreeAndTwo;
        }

        public virtual void Invoke(CardPromptPiplineContext context)
        {
            // 1.找出3张的牌，多于3张的牌提示为炸弹.
            // 2.找出单牌,找出对子.排除王
            // 3.优先带单牌，单牌不足拆对子。没有单牌时，就带对子.都没有由玩家决定带的牌。

            // 3张的牌，大于3张的牌就是属于炸弹了。
            List<AnalyseResult> threeAnalyseResults = context.AnalyseResults.Where(f => f.Count == 3).ToList();

            var cards = GetTwo(context.AnalyseResults);

            // 生成提示牌
            for (int i = 0; i < threeAnalyseResults.Count; i++)
            {
                // 可带牌为0(因为只从单张和对子中进行带牌),只用生成大于牌桌上的3张牌，余下2张带牌由玩家挑选。
                if (cards.Count == 0)
                {
                    List<Card> tmpCards = new List<Card>();
                    // 添加3张的牌
                    tmpCards.AddRange(threeAnalyseResults[i].Cards);

                    // 添加两张最小用于逻辑的牌
                    tmpCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
                    tmpCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });

                    if (!context.CheckPop(tmpCards))
                    {
                        continue;
                    }

                    // 移除逻辑牌
                    tmpCards.RemoveAt(tmpCards.Count - 1);
                    tmpCards.RemoveAt(tmpCards.Count - 1);

                    context.Add(tmpCards);
                }
                else
                {
                    for (int j = 0; j < cards.Count; j++)
                    {
                        (Card item1, Card item2) = cards[j];

                        List<Card> tmpCards = new List<Card>();

                        // 添加3张的牌
                        tmpCards.AddRange(threeAnalyseResults[i].Cards);

                        // 添加的带的牌
                        tmpCards.Add(item1);
                        tmpCards.Add(item2);

                        if (context.CheckPop(tmpCards))
                        {
                            context.Add(tmpCards);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 找出可带的牌
        /// </summary>
        /// <param name="analyseResults"></param>
        /// <returns></returns>
        protected List<Tuple<Card, Card>> GetTwo(IReadOnlyList<AnalyseResult> analyseResults)
        {
            // 找出除王的单牌
            List<AnalyseResult> oneAnalyseResults = analyseResults.Where(f => f.Count == 1 && !CardsHelper.IsJoker(f.Weight)).ToList();

            // 找出除王的对子
            List<AnalyseResult> twoAnalyseResults = analyseResults.Where(f => f.Count == 2 && !CardsHelper.IsJoker(f.Weight)).ToList();

            // 可带的牌
            List<Tuple<Card, Card>> cards = new List<Tuple<Card, Card>>();

            // 单牌与对子的拆分组合
            for (int i = 0; i < oneAnalyseResults.Count; i++)
            {
                Card a = oneAnalyseResults[i].Cards[0];
                Card b = default;
                int next = i + 1;

                // 单牌没有了，拆分对子替补。
                if (next > oneAnalyseResults.Count - 1)
                {
                    if (twoAnalyseResults.Count == 0)
                    {
                        break;
                    }

                    b = twoAnalyseResults[0].Cards[0];
                }
                else
                {
                    b = oneAnalyseResults[next].Cards[0];
                }

                cards.Add(new Tuple<Card, Card>(a, b));
            }

            // 对子
            for (int i = 0; i < twoAnalyseResults.Count; i++)
            {
                cards.Add(new Tuple<Card, Card>(twoAnalyseResults[i].Cards[0], twoAnalyseResults[i].Cards[1]));
            }

            return cards;
        }
    }
}