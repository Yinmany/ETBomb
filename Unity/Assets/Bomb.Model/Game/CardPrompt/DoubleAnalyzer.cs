using System.Collections.Generic;
using System.Linq;

namespace Bomb.CardPrompt
{
    public class DoubleAnalyzer: IAnalyzer
    {
        public bool Check(CardType targetType)
        {
            return targetType == CardType.Double;
        }

        public void Invoke(AnalysisContext context)
        {
            // 符合2张的所有牌类
            List<AnalyseResult> doubleAnalyseResults = context.AnalyseResults.Where(f => f.Count >= 2 && !CardsHelper.IsJoker(f.Weight)).ToList();
            foreach (AnalyseResult analyseResult in doubleAnalyseResults)
            {
                if (context.CheckPop(analyseResult.Cards))
                {
                    context.Add(new PrompCards { Cards = analyseResult.Cards.GetRange(0, 2), CardType = context.TargetType });
                }
            }
        }

        // 局部方法
        // private void AddAnalyseResultToPrompCards(CardPromptPiplineContext context, List<AnalyseResult> doubleAnalyseResults, int multiple)
        // {
        //     foreach (AnalyseResult doubleAnalyseResult in doubleAnalyseResults)
        //     {
        //         // 如果是对子就只添加到2的倍数
        //         // 3/2=1*2=2 4/2=2*2=4
        //         int count = doubleAnalyseResult.Count / multiple * multiple;
        //         var list = doubleAnalyseResult.Cards.GetRange(0, count);
        //
        //         if (Pop(new PopCheckInfo
        //         {
        //             HandCards = handCards.Count,
        //             PopCards = list,
        //             PopCardsType = targetType,
        //             DesktopCards = target,
        //             DesktopCardsType = targetType
        //         }))
        //         {
        //             prompCardsList.Add(new PrompCards { Cards = list, CardsType = targetType });
        //         }
        //     }
        // }
    }
}