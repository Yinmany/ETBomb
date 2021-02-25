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
        public IReadOnlyList<Card> PopCards;

        // 出牌的牌型
        public CardsType PopCardsType;

        // 桌上的牌
        public IReadOnlyList<Card> DesktopCards;

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
        public static bool Pop(this IReadOnlyList<Card> self, CardsType selfType, IReadOnlyList<Card> cards, CardsType cardsType)
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
        public static bool CheckRuleJoker(IReadOnlyList<Card> cards, CardsType type)
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

        /// <summary>
        /// 提示出牌
        /// 根据目标牌型，提示能出的牌。
        /// </summary>
        /// <param name="handCards"></param>
        /// <param name="target"></param>
        /// <param name="targetType"></param>
        /// <param name="defaultPrompCardsList"></param>
        /// <returns></returns>
        public static List<PrompCards> Prompt(List<Card> handCards, List<Card> target, CardsType targetType,
        List<PrompCards> defaultPrompCardsList = null)
        {
            List<PrompCards> prompCardsList = defaultPrompCardsList;
            if (prompCardsList == null)
            {
                prompCardsList = new List<PrompCards>();
            }
            else
            {
                prompCardsList.Clear();
            }

            List<AnalyseResult> analyseResults = AnalyseResult.Analyse(handCards);

            switch (targetType)
            {
                case CardsType.None:
                    break;
                case CardsType.Single:
                {


                    break;
                }
                case CardsType.Double:
                {

                    break;
                }
                case CardsType.OnlyThree:
                {
                    
                    break;
                }
                case CardsType.DoubleStraight:
                {
                   

                    break;
                }
                case CardsType.ThreeAndTwo:
                {
                   

                    break;
                }
                case CardsType.TripleStraight:
                {
                    

                    break;
                }
                case CardsType.Straight:
                {
                   

                    break;
                }
                case CardsType.Boom:
                    break;
                case CardsType.JokerBoom:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof (targetType), targetType, null);
            }

            return prompCardsList;
        }

        public static void PromptBom(List<AnalyseResult> analyseResults, List<Card> handCards, List<Card> target, CardsType targetType,
        List<PrompCards> defaultPrompCardsList = null)
        {
        }
    }
}