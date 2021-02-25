using System.Collections.Generic;
using System.Linq;

namespace Bomb.Handler
{
    public class OnlyThreeHandler: ICardPromptPiplineHandler
    {
        public bool Check(CardsType targetType)
        {
            return targetType == CardsType.OnlyThree;
        }

        public void Invoke(CardPromptPiplineContext context)
        {
            // 符合3张的所有牌类
            List<AnalyseResult> doubleAnalyseResults = context.AnalyseResults.Where(f => f.Count >= 3 && !CardsHelper.IsJoker(f.Weight)).ToList();
            foreach (AnalyseResult analyseResult in doubleAnalyseResults)
            {
                if (context.CheckPop(analyseResult.Cards))
                {
                    context.Add(new PrompCards { Cards = analyseResult.Cards.GetRange(0, 3), CardsType = context.TargetType });
                }
            }
        }
    }
}