using System;
using System.Collections.Generic;

namespace Bomb
{
    public struct AnalyseResult: IComparable<AnalyseResult>
    {
        public CardWeight Weight { get; set; }
        public int Count { get; set; }

        public List<Card> Cards { get; set; } // 保留原本的牌

        public override string ToString()
        {
            return $"{this.Weight}:{this.Count}";
        }

        /// <summary>
        /// 分析
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static List<AnalyseResult> Analyse(IReadOnlyList<Card> cards)
        {
            List<AnalyseResult> results = new List<AnalyseResult>();
            for (int i = 0; i < cards.Count; i++)
            {
                Card item = cards[i];

                bool has = false;
                for (int j = 0; j < results.Count; j++)
                {
                    AnalyseResult result = results[j];
                    if (result.Weight != item.Weight)
                        continue;

                    has = true;
                    ++result.Count;
                    results[j] = result;
                    result.Cards.Add(item);
                }

                if (has)
                {
                    continue;
                }

                AnalyseResult ar = new AnalyseResult { Weight = item.Weight, Count = 1 };
                ar.Cards = new List<Card>();
                ar.Cards.Add(item);
                results.Add(ar);
            }

            return results;
        }

        public int CompareTo(AnalyseResult other)
        {
            // 直接按照数量排序，因为牌在处理之前就需要进行升序排序。
            // 在分析牌的数量时，就以及是按照权重有序进行分析的。
            return other.Count.CompareTo(this.Count);
        }
    }
}