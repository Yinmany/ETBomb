using System;
using System.Collections.Generic;
using System.Linq;

namespace Bomb
{
    public struct PopCheckInfo
    {
        // 手牌个数
        public int HandCards;

        // 出的牌
        public List<Card> PopCards;

        // 出牌的牌型
        public CardsType PopCardsType;

        // 桌上的牌
        public List<Card> DesktopCards;

        // 桌上的牌型
        public CardsType DesktopCardsType;
    }

    /// <summary>
    /// 出牌
    /// </summary>
    public static class PopCardHelper
    {
        public static bool Pop(PopCheckInfo info)
        {
            // 前置验证

            // 不能单独出王
            if (!CheckRuleJoker(info.PopCards, info.PopCardsType))
            {
                return false;
            }

            // 三不带，需要手牌小于5张才能出
            if (info.PopCardsType == CardsType.OnlyThree && !CheckRuleOnlyThree(info.HandCards, info.PopCardsType))
            {
                return false;
            }

            return Pop(info.PopCards, info.PopCardsType, info.DesktopCards, info.DesktopCardsType);
        }

        /// <summary>
        /// 基础出牌验证
        /// </summary>
        /// <param name="self">出的牌</param>
        /// <param name="cards">牌桌上的牌</param>
        /// <param name="selfType">出的牌的牌型</param>
        /// <param name="cardsType">牌桌上的牌型</param>
        /// <returns></returns>
        public static bool Pop(this List<Card> self, CardsType selfType, List<Card> cards, CardsType cardsType)
        {
            // 无法验证的牌型，直接不通过.
            if (selfType == CardsType.None || cardsType == CardsType.None)
            {
                return false;
            }

            int a = self.GetWeight(selfType);
            int b = cards.GetWeight(cardsType);

            // 炸弹通吃, 只需要权重大于桌上的牌就行。
            if (selfType == CardsType.JokerBoom || selfType == CardsType.Boom)
            {
                return a > b;
            }

            // 剩下的都需要牌型一致, 频断权重即可。
            if (selfType == cardsType)
            {
                return a > b;
            }

            return false;
        }

        /// <summary>
        /// 三张：牌点相同的三张牌（只有在手牌＜5张时才可以单出，别人也只能用三张或炸弹接牌）  
        /// </summary>
        /// <param name="handCardCount">手牌个数</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool CheckRuleOnlyThree(int handCardCount, CardsType type)
        {
            return handCardCount < 5;
        }

        // 王：王分大小，可以充当任意牌；当手上有四个或四个以上相同的牌时，王才可以当“宝”用，但不能与其它牌配成五十K用，四个王是最大的八炸。  
        // 王只能当宝用，不能单独出，但4个王可以当最大的8炸出。
        public static bool CheckRuleJoker(List<Card> cards, CardsType type)
        {
            if (type == CardsType.JokerBoom)
            {
                return true;
            }

            switch (type)
            {
                case CardsType.Single:
                case CardsType.Double:
                case CardsType.OnlyThree:
                case CardsType.ThreeAndTwo:
                case CardsType.TripleStraight:
                    bool isJoker = cards.Any(CardsHelper.IsJoker);
                    if (isJoker)
                    {
                        return false;
                    }

                    break;
            }

            return true;
        }

        public struct PrompCards
        {
            public CardsType CardsType;
            public List<Card> Cards;
        }

