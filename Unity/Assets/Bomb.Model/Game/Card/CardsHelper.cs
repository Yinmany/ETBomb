using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ET;

namespace Bomb
{
    public static class CardsHelper
    {
        /// <summary>
        /// 计算牌展开的位置
        /// </summary>
        /// <param name="count">牌的数量</param>
        /// <param name="marginLeft">间距</param>
        /// <param name="onPos">展开后的位置</param>
        public static void ComputerCardPos(int count, float marginLeft, Action<int, float, float> onPos)
        {
            // 每张牌，往左移动的距离。
            float moveLeft = (count - 1) * marginLeft / 2;

            for (int i = 0; i < count; i++)
            {
                onPos?.Invoke(i, i * marginLeft - moveLeft, 0);
            }
        }

        /// <summary>
        /// 生成一副牌
        /// </summary>
        /// <param name="cards"></param>
        public static void Spawn(List<Card> cards)
        {
            // 发出52张牌，不带王.
            for (int i = 0; i < 13; i++)
            {
                int weight = i % 13;
                for (int j = 0; j < 4; j++)
                {
                    int color = j % 4 + 1;
                    cards.Add(new Card { Color = (CardColor) color, Weight = (CardWeight) weight });
                }
            }

            // 两张大王
            cards.Add(new Card { Color = CardColor.None, Weight = CardWeight.SJoker });
            cards.Add(new Card { Color = CardColor.None, Weight = CardWeight.LJoker });
        }

        public static void Shuffle(List<Card> cards)
        {
            Shuffle1(cards);
            Shuffle2(cards);
        }

        /// <summary>
        /// 洗牌
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="count">洗牌次数</param>
        public static void Shuffle1(List<Card> cards, int count = 5)
        {
            // 上下交替
            for (int i = 0; i < count; i++)
            {
                int lower = RandomHelper.RandomNumber(1, cards.Count / 4);
                int upper = RandomHelper.RandomNumber(cards.Count / 4, cards.Count / 2);
                int swapCount = RandomHelper.RandomNumber(lower, upper);

                for (int j = 0; j < swapCount; j++)
                {
                    int index = cards.Count - j - 1;
                    var card = cards[index];
                    cards.RemoveAt(index);
                    cards.Insert(0, card);
                }
            }
        }

        public static void Shuffle2(List<Card> cards)
        {
            int count = cards.Count;
            Card[] cardsArr = cards.ToArray();
            int a = 0;
            for (int i = 0; i < count / 2; i++)
            {
                // 一般的最后一张牌
                int index = cards.Count / 2 - i - 1;
                var card = cardsArr[index];

                // 依次往前移动
                for (int j = index; j < count - 1 - a - i; j++)
                {
                    cardsArr[j] = cardsArr[j + 1];
                }

                cardsArr[count - i - 1 - a] = card;
                ++a;
            }

            cards.Clear();
            cards.AddRange(cardsArr);
        }

        /// <summary>
        /// 对牌进行排序
        /// 花色升序 权重降序
        /// </summary>
        /// <param name="cards"></param>
        public static void Sort(List<Card> cards)
        {
            cards.Sort();
        }

        /// <summary>
        /// 炸弹排序
        /// 玩家手牌视图的两种排序(普通排序、炸弹排序)
        /// 炸弹提出来最后
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="jokerBefore"></param>
        public static void BoomSort(List<Card> cards, bool jokerBefore = true)
        {
            Sort(cards);

            // 分析牌的数量
            List<AnalyseResult> analyseResults = AnalyseResult.Analyse(cards);

            // 王的数量
            int jokerCount = analyseResults.Count(f => f.Weight == CardWeight.SJoker || f.Weight == CardWeight.LJoker);

            // 数量>=4的牌就是炸弹
            analyseResults = analyseResults.Where(analyseResult => analyseResult.Count >= 4).ToList();

            // 没有一个炸弹
            if (analyseResults.Count == 0)
            {
                return;
            }

            // 如果joker数量有4个，就归为炸弹.
            if (jokerCount != 4)
            {
                if (!jokerBefore)
                {
                    jokerCount = 0;
                }
            }

            // 先把炸弹按照数量进行，再把炸弹插入到所有joker的前面。
            analyseResults.Sort((a, b) =>
            {
                int x = a.Count * 100 + (int) a.Weight;
                int y = b.Count * 100 + (int) b.Weight;
                if (x > y)
                {
                    return 1;
                }

                if (x < y)
                {
                    return -1;
                }

                return 0;
            });

            // 移除炸弹牌，如果需要插入到王前面就放到前面，不需要就插入到尾巴上.
            foreach (AnalyseResult result in analyseResults)
            {
                foreach (Card card in result.Cards)
                {
                    cards.Remove(card);
                    cards.Insert(cards.Count - jokerCount, card);
                }
            }
        }

