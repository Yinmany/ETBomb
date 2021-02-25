using System.Collections.Generic;

namespace Bomb
{
    public class CardPromptPiplineContext
    {
        public CardPromptPipline Pipline { get; }
        private List<PrompCards> PrompCardsList { get; }
        public IReadOnlyList<Card> HandCards { get; }
        public IReadOnlyList<Card> Target { get; }
        public CardsType TargetType { get; }
        public IReadOnlyList<AnalyseResult> AnalyseResults { get; }

        public CardPromptPiplineContext(CardPromptPipline pipline, List<PrompCards> prompCardsList, IReadOnlyList<Card> handCards,
        IReadOnlyList<Card> target,
        CardsType targetType)
        {
            this.Pipline = pipline;
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

        public void Add(List<Card> cards, CardsType type = CardsType.None)
        {
            if (type == CardsType.None)
            {
                type = this.TargetType;
            }

            Add(new PrompCards() { Cards = cards, CardsType = type });
        }

        public bool CheckPop(List<Card> cards, CardsType type)
        {
            return PopCardHelper.Pop(new PopCheckInfo
            {
                HandCards = HandCards.Count,
                PopCards = cards,
                PopCardsType = type,
                DesktopCards = Target,
                DesktopCardsType = TargetType
            });
        }

        public bool CheckPop(List<Card> cards)
        {
            return CheckPop(cards, this.TargetType);
        }
    }
}