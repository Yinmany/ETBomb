using System.Collections.Generic;

namespace Bomb.CardPrompt
{
    /// <summary>
    /// 提示分析器
    /// </summary>
    public class CardPromptAnalysis: LinkedList<IAnalyzer>
    {
        private List<PrompCards> prompCardsList;

        public IReadOnlyList<PrompCards> PrompCardsList => this.prompCardsList;

        public void SetPrompCardsList(List<PrompCards> list)
        {
            this.prompCardsList = list;
        }

        /// <summary>
        /// 开始执行提示
        /// </summary>
        /// <param name="handCards"></param>
        /// <param name="target"></param>
        /// <param name="targetType"></param>
        public void Run(List<Card> handCards, List<Card> target, CardType targetType)
        {
            if (this.prompCardsList == null)
            {
                prompCardsList = new List<PrompCards>();
            }

            // 每次执行都清空上次的提示结果
            this.prompCardsList.Clear();

            // 创建提示上下文
            AnalysisContext context = new AnalysisContext(this, this.prompCardsList, handCards, target, targetType);
            foreach (IAnalyzer handler in this)
            {
                if (handler.Check(targetType))
                {
                    handler.Invoke(context);
                }
            }

            // 提示结果按照权重排序
            this.prompCardsList.Sort();
        }
    }
}