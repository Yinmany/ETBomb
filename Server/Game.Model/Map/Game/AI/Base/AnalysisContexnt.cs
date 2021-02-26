namespace Bomb.AI
{
    public struct AnalysisContexnt
    {
        /// <summary>
        /// 原本的手牌
        /// </summary>
        public CardComboAnalysis Analysis { get;  }

        public AnalysisContexnt(CardComboAnalysis analysis)
        {
            Analysis = analysis;
        }
    }
}