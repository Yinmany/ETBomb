using System.Collections.Generic;

namespace Bomb.CardPrompt
{
    public class AnalysisContext
    {
        public CardPromptAnalysis Analysis { get; }
        private List<PrompCards> PrompCardsList { get; }
        public IReadOnlyList<Card> HandCards { get; }
        public IReadOnlyList<Card> Target { get; }
        public CardType TargetType { get; }
        public IReadOnlyList<AnalyseResult> AnalyseResults { get; }

        public AnalysisContext(CardPromptAnalysis analysis, List<PrompCards> prompCardsList, IReadOnlyList<Card> handCards,
        IReadOnlyList<Card> target,
        CardType targetType)
        {
            this.Analysis = analysis;
            this.PrompCardsList = prompCardsList;
            this.HandCards = handCards;
            this.Target = target;
            this.TargetType = targetType;
            this.AnalyseResults = AnalyseResult.Analyse(handCards);
        }

        public void Add(PrompCards prompCards)
        {
            PrompCardsList.Add(prompCards);
        }

        public void Add(List<Card> cards, CardType type = CardType.None)
        {
            if (type == CardType.None)
            {
                type = this.TargetType;
            }

            Add(new PrompCards() { Cards = cards, CardType = type });
        }

        public bool CheckPop(List<Card> cards, CardType type)
        {
            return PopCardHelper.Pop(new PopCheckInfo
            {
                HandCards = HandCards.Count,
                PopCards = cards,
                PopCardType = type,
                DesktopCards = Target,
                DesktopCardType = TargetType
            });
        }

        public bool CheckPop(List<Card> cards)
        {
            return cards.TryGetCardType(out var popCardType) && CheckPop(cards, popCardType);
        }
    }
}