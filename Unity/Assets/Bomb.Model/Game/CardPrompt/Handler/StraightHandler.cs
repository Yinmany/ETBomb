using System.Collections.Generic;

namespace Bomb.Handler
{
    public class StraightHandler: ICardPromptPiplineHandler
    {
        public bool Check(CardsType targetType)
        {
            return targetType == CardsType.Straight;
        }

        public void Invoke(CardPromptPiplineContext context)
        {
            for (int i = 0; i <= context.AnalyseResults.Count - context.Target.Count; i++)
            {
                List<Card> tmpCards = new List<Card>();
                for (int j = i; j < context.Target.Count + i; j++)
                {
                    tmpCards.Add(context.AnalyseResults[j].Cards[0]);
                }

                if (context.CheckPop(tmpCards))
                {
                    context.Add(tmpCards);
                }
            }
        }
    }
}