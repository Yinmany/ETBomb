using System.Collections.Generic;

namespace Bomb.Handler
{
    public class SingleHandler: ICardPromptPiplineHandler
    {
        public bool Check(CardsType targetType)
        {
            return targetType == CardsType.Single;
        }

        public void Invoke(CardPromptPiplineContext context)
        {
            foreach (AnalyseResult analyse in context.AnalyseResults)
            {
                var list = new List<Card> { analyse.Cards[0] };
                if (context.CheckPop(list))
                {
                    context.Add(new PrompCards { Cards = list, CardsType = context.TargetType });
                }
            }
        }
    }
}