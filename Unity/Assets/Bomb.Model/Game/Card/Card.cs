using System;
using System.ComponentModel;
using System.Reflection;

namespace Bomb
{
    /// <summary>
    /// 牌
    /// </summary>
    public struct Card: IEquatable<Card>, IComparable<Card>
    {
        public CardColor Color { get; set; }
        public CardWeight Weight { get; set; }

        public bool Equals(Card other)
        {
            // 权重和颜色相等就是一样的牌
            return this.Weight == other.Weight && this.Color == other.Color;
        }

        public override string ToString()
        {
            var colorDesc = this.Color.GetType().GetField(this.Color.ToString()).GetCustomAttribute<DescriptionAttribute>(false);

            return $"{colorDesc?.Description}{GetWeightDesc()}";
        }

        public string GetWeightDesc()
        {
            var weightDesc = this.Weight.GetType().GetField(this.Weight.ToString()).GetCustomAttribute<DescriptionAttribute>(false);
            return weightDesc?.Description;
        }

        public int CompareTo(Card other)
        {
            int a = (int) this.Color + (int) this.Weight * 100;
            int b = (int) other.Color + (int) other.Weight * 100;
            return a.CompareTo(b);
        }
    }
}