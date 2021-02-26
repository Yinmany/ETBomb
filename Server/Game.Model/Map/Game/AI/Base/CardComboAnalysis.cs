using System.Collections.Generic;
using System.Linq;

namespace Bomb.AI
{
    /// <summary>
    /// 牌组分析
    /// </summary>
    public class CardComboAnalysis: LinkedList<IAnalyzer>
    {
        private IReadOnlyList<Card> _originCards;

        public CardComboAnalysis(IReadOnlyList<Card> cards)
        {
            this._originCards = cards;
        }

        /// <summary>
        /// 运行分析
        /// </summary>
        /// <param name="cards">手牌</param>
        public void Run(List<Card> cards)
        {
            // 使用手牌开始分析
            if (cards == null)
            {
                cards = this._originCards.ToList();
            }

            var ctx = new AnalysisContexnt(this);
            foreach (IAnalyzer analyzer in this)
            {
                analyzer.Run(ctx);
            }
        }
    }
}