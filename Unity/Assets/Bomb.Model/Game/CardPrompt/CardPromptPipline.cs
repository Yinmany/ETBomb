using System.Collections.Generic;

namespace Bomb
{
    /// <summary>
    /// 提示管道
    /// </summary>
    public class CardPromptPipline
    {
        private List<PrompCards> prompCardsList;

        public IReadOnlyList<PrompCards> PrompCardsList => this.prompCardsList;

        public LinkedList<ICardPromptPiplineHandler> Handlers { get; } = new LinkedList<ICardPromptPiplineHandler>();

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
        public void Run(List<Card> handCards, List<Card> target, CardsType targetType)
        {
            if (this.prompCardsList == null)
            {
                prompCardsList = new List<PrompCards>();
            }

            // 每次执行都清空上次的提示结果
            this.prompCardsList.Clear();

            // 创建提示上下文
            CardPromptPiplineContext context = new CardPromptPiplineContext(this, this.prompCardsList, handCards, target, targetType);
            foreach (ICardPromptPiplineHandler handler in this.Handlers)
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