        /// <summary>
        /// 数量权重排序
        /// 一些牌型验证需要使用
        /// 比如出三带二、连三带二时、炸弹时。多的牌在前面
        /// </summary>
        /// <param name="cards"></param>
        public static void WeightSort(List<Card> cards)
        {
            Sort(cards);

            // 分析每张牌的数量
            List<AnalyseResult> analyseResults = AnalyseResult.Analyse(cards);

            // 按照数量以及权重进行牌型，数量>权重
            analyseResults.Sort();

            // 从排序结果中
            cards.Clear();
            foreach (AnalyseResult result in analyseResults)
            {
                if (result.Count > 0)
                {
                    cards.AddRange(result.Cards);
                }
            }
        }

        //=========================== 牌型验证 ===========================

        /// <summary>
        /// 单张
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsSingle(List<Card> cards)
        {
            return cards.Count == 1;
        }

        /// <summary>
        /// 对子
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsDouble(List<Card> cards)
        {
            if (cards.Count != 2)
            {
                return false;
            }

            return cards.Count == 2 && cards[0].Weight == cards[1].Weight;
        }

        /// <summary>
        /// 三张：牌点相同的三张牌（只有在手牌＜5张时才可以单出，别人也只能用三张或炸弹接牌）
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsOnlyThree(List<Card> cards)
        {
            if (cards.Count != 3)
            {
                return false;
            }

            return cards[0].Weight == cards[1].Weight == (cards[1].Weight == cards[2].Weight);
        }

        /// <summary>
        /// 三带二：牌点相同的三张牌+随便两张牌（两张牌可以为单牌）
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsThreeAndTwo(List<Card> cards)
        {
            if (cards.Count != 5)
            {
                return false;
            }

            // 3带2 可能: 33334 33333 第一种可以看成三带二，第二种是需要把频断炸弹放在频断三带二前面即可。
            return cards[0].Weight == cards[1].Weight && cards[1].Weight == cards[2].Weight
                    || cards[1].Weight == cards[2].Weight && cards[2].Weight == cards[3].Weight
                    || cards[2].Weight == cards[3].Weight && cards[3].Weight == cards[4].Weight;
        }