        /// <summary>
        /// 提示出牌
        /// 根据目标牌型，提示能出的牌。
        /// </summary>
        /// <param name="handCards"></param>
        /// <param name="target"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static List<PrompCards> Prompt(List<Card> handCards, List<Card> target, CardsType targetType)
        {
            List<PrompCards> prompCardsList = new List<PrompCards>();

            List<AnalyseResult> analyseResults = AnalyseResult.Analyse(handCards);

            // 添加炸弹
            void AddBoom(List<AnalyseResult> list1, List<PrompCards> prompCardsList1)
            {
                // 1.搜寻炸弹
                //      a.先确定王的个数
                //      b.大于等于4张的炸弹
                //      c.王与炸弹的所有组合
                //      d.按照炸弹权重排序
                List<AnalyseResult> jokerAnalyseResults = list1.Where(f => CardsHelper.IsJoker(f.Weight)).ToList();
                int jokerCount = jokerAnalyseResults.Sum(f => f.Count);

                // 不包括王,如果王有4张，就再最后的时候，把4张王单独添加。
                List<AnalyseResult> boomResults = list1.Where(f => f.Count >= 4 && !CardsHelper.IsJoker(f.Weight)).ToList();

                for (int i = 0; i < boomResults.Count; i++)
                {
                    var result = boomResults[i];

                    // 验证此牌是否能接目标牌型
                    if (Pop(new PopCheckInfo
                    {
                        HandCards = handCards.Count,
                        PopCards = result.Cards,
                        PopCardsType = CardsType.Boom,
                        DesktopCards = target,
                        DesktopCardsType = targetType
                    }))
                    {
                        prompCardsList1.Add(new PrompCards { CardsType = CardsType.Boom, Cards = result.Cards });
                    }

                    // 组合王
                    var boomCards = result.Cards.ToList();

                    for (int j = 0; j < jokerAnalyseResults.Count; j++)
                    {
                        AnalyseResult analyseResult = jokerAnalyseResults[j];
                        for (int k = 0; k < analyseResult.Count; k++)
                        {
                            var item = analyseResult.Cards[k];
                            boomCards.Add(item);
                            // 每添加一个王，就向提示列表中，添加一种。
                            if (Pop(new PopCheckInfo
                            {
                                HandCards = handCards.Count,
                                PopCards = boomCards,
                                PopCardsType = CardsType.Boom,
                                DesktopCards = target,
                                DesktopCardsType = targetType
                            }))
                            {
                                prompCardsList1.Add(new PrompCards { CardsType = CardsType.Boom, Cards = result.Cards.ToList() });
                            }
                        }
                    }
                }

                // 4张王，算最大的炸弹. 单独添加进提示列表
                if (jokerCount == 4)
                {
                    var jokerBoomCards = new List<Card>();
                    for (int i = 0; i < jokerAnalyseResults.Count; i++)
                    {
                        jokerBoomCards.AddRange(jokerAnalyseResults[i].Cards);
                    }

                    prompCardsList1.Add(new PrompCards { CardsType = CardsType.JokerBoom, Cards = jokerBoomCards });
                }
            }

            // 局部方法，找出可带的牌。
            List<Tuple<Card, Card>> Tuples(List<AnalyseResult> analyseResults1)
            {
                // 找出除王的单牌
                List<AnalyseResult> oneAnalyseResults = analyseResults1.Where(f => f.Count == 1 && !CardsHelper.IsJoker(f.Weight)).ToList();

                // 找出除王的对子
                List<AnalyseResult> twoAnalyseResults = analyseResults1.Where(f => f.Count == 2 && !CardsHelper.IsJoker(f.Weight)).ToList();

                // 可带的牌
                List<Tuple<Card, Card>> cards = new List<Tuple<Card, Card>>();

                // 单牌与对子的拆分组合
                for (int i = 0; i < oneAnalyseResults.Count; i++)
                {
                    Card a = oneAnalyseResults[i].Cards[0];
                    Card b = default;
                    int next = i + 1;

                    // 单牌没有了，拆分对子替补。
                    if (next > oneAnalyseResults.Count - 1)
                    {
                        if (twoAnalyseResults.Count == 0)
                        {
                            break;
                        }

                        b = twoAnalyseResults[0].Cards[0];
                    }
                    else
                    {
                        b = oneAnalyseResults[next].Cards[0];
                    }

                    cards.Add(new Tuple<Card, Card>(a, b));
                }

                // 对子
                for (int i = 0; i < twoAnalyseResults.Count; i++)
                {
                    cards.Add(new Tuple<Card, Card>(twoAnalyseResults[i].Cards[0], twoAnalyseResults[i].Cards[1]));
                }

                return cards;
            }

            // 局部方法
            void AddAnalyseResultToPrompCards(List<AnalyseResult> doubleAnalyseResults, int multiple)
            {
                foreach (AnalyseResult doubleAnalyseResult in doubleAnalyseResults)
                {
                    // 如果是对子就只添加到2的倍数
                    // 3/2=1*2=2 4/2=2*2=4
                    int count = doubleAnalyseResult.Count / multiple * multiple;
                    var list = doubleAnalyseResult.Cards.GetRange(0, count);

                    if (Pop(new PopCheckInfo
                    {
                        HandCards = handCards.Count,
                        PopCards = list,
                        PopCardsType = targetType,
                        DesktopCards = target,
                        DesktopCardsType = targetType
                    }))
                    {
                        prompCardsList.Add(new PrompCards { Cards = list, CardsType = targetType });
                    }
                }
            }

            switch (targetType)
            {
                case CardsType.None:
                    break;
                case CardsType.Single:
                {
                    foreach (Card card in handCards)
                    {
                        var list = new List<Card> { card };
                        if (Pop(new PopCheckInfo
                        {
                            HandCards = handCards.Count,
                            PopCards = list,
                            PopCardsType = targetType,
                            DesktopCards = target,
                            DesktopCardsType = targetType
                        }))
                        {
                            prompCardsList.Add(new PrompCards { Cards = list, CardsType = targetType });
                        }
                    }

                    break;
                }
                case CardsType.Double:
                {
                    // 符合2张的所有牌类
                    List<AnalyseResult> doubleAnalyseResults = analyseResults.Where(f => f.Count >= 2).ToList();
                    AddAnalyseResultToPrompCards(doubleAnalyseResults, 2);
                    break;
                }
                case CardsType.OnlyThree:
                {
                    // 符合3张的所有牌类
                    List<AnalyseResult> doubleAnalyseResults = analyseResults.Where(f => f.Count >= 3).ToList();
                    AddAnalyseResultToPrompCards(doubleAnalyseResults, 3);
                    break;
                }
                case CardsType.DoubleStraight:
                {
                    // 搜索连续的连队
                    //  1.确定目标是几对
                    //  2.根据目标对子搜索连续的牌型
                    int targetNum = target.Count / 2;

                    // 找到两个以上的牌
                    List<AnalyseResult> doubleAnalyseResults = analyseResults.Where(f => f.Count >= 2).ToList();

                    // 手中牌不够
                    if (doubleAnalyseResults.Count < targetNum)
                    {
                        break;
                    }

                    List<Card> tmpCards = new List<Card>();
                    for (int i = 0; i < doubleAnalyseResults.Count - 1; i++)
                    {
                        tmpCards.Clear();
                        for (int j = i; j < targetNum + i; j++)
                        {
                            tmpCards.AddRange(doubleAnalyseResults[j].Cards.GetRange(0, 2));
                        }

                        if (Pop(new PopCheckInfo
                        {
                            HandCards = handCards.Count, PopCards = tmpCards, DesktopCardsType = targetType, DesktopCards = target,
                        }))
                        {
                            prompCardsList.Add(new PrompCards { Cards = tmpCards.ToList(), CardsType = targetType });
                        }
                    }

                    break;
                }
                case CardsType.ThreeAndTwo:
                {
                    // 1.找出3张的牌，多于3张的牌提示为炸弹.
                    // 2.找出单牌,找出对子.排除王
                    // 3.优先带单牌，单牌不足拆对子。没有单牌时，就带对子.都没有由玩家决定带的牌。

                    // 3张的牌，大于3张的牌就是属于炸弹了。
                    List<AnalyseResult> threeAnalyseResults = analyseResults.Where(f => f.Count == 3).ToList();

                    var cards = Tuples(analyseResults);

                    // 生成提示牌
                    for (int i = 0; i < threeAnalyseResults.Count; i++)
                    {
                        // 可带牌为0(因为只从单张和对子中进行带牌),只用生成大于牌桌上的3张牌，余下2张带牌由玩家挑选。
                        if (cards.Count == 0)
                        {
                            List<Card> tmpCards = new List<Card>();
                            // 添加3张的牌
                            tmpCards.AddRange(threeAnalyseResults[i].Cards);

                            // 添加两张最小用于逻辑的牌
                            tmpCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
                            tmpCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });

                            if (!Pop(new PopCheckInfo
                            {
                                HandCards = handCards.Count, PopCards = tmpCards, DesktopCardsType = targetType, DesktopCards = target,
                            }))
                            {
                                continue;
                            }

                            // 移除逻辑牌
                            tmpCards.RemoveAt(tmpCards.Count - 1);
                            tmpCards.RemoveAt(tmpCards.Count - 1);

                            prompCardsList.Add(new PrompCards { Cards = tmpCards, CardsType = targetType });
                        }
                        else
                        {
                            for (int j = 0; j < cards.Count; j++)
                            {
                                (Card item1, Card item2) = cards[j];

                                List<Card> tmpCards = new List<Card>();

                                // 添加3张的牌
                                tmpCards.AddRange(threeAnalyseResults[i].Cards);

                                // 添加的带的牌
                                tmpCards.Add(item1);
                                tmpCards.Add(item2);

                                if (Pop(new PopCheckInfo
                                {
                                    HandCards = handCards.Count, PopCards = tmpCards, DesktopCardsType = targetType, DesktopCards = target,
                                }))
                                {
                                    prompCardsList.Add(new PrompCards { Cards = tmpCards, CardsType = targetType });
                                }
                            }
                        }
                    }

                    break;
                }
                case CardsType.TripleStraight:
                {
                    // 几个连3带2
                    int targetNum = target.Count / 5;

                    // 3张的牌，大于3张的牌就是属于炸弹了。
                    List<AnalyseResult> threeAnalyseResults = analyseResults.Where(f => f.Count == 3).ToList();

                    // 牌不够
                    if (targetNum > threeAnalyseResults.Count)
                    {
                        break;
                    }

                    // 带的牌
                    var cards = Tuples(analyseResults);

                    for (int i = 0; i < threeAnalyseResults.Count - 1; i++)
                    {
                        List<Card> tmpCards = new List<Card>();

                        // 每次往后找targetNum个牌
                        for (int j = i; j < targetNum + i; j++)
                        {
                            tmpCards.AddRange(threeAnalyseResults[j].Cards.GetRange(0, 3));
                        }

                        // 带的牌不够
                        if (cards.Count < targetNum)
                        {
                            // 添加最小用于逻辑的牌
                            for (int j = 0; j < targetNum * 2; j++)
                            {
                                tmpCards.Add(new Card { Color = CardColor.Club, Weight = CardWeight._3 });
                            }

                            if (!Pop(new PopCheckInfo
                            {
                                HandCards = handCards.Count, PopCards = tmpCards, DesktopCardsType = targetType, DesktopCards = target,
                            }))
                            {
                                continue;
                            }

                            tmpCards.RemoveRange(targetNum * 3 - 1, targetNum * 2);
                            prompCardsList.Add(new PrompCards { Cards = tmpCards, CardsType = targetType });
                        }
                        else
                        {
                            // 每两张都和
                            for (int j = 0; j < cards.Count - 1; j++)
                            {
                                for (int k = j; k < targetNum + j; k++)
                                {
                                    tmpCards.Add(cards[k].Item1);
                                    tmpCards.Add(cards[k].Item2);
                                }
                            }

                            if (Pop(new PopCheckInfo
                            {
                                HandCards = handCards.Count, PopCards = tmpCards, DesktopCardsType = targetType, DesktopCards = target,
                            }))
                            {
                                prompCardsList.Add(new PrompCards { Cards = tmpCards, CardsType = targetType });
                            }
                        }
                    }

                    break;
                }
                case CardsType.Straight:
                {
                    for (int i = 0; i < analyseResults.Count - 1; i++)
                    {
                        List<Card> tmpCards = new List<Card>();
                        for (int j = i; j < target.Count + i; j++)
                        {
                            tmpCards.Add(analyseResults[j].Cards[0]);
                        }

                        if (Pop(new PopCheckInfo
                        {
                            HandCards = handCards.Count, PopCards = tmpCards, DesktopCardsType = targetType, DesktopCards = target,
                        }))
                        {
                            prompCardsList.Add(new PrompCards { Cards = tmpCards, CardsType = targetType });
                        }
                    }

                    break;
                }
                case CardsType.Boom:
                    break;
                case CardsType.JokerBoom:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof (targetType), targetType, null);
            }

            AddBoom(analyseResults, prompCardsList);

            return prompCardsList;
        }
    }
}