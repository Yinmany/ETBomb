using System.Collections.Generic;
using System.Linq;
using ET;

namespace Bomb
{
    public static class GameControllerComponentSystem
    {
        /// <summary>
        /// 首轮检查先手玩家
        /// </summary>
        /// <param name="self"></param>
        /// <param name="seat"></param>
        /// <param name="card"></param>
        public static bool IsFirst(this GameControllerComponent self, Card card)
        {
            // 首轮发牌无需翻牌，谁先抓到黑桃3为先出牌，后面谁为上游先出牌。
            return self.Count == 0 && card.Color == CardColor.Spade && card.Weight == CardWeight._3;
        }

        public static bool IsTeamB(this GameControllerComponent self, Card card)
        {
            // 所有玩家默认A对，
            if (self.Count == 0 || self.IsDoubleEnd)
            {
                // 把黑桃3的归为B队
                if (card.Color == CardColor.Spade && card.Weight == CardWeight._3)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 同步游戏
        /// </summary>
        /// <param name="self"></param>
        /// <param name="isFirst"></param>
        public static void SyncGame(this GameControllerComponent self)
        {
            TurnMessage msg = new TurnMessage();
            msg.CurrentSeat = self.CurrentSeat;
            msg.LastOpSeat = self.LastOpSeat;
            msg.LastOp = (int) self.LastOp;

            // 最后操作是出了牌的，才同步牌桌上的牌。
            if (self.LastOp == GameOp.Play)
            {
                msg.DeskSeat = self.DeskSeat;
                msg.DeskCards = self.DeskCards.Select(f => new CardProto { Color = (int) f.Color, Weight = (int) f.Weight }).ToList();
                msg.DeskCardType = (int) self.DeskCardsType;
            }

            Room room = (Room) self.Parent;
            for (int i = 0; i < room.Seats.Length; i++)
            {
                var item = room.Seats[i];
                room.SendActor(item, msg);
            }
        }

        /// <summary>
        /// 一个玩家出牌后调用
        /// </summary>
        /// <param name="self"></param>
        public static void Turn(this GameControllerComponent self)
        {
            // 轮转到下一个人
            self.CurrentSeat = (self.CurrentSeat + 1) % 4;
            self.SyncGame();
        }

        /// <summary>
        /// 不出
        /// </summary>
        /// <param name="self"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool NotPop(this GameControllerComponent self, Player player)
        {
            // 不该此玩家操作
            if (self.CurrentSeat != player.SeatIndex)
            {
                return false;
            }

            if (self.DeskSeat == self.CurrentSeat)
            {
                return false;
            }

            self.LastOpSeat = player.SeatIndex;
            self.LastOp = GameOp.NotPlay;
            self.Turn();
            return true;
        }

        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="self"></param>
        /// <param name="player"></param>
        /// <param name="cards"></param>
        public static bool Pop(this GameControllerComponent self, Player player, List<Card> cards)
        {
            // 不该此玩家操作
            if (self.CurrentSeat != player.SeatIndex)
            {
                return false;
            }

            if (cards == null || cards.Count == 0)
            {
                return false;
            }

            // 验证出牌是否符合
            if (!CardsHelper.TryGetCardsType(cards, out CardsType type))
            {
                // 出牌失败，牌型不对.
                return false;
            }

            // 接牌，自己上次出牌没人接牌以及，自己能接上次别人出的牌，就轮转。
            // self.LastPopCardSeat == self.CurrentPopSeat 是自己的牌最大，都没有人要。
            // 一般来说都得同牌型，但炸弹能接所有牌型。
            if (self.DeskSeat != self.CurrentSeat && !PopCardHelper.Pop(new PopCheckInfo
            {
                DesktopCards = self.DeskCards,
                DesktopCardsType = self.DeskCardsType,
                HandCards = player.GetComponent<HandCardsComponent>().Cards.Count,
                PopCards = cards,
                PopCardsType = type
            }))
            {
                return false;
            }

            // 接牌成功
            self.DeskCards = cards;
            self.DeskCardsType = type;
            self.DeskSeat = player.SeatIndex;

            self.LastOp = GameOp.Play;
            self.LastOpSeat = player.SeatIndex;

            // 从玩家手牌中移除牌
            var handCards = player.GetComponent<HandCardsComponent>().Cards;
            foreach (Card card in cards)
            {
                handCards.Remove(card);
                self.Cards.Add(card);
            }

            player.Action = PlayerAction.Play;
            player.LastPlayCards = cards.ToList();

            // 下一个人出牌
            self.Turn();

            return true;
        }

        /// <summary>
        /// 算分
        /// </summary>
        /// <param name="self"></param>
        /// <param name="player"></param>
        /// <param name="cardsType"></param>
        private static void Score(this GameControllerComponent self, Player player, CardsType cardsType)
        {
        }
    }
}