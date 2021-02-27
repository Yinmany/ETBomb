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
        public CardType PopCardType;

        // 桌上的牌
        public IReadOnlyList<Card> DesktopCards;

        // 桌上的牌型
        public CardType DesktopCardType;
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
            if (!CheckRuleJoker(info.PopCards, info.PopCardType))
            {
                return false;
            }

            // 三不带，需要手牌小于5张才能出
            if (info.PopCardType == CardType.OnlyThree && !CheckRuleOnlyThree(info.HandCards, info.PopCardType))
            {
                return false;
            }
            
            return Pop(info.PopCards, info.PopCardType, info.DesktopCards, info.DesktopCardType);
        }

        /// <summary>
        /// 基础出牌验证
        /// </summary>
        /// <param name="self">出的牌</param>
        /// <param name="cards">牌桌上的牌</param>
        /// <param name="selfType">出的牌的牌型</param>
        /// <param name="cardType">牌桌上的牌型</param>
        /// <returns></returns>
        public static bool Pop(this IReadOnlyList<Card> self, CardType selfType, IReadOnlyList<Card> cards, CardType cardType)
        {
            // 无法验证的牌型，直接不通过.
            if (selfType == CardType.None || cardType == CardType.None)
            {
                return false;
            }

            int a = self.GetWeight(selfType);
            int b = cards.GetWeight(cardType);

            // 炸弹通吃, 只需要权重大于桌上的牌就行。
            if (selfType == CardType.JokerBoom || selfType == CardType.Boom)
            {
                return a > b;
            }

            // 剩下的都需要牌型一致, 频断权重即可。
            if (selfType == cardType)
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
        public static bool CheckRuleOnlyThree(int handCardCount, CardType type)
        {
            return handCardCount < 5;
        }

        // 王：王分大小，可以充当任意牌；当手上有四个或四个以上相同的牌时，王才可以当“宝”用，但不能与其它牌配成五十K用，四个王是最大的八炸。  
        // 王只能当宝用，不能单独出，但4个王可以当最大的8炸出。
        public static bool CheckRuleJoker(IReadOnlyList<Card> cards, CardType type)
        {
            if (type == CardType.JokerBoom)
            {
                return true;
            }

            switch (type)
            {
                case CardType.Single:
                case CardType.Double:
                case CardType.OnlyThree:
                case CardType.ThreeAndTwo:
                case CardType.TripleStraight:
                    bool isJoker = cards.Any(CardsHelper.IsJoker);
                    if (isJoker)
                    {
                        return false;
                    }

                    break;
            }

            return true;
        }
    }
}