        /// <summary>
        /// 连三带二：牌点连续的两个（含两个）以上的三张+随意4张（6张、8张、、、，2不出现连三中）
        /// </summary>
        /// <returns></returns>
        public static bool IsTripleStraight(List<Card> cards)
        {
            // 牌数不对
            if (cards.Count < 10 || cards.Count % 5 != 0)
            {
                return false;
            }

            List<AnalyseResult> analyseResults = AnalyseResult.Analyse(cards);

            // 3张的牌有几个
            int count = analyseResults.Count(f => f.Count >= 3);

            // 3张牌的个数+每三张带二张牌的个数 刚好等于 需要验证的牌的个数，如果不等于直接false
            if (count * 3 + count * 2 != cards.Count)
            {
                return false;
            }

            // 3张的牌的权重依次递增: 3334443456需要给通过，但视图现在显示为：3333444456
            for (int i = 0; i < count - 1; i++)
            {
                // 排除2
                if (analyseResults[i + 1].Weight - analyseResults[i].Weight != 1 || analyseResults[i].Weight == CardWeight._2 ||
                    analyseResults[i + 1].Weight == CardWeight._2)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 顺子
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsStraight(List<Card> cards)
        {
            // 最多只能从3连到A。刚好12张。并且不能小于5张牌。
            if (cards.Count < 5 || cards.Count > 12)
            {
                return false;
            }

            for (int i = 0; i < cards.Count - 1; i++)
            {
                CardWeight w = cards[i].Weight;

                // 因为是升序，所以前一张权重刚好比后一张多一
                if (cards[i + 1].Weight - w != 1)
                {
                    return false;
                }

                if (w > CardWeight.A || cards[i + 1].Weight > CardWeight.A)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 连对：牌点连续的两个（含两个）以上的对子叫对顺。（2不能在连对中）
        /// 3344、667788....
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsDoubleStraight(List<Card> cards)
        {
            if (cards.Count < 4 || cards.Count % 2 != 0)
            {
                return false;
            }

            for (int i = 0; i < cards.Count; i += 2)
            {
                if (cards[i + 1].Weight != cards[i].Weight)
                {
                    return false;
                }

                // 升序 验证是否是连续的,第一张和第三张是否权重刚好多一。
                if (i < cards.Count - 2)
                {
                    if (cards[i + 2].Weight - cards[i].Weight != 1)
                    {
                        return false;
                    }

                    // 不能超过A
                    if (cards[i + 2].Weight > CardWeight.A || cards[i].Weight > CardWeight.A)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 炸弹：四个（含四个）以上相同点的牌叫炸弹，可以炸任何类型的牌，炸弹的大小 按数量及2＞A＞K＞Q＞J＞10＞9……..3排序，相同数目的炸按先出顺序比大小（如：四个3先出，则后出的四个3不能压）。
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsBoom(List<Card> cards)
        {
            // 炸弹最少4张牌, 最多12张
            if (cards.Count < 4 || cards.Count > 12) return false;

            List<AnalyseResult> analyseResults = AnalyseResult.Analyse(cards);

            // 结果不能大于3种，因为不算花色，加上大小王最多3种牌。
            if (analyseResults.Count > 3)
            {
                return false;
            }

            // 只有一种牌，并且大于等于4张
            if (analyseResults.Count == 1 && analyseResults[0].Count >= 4)
            {
                return true;
            }

            // 把王排除掉，后有且只能有一种牌。
            int jokerCount = analyseResults.Count(f => f.Weight == CardWeight.LJoker || f.Weight == CardWeight.SJoker);
            if (analyseResults.Count - jokerCount != 1)
            {
                return false;
            }

            // 并且剩下的那种牌的数量必须>=4
            foreach (AnalyseResult analyseResult in analyseResults)
            {
                // 跳过王
                if (analyseResult.Weight == CardWeight.LJoker || analyseResult.Weight == CardWeight.SJoker)
                {
                    continue;
                }

                if (analyseResult.Count < 4)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 特殊牌型，4张王，最大的8炸。
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsJokerBoom(List<Card> cards)
        {
            if (cards.Count != 4)
            {
                return false;
            }

            List<AnalyseResult> analyseResults = AnalyseResult.Analyse(cards);
            if (analyseResults.Count != 2)
            {
                return false;
            }

            // 4张王
            return analyseResults[0].Count == 2 && analyseResults[1].Count == 2 && IsJoker(analyseResults[0].Weight) &&
                    IsJoker(analyseResults[1].Weight);
        }

        /// <summary>
        /// 是否是王(小王，大王)
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static bool IsJoker(CardWeight weight)
        {
            return weight >= CardWeight.SJoker;
        }

        /// <summary>
        /// 是否是王(小王，大王)
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static bool IsJoker(Card card)
        {
            return card.Weight >= CardWeight.SJoker;
        }

        /// <summary>
        /// 玩法：五十K
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsFiveTenKing(List<Card> cards)
        {
            if (cards.Count != 3) return false;
            return cards.All(item => item.Weight == CardWeight._5 && item.Weight == CardWeight._10 && item.Weight == CardWeight.K);
        }

        public static bool TryGetCardType(this List<Card> cards, out CardType type)
        {
            type = CardType.None;
            Sort(cards);

            // 小于4张的牌
            if (cards.Count < 4)
            {
                if (IsSingle(cards))
                {
                    type = CardType.Single;
                    return true;
                }

                if (IsDouble(cards))
                {
                    type = CardType.Double;
                    return true;
                }

                if (IsOnlyThree(cards))
                {
                    type = CardType.OnlyThree;
                    return true;
                }

                return false;
            }

            // 后面都是 >=4张牌的
            if (IsJokerBoom(cards))
            {
                type = CardType.JokerBoom;
                return true;
            }

            if (IsBoom(cards))
            {
                type = CardType.Boom;
                return true;
            }

            if (IsDoubleStraight(cards))
            {
                type = CardType.DoubleStraight;
                return true;
            }

            if (IsThreeAndTwo(cards))
            {
                type = CardType.ThreeAndTwo;
                return true;
            }

            if (IsStraight(cards))
            {
                type = CardType.Straight;
                return true;
            }

            if (IsTripleStraight(cards))
            {
                type = CardType.TripleStraight;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取牌组权重
        /// 已经按照数量权重排序
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetWeight(this IReadOnlyList<Card> cards, CardType type)
        {
            int w = 0;
            switch (type)
            {
                case CardType.None:
                    break;

                case CardType.ThreeAndTwo:
                {
                    List<AnalyseResult> analyseResults = AnalyseResult.Analyse(cards);
                    analyseResults.Sort();

                    w += (int) analyseResults[0].Weight * 3;

                    break;
                }
                case CardType.TripleStraight:
                {
                    // 需要进行分析，因为特殊牌型:3333344444 3333444456 再排序上是挨着一起的。
                    List<AnalyseResult> analyseResults = AnalyseResult.Analyse(cards);
                    analyseResults.Sort();
                    w = analyseResults.Sum(f => f.Count >= 3? (int) f.Weight * 3 : 0);
                    break;
                }
                case CardType.Boom:
                    // 数量占大头 + 除王的权重值
                    w = int.MaxValue / 2 + cards.Count * 100 + cards.Where(card => !IsJoker(card)).Sum(card => (int) card.Weight);
                    break;
                case CardType.JokerBoom:
                    w = int.MaxValue;
                    break;

                // 直接求和的
                case CardType.Single:
                case CardType.Double:
                case CardType.OnlyThree:
                case CardType.DoubleStraight:
                case CardType.Straight:
                default:
                    w = cards.Sum(c => (int) c.Weight);
                    break;
            }

            return w;
        }

        public static string ToText(this IReadOnlyList<Card> cards)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            foreach (Card card in cards)
            {
                builder.Append(card + ",");
            }

            builder.Replace(',', ']', builder.Length - 1, 1);
            return builder.ToString();
        }
    }
}