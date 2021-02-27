using System.Linq;
using ET;

namespace Bomb
{
    public static class RoomServerSystem
    {
        public static bool TryGetSeatIndex(this Room self, out int index)
        {
            index = -1;

            for (int i = 0; i < self.Seats.Length; i++)
            {
                if (self.Seats[i] == null)
                {
                    index = i;
                    return true;
                }
            }

            return false;
        }

        public static void Enter(this Room self, Player player, int seatIndex)
        {
            player.AddComponent<HandCardsComponent>();
            player.AddComponent<TeamComponent>();

            player.Parent = self;
            player.SeatIndex = seatIndex;

            PlayerModel curModel = player.GetComponent<PlayerModel>();
            PlayerEnterRoom curMsg = new PlayerEnterRoom
            {
                SeatIndex = seatIndex,
                Info = new PlayerInfo { UId = curModel.UId, Coin = curModel.Coin, NickName = curModel.NickName, RoomCard = curModel.RoomCard }
            };

            // 把自己的加入先回复
            self.SendActor(player, curMsg);

            for (int index = 0; index < self.Seats.Length; index++)
            {
                Player item = self.Seats[index];
                if (item == null)
                {
                    continue;
                }

                // 房间中已经存在的玩家同步给加入的玩家
                PlayerModel model = item.GetComponent<PlayerModel>();
                self.SendActor(player,
                    new PlayerEnterRoom
                    {
                        SeatIndex = index,
                        Ready = item.IsReady,
                        Info = new PlayerInfo { UId = model.UId, Coin = model.Coin, NickName = model.NickName, RoomCard = model.RoomCard }
                    });

                // 把加入的玩家同步给房间中已经存在的玩家
                self.SendActor(item, curMsg);
            }

            self.Seats[seatIndex] = player;

            Log.Debug($"玩家进入房间:{player.UId}");

            self.RemoveComponent<RoomTimeout>();
        }

        public static void SendActor(this Room self, Player player, IActorMessage message)
        {
            var comp = player.GetComponent<PlayerServer>();

            if (comp.Flags == PlayerFlags.Robot)
            {
                // 处理此消息
                player.GetComponent<RobotProxy>().Handle(message);
            }

            if (!comp.IsNetSync)
            {
                return;
            }

            ActorMessageHelper.SendActor(comp.GateSessionId, message);
        }

        public static void Exit(this Room self, Player player)
        {
            // 游戏中不允许退出
            if (self.Locked)
            {
                return;
            }

            PlayerExitRoom msg = new PlayerExitRoom();
            msg.UId = player.UId;
            msg.SeatIndex = player.SeatIndex;

            for (int i = 0; i < self.Seats.Length; i++)
            {
                Player item = self.Seats[i];
                if (item == player)
                {
                    self.Seats[i] = null;
                    continue;
                }

                if (item != null)
                {
                    // 发送离开消息
                    self.SendActor(item, msg);
                }
            }

            Log.Debug($"玩家离开房间:{player.UId}");
            player.Dispose();

            self.Timeout();
        }

        public static void Timeout(this Room self)
        {
            int playerCount = self.GetRoomPlayerCount();
            if (playerCount == 0)
            {
                // 60秒超时销毁房间
                Log.Debug($"房间添加超时销毁组件:{self.Num}");
                self.AddComponent<RoomTimeout>();
            }
        }

        /// <summary>
        /// 获取玩家数量
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int GetRoomPlayerCount(this Room self)
        {
            int count = 0;
            for (int i = 0; i < self.Seats.Length; i++)
            {
                if (self.Seats[i] != null)
                {
                    ++count;
                }
            }

            return count;
        }

        public static bool TryGetSeatIndex(this Room self, long uid, out int seatIndex)
        {
            seatIndex = 1;
            for (int i = 0; i < self.Seats.Length; i++)
            {
                Player item = self.Seats[i];
                if (item == null)
                {
                    continue;
                }

                if (item.UId == uid)
                {
                    seatIndex = i;
                    return true;
                }
            }

            return false;
        }

