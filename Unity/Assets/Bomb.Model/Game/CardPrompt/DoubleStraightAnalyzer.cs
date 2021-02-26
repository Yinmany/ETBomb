using System.Collections.Generic;
using System.Linq;

namespace Bomb.CardPrompt
{
    public class DoubleStraightAnalyzer: IAnalyzer
    {
        public bool Check(CardType targetType)
        {
            return targetType == CardType.DoubleStraight;
        }

        public void Invoke(AnalysisContext context)
        {
            // 搜索连续的连队
            //  1.确定目标是几对
            //  2.根据目标对子搜索连续的牌型
            int targetNum = context.Target.Count / 2;

            // 找到两个以上的牌
            List<AnalyseResult> doubleAnalyseResults = context.AnalyseResults.Where(f => f.Count >= 2).ToList();

            // 手中牌不够
            if (doubleAnalyseResults.Count < targetNum)
            {
                return;
            }

            List<Card> tmpCards = new List<Card>();
            for (int i = 0; i <= doubleAnalyseResults.Count - targetNum; i++)
            {
                tmpCards.Clear();
                for (int j = i; j < targetNum + i; j++)
                {
                    tmpCards.AddRange(doubleAnalyseResults[j].Cards.GetRange(0, 2));
                }

                if (context.CheckPop(tmpCards))
                {
                    context.Add(tmpCards.ToList());
                }
            }
        }
    }
}