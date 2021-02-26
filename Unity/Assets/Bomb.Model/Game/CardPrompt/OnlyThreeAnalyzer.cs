using System.Collections.Generic;
using System.Linq;

namespace Bomb.CardPrompt
{
    public class OnlyThreeAnalyzer: IAnalyzer
    {
        public bool Check(CardType targetType)
        {
            return targetType == CardType.OnlyThree;
        }

        public void Invoke(AnalysisContext context)
        {
            // 符合3张的所有牌类
            List<AnalyseResult> doubleAnalyseResults = context.AnalyseResults.Where(f => f.Count >= 3 && !CardsHelper.IsJoker(f.Weight)).ToList();
            foreach (AnalyseResult analyseResult in doubleAnalyseResults)
            {
                if (context.CheckPop(analyseResult.Cards))
                {
                    context.Add(new PrompCards { Cards = analyseResult.Cards.GetRange(0, 3), CardType = context.TargetType });
                }
            }
        }
    }
}