        public static void Destroy(this Room self)
        {
            for (int i = 0; i < self.Seats.Length; i++)
            {
                Player player = self.Seats[i];
                if (player == null)
                {
                    continue;
                }

                self.SendActor(player, new RoomOpMessage { Op = RoomOpType.Destroy });
                player.Dispose();
                self.Seats[i] = null;
            }

            self.Dispose();
            Log.Debug($"销毁房间: num={self.Num}");
        }

        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="self"></param>
        /// <param name="player"></param>
        /// <param name="ready"></param>
        public static void Ready(this Room self, Player player, bool ready = true)
        {
            player.IsReady = ready;
            int seatIndex = player.SeatIndex;
            for (int i = 0; i < self.Seats.Length; i++)
            {
                var item = self.Seats[i];
                if (item != null)
                {
                    self.SendActor(item,
                        new RoomOpMessage { UId = player.UId, SeatIndex = seatIndex, Op = ready? RoomOpType.Ready : RoomOpType.CancelReady });
                }
            }

            // 是否能开始游戏
            bool isGameStart = self.IsReadyAll();
            if (isGameStart)
            {
                self.GameStart();
            }
        }

        private static bool IsReadyAll(this Room self)
        {
            int readyCount = 0;
            foreach (Player player in self.Seats)
            {
                if (player != null && player.IsReady)
                {
                    ++readyCount;
                }
            }

            // 全部准备
            return readyCount == self.Seats.Length;
        }

        private static void GameStart(this Room self)
        {
            GameControllerComponent game = self.GetComponent<GameControllerComponent>() ?? self.AddComponent<GameControllerComponent>();

            // 两幅牌
            game.Cards.Clear();
            CardsHelper.Spawn(game.Cards);
            CardsHelper.Spawn(game.Cards);

            self.Lock();

            game.Win.Clear();
            game.IsWindup = false;

            // 置为false
            game.IsDoubleThree = false;
            game.RoundEnd = false;

            // 洗牌
            CardsHelper.Shuffle(game.Cards);

            // 发牌每人27张
            for (int i = 0; i < self.Seats.Length; i++)
            {
                var item = self.Seats[i];
                HandCardsComponent handCardsComponent = item.GetComponent<HandCardsComponent>();
                handCardsComponent.Cards = game.Cards.GetRange(i * 27, 27).ToList();
                handCardsComponent.Cards.Sort();

                int doubleThreeCount = 0;
                foreach (Card card in handCardsComponent.Cards)
                {
                    if (game.IsFirst(card))
                    {
                        game.CurrentSeat = i;
                    }

                    if (game.IsTeamB(card))
                    {
                        ++doubleThreeCount;
                        item.GetComponent<TeamComponent>().Team = TeamType.B;
                    }
                }

                // 一局开始时，会重置此值为false。如果设置了此值为true后就不用再设置了。因为只有两个黑桃3.
                if (!game.IsDoubleThree && doubleThreeCount == 2)
                {
                    // 找到对家,作为伙伴。
                    int teamBSeatIndex = (i + 2) % 4;
                    self.Seats[teamBSeatIndex].GetComponent<TeamComponent>().Team = TeamType.B;
                    game.IsDoubleThree = true;
                }
            }

            self.SyncTeam();
            self.SyncHandCard();

            // 让首次出牌玩家成功
            game.LastOpSeat = game.DeskSeat = game.CurrentSeat;
            game.SyncGame();
            Log.Debug($"房间游戏开始:RoomNum={self.Num}");
        }

        private static void SyncHandCard(this Room self)
        {
            // 同步手牌
            HandCardsMessage msg = new HandCardsMessage();
            for (int i = 0; i < self.Seats.Length; i++)
            {
                msg.Cards.Clear();
                var item = self.Seats[i];
                var handCards = item.GetComponent<HandCardsComponent>().Cards;
                msg.Cards.AddRange(handCards.Select(f => new CardProto { Color = (int) f.Color, Weight = (int) f.Weight }));
                self.SendActor(item, msg);
            }
        }

        private static void SyncTeam(this Room self)
        {
            // 同步Team
            TeamMessage teamMsg = new TeamMessage();
            for (int i = 0; i < self.Seats.Length; i++)
            {
                var item = self.Seats[i];
                teamMsg.Team = (int) item.GetComponent<TeamComponent>().Team;
                teamMsg.SeatIndex = item.SeatIndex;

                // 把team发给每个人
                for (int j = 0; j < self.Seats.Length; j++)
                {
                    self.SendActor(self.Seats[j], teamMsg);
                }
            }
        }
    }
}