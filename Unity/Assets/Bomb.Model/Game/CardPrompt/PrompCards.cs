using System;
using System.Collections.Generic;

namespace Bomb
{
    public struct PrompCards: IComparable<PrompCards>
    {
        public CardsType CardsType;
        public List<Card> Cards;

        public int CompareTo(PrompCards other)
        {
            var a = CardsHelper.GetWeight(other.Cards, other.CardsType);
            var b = CardsHelper.GetWeight(this.Cards, this.CardsType);
            return b.CompareTo(a);
        }
    }
}