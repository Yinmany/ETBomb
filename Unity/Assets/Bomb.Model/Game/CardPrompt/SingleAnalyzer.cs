using System.Collections.Generic;

namespace Bomb.CardPrompt
{
    public class SingleAnalyzer: IAnalyzer
    {
        public bool Check(CardType targetType)
        {
            return targetType == CardType.Single;
        }

        public void Invoke(AnalysisContext context)
        {
            foreach (AnalyseResult analyse in context.AnalyseResults)
            {
                var list = new List<Card> { analyse.Cards[0] };
                if (context.CheckPop(list))
                {
                    context.Add(new PrompCards { Cards = list, CardType = context.TargetType });
                }
            }
        }
    }
}