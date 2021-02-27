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
        public static void SyncGame(this GameControllerComponent self)
        {
            Room room = (Room) self.Parent;
            Player lastPlayer = room.Seats[self.LastOpSeat];

            TurnMessage msg = new TurnMessage();
            msg.CurrentSeat = self.CurrentSeat;
            msg.LastOpSeat = self.LastOpSeat;
            msg.LastOp = (int) lastPlayer.Action;
            msg.GameOver = self.RoundEnd;

            // 最后操作是出了牌的，才同步牌桌上的牌。
            if (lastPlayer.Action == PlayerAction.Play)
            {
                msg.DeskSeat = self.DeskSeat;
                msg.DeskCards = self.DeskCards.Select(f => new CardProto { Color = (int) f.Color, Weight = (int) f.Weight }).ToList();
                msg.DeskCardType = (int) self.DeskCardType;
            }

            for (int i = 0; i < room.Seats.Length; i++)
            {
                var item = room.Seats[i];
                if (item != null)
                {
                    room.SendActor(item, msg);
                }
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
            var room = self.GetParent<Room>();
            var player = room.Seats[self.CurrentSeat];
            if (player.GetComponent<HandCardsComponent>().Cards.Count == 0)
            {
                if (self.DeskSeat == player.SeatIndex && !self.IsWindup)
                {
                    // 又轮到出完牌的玩家，就把出牌权给朋友。接风
                    Player friend = room.GetSameTeam(player);
                    self.CurrentSeat = friend.SeatIndex;
                    self.DeskCards.Clear();
                    self.DeskSeat = self.CurrentSeat;
                    self.DeskCardType = CardType.None;

                    self.IsWindup = true;
                }
                else
                {
                    // 跳过此玩家
                    self.CurrentSeat = (self.CurrentSeat + 1) % 4;
                }
            }

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
            if (self.IsGameEnd())
            {
                return false;
            }

            // 不该此玩家操作
            if (self.CurrentSeat != player.SeatIndex)
            {
                return false;
            }

            if (self.DeskSeat == self.CurrentSeat)
            {
                return false;
            }

            // 记录Player最后一次的操作，断线重连后需要恢复每个玩家的最近的操作。
            self.LastOpSeat = player.SeatIndex;
            player.Action = PlayerAction.NotPlay;
            player.LastPlayCards?.Clear();

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
            if (self.IsGameEnd())
            {
                return false;
            }

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
            if (!CardsHelper.TryGetCardType(cards, out CardType type))
            {
                // 出牌失败，牌型不对.
                return false;
            }

            // 特殊排序
            if (type == CardType.ThreeAndTwo || type == CardType.TripleStraight)
            {
                // 权重排序
                CardsHelper.WeightSort(cards);
            }

            // 接牌，自己上次出牌没人接牌以及，自己能接上次别人出的牌，就轮转。
            // self.LastPopCardSeat == self.CurrentPopSeat 是自己的牌最大，都没有人要。
            // 一般来说都得同牌型，但炸弹能接所有牌型。
            if (self.DeskSeat != self.CurrentSeat && !PopCardHelper.Pop(new PopCheckInfo
            {
                DesktopCards = self.DeskCards,
                DesktopCardType = self.DeskCardType,
                HandCards = player.GetComponent<HandCardsComponent>().Cards.Count,
                PopCards = cards,
                PopCardType = type
            }))
            {
                return false;
            }

            // 接牌成功
            self.DeskCards = cards;
            self.DeskCardType = type;
            self.DeskSeat = player.SeatIndex;

            self.LastOpSeat = player.SeatIndex;

            // 从玩家手牌中移除牌
            var handCards = player.GetComponent<HandCardsComponent>();
            handCards.Remove(cards);

            foreach (Card card in cards)
            {
                self.Cards.Add(card);
            }

            // 记录Player最后一次的操作，断线重连后需要恢复每个玩家的最近的操作。
            player.Action = PlayerAction.Play;
            player.LastPlayCards = cards.ToList();

            if (handCards.Cards.Count == 0)
            {
                self.Win.Enqueue(player.SeatIndex);
                if (self.Win.Count == 2) // 一局结束
                {
                    OnRoundEnd(self, player);
                }
            }

            // 下一个人出牌
            self.Turn();

            return true;
        }

        private static bool IsGameEnd(this GameControllerComponent self)
        {
            return self.RoundEnd;
        }

        private static void OnRoundEnd(this GameControllerComponent self, Player player)
        {
            self.RoundEnd = true;

            int a = self.Win.Dequeue();
            Room room = self.GetParent<Room>();

            // 双扣，需要重新找伙伴
            if (room.Seats[a].GetComponent<TeamComponent>().Team == player.GetComponent<TeamComponent>().Team)
            {
                self.IsDoubleEnd = true;
                Log.Debug($"一局结束:双扣...");
            }
            else
            {
                Log.Debug($"一局结束...");
            }

            for (int i = 0; i < room.Seats.Length; i++)
            {
                var p = room.Seats[i];
                room.SendActor(p, new RoundEndMessage());
            }

            // 不在进行游戏...
            room.IsGame = false;
        }

        /// <summary>
        /// 算分
        /// </summary>
        /// <param name="self"></param>
        /// <param name="player"></param>
        /// <param name="cardType"></param>
        private static void Score(this GameControllerComponent self, Player player, CardType cardType)
        {
        }
    }
}