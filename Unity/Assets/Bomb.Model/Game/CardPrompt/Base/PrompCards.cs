using System;
using System.Collections.Generic;

namespace Bomb.CardPrompt
{
    public struct PrompCards: IComparable<PrompCards>
    {
        public CardType CardType;
        public List<Card> Cards;

        public int CompareTo(PrompCards other)
        {
            var a = CardsHelper.GetWeight(other.Cards, other.CardType);
            var b = CardsHelper.GetWeight(this.Cards, this.CardType);
            return b.CompareTo(a);
        }
    }
}