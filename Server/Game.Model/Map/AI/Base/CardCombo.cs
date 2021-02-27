namespace Bomb.AI
{
    /// <summary>
    /// 牌组
    /// </summary>
    public struct CardCombo
    {
        public CardType Type { get; }
        public Card[] Cards { get; }

        public CardCombo(CardType type, Card[] cards)
        {
            this.Type = type;
            this.Cards = cards;
        }
    }
}