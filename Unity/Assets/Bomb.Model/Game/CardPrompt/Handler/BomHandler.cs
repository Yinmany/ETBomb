using System.Collections.Generic;
using System.Linq;

namespace Bomb.Handler
{
    public class BomHandler: ICardPromptPiplineHandler
    {
        // 炸弹通吃
        public bool Check(CardsType targetType)
        {
            return true;
        }

        public void Invoke(CardPromptPiplineContext context)
        {
            // 1.搜寻炸弹
            //      a.先确定王的个数
            //      b.大于等于4张的炸弹
            //      c.王与炸弹的所有组合
            //      d.按照炸弹权重排序
            List<AnalyseResult> jokerAnalyseResults = context.AnalyseResults.Where(f => CardsHelper.IsJoker(f.Weight)).ToList();
            int jokerCount = jokerAnalyseResults.Sum(f => f.Count);

            // 不包括王,如果王有4张，就再最后的时候，把4张王单独添加。
            List<AnalyseResult> boomResults = context.AnalyseResults.Where(f => f.Count >= 4 && !CardsHelper.IsJoker(f.Weight)).ToList();

            for (int i = 0; i < boomResults.Count; i++)
            {
                var result = boomResults[i];

                // 验证此牌是否能接目标牌型
                if (context.CheckPop(result.Cards, CardsType.Boom))
                {
                    context.Add(new PrompCards { CardsType = CardsType.Boom, Cards = result.Cards.ToList() });
                }
                
                // 组合王
                var boomCards = result.Cards.ToList();

                for (int j = 0; j < jokerAnalyseResults.Count; j++)
                {
                    AnalyseResult analyseResult = jokerAnalyseResults[j];
                    for (int k = 0; k < analyseResult.Count; k++)
                    {
                        var item = analyseResult.Cards[k];
                        boomCards.Add(item);

                        // 每添加一个王，就向提示列表中，添加一种。
                        if (context.CheckPop(boomCards, CardsType.Boom))
                        {
                            context.Add(new PrompCards { CardsType = CardsType.Boom, Cards = boomCards.ToList() });
                        }
                    }
                }
            }

            // 4张王，算最大的炸弹. 单独添加进提示列表
            if (jokerCount == 4)
            {
                var jokerBoomCards = new List<Card>();
                for (int i = 0; i < jokerAnalyseResults.Count; i++)
                {
                    jokerBoomCards.AddRange(jokerAnalyseResults[i].Cards);
                }

                context.Add(new PrompCards { CardsType = CardsType.JokerBoom, Cards = jokerBoomCards });
            }
        }
    }
}