using System.Linq;
using ET;

namespace Bomb
{
    public static class SRoomSystem
    {
        public static void Enter(this Room self, Player player)
        {
            player.Parent = self;

            PlayerModel curModel = player.GetComponent<PlayerModel>();
            PlayerEnterRoom curMsg = new PlayerEnterRoom
            {
                SeatIndex = player.SeatIndex,
                Info = new PlayerInfo { UId = curModel.UId, Coin = curModel.Coin, NickName = curModel.NickName, RoomCard = curModel.RoomCard }
            };

            // 把自己的加入先回复
            self.SendActor(player, curMsg);

            for (int index = 0; index < self.Players.Length; index++)
            {
                Player item = self.Players[index];
                if (item == null)
                {
                    continue;
                }

                // 同步房间中的玩家
                PlayerModel model = item.GetComponent<PlayerModel>();
                self.SendActor(player,
                    new PlayerEnterRoom
                    {
                        SeatIndex = index,
                        Ready = item.IsReady,
                        Info = new PlayerInfo { UId = model.UId, Coin = model.Coin, NickName = model.NickName, RoomCard = model.RoomCard }
                    });

                // 同步给其它玩家
                self.SendActor(item, curMsg);
            }

            self.Players[player.SeatIndex] = player;
            self.OnEnter(player);
        }

        public static void Exit(this Room self, Player player)
        {
            // 游戏中不允许退出
            if (self.Locked)
            {
                return;
            }

            // 发送玩家离开消息
            PlayerExitRoom msg = new PlayerExitRoom();
            msg.UId = player.UId;
            msg.SeatIndex = player.SeatIndex;

            for (int i = 0; i < self.Players.Length; i++)
            {
                Player item = self.Players[i];
                if (item == player)
                {
                    self.Players[i] = null;
                    continue;
                }

                if (item != null)
                {
                    self.SendActor(item, msg);
                }
            }

            self.OnExit(player);
            player.Dispose();
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
            for (int i = 0; i < self.Players.Length; i++)
            {
                var item = self.Players[i];
                if (item != null)
                {
                    self.SendActor(item,
                        new RoomOpMessage { UId = player.UId, SeatIndex = seatIndex, Op = ready? RoomOpType.Ready : RoomOpType.CancelReady });
                }
            }

            self.OnReady(player);
        }

        private static void OnEnter(this Room self, Player player)
        {
            player.AddComponent<HandCardsComponent>();
            player.AddComponent<TeamComponent>();

            // 移除房间超时组件
            self.RemoveComponent<RoomTimeout>();

            Log.Debug($"玩家进入房间:{player.UId}");
        }

        private static void OnExit(this Room self, Player player)
        {
            Log.Debug($"玩家离开房间:{player.UId}");
            self.AddRoomTimeout();
        }

        private static void OnReady(this Room self, Player player)
        {
            // 已经再游戏中.
            if (self.GetComponent<GameController>() != null)
            {
                return;
            }

            // 全部准备完成，添加游戏控制器。
            if (self.IsReadyAll())
            {
                self.AddComponent<GameController>();
            }
        }

        public static void SendActor(this Room self, Player player, IActorMessage message)
        {
            var comp = player.GetComponent<PlayerServer>();

            if (!comp.IsNetSync)
            {
                return;
            }

            ActorMessageHelper.SendActor(comp.GateSessionId, message);
        }

        public static bool TryGetSeatIndex(this Room self, out int index)
        {
            index = -1;

            for (int i = 0; i < self.Players.Length; i++)
            {
                if (self.Players[i] == null)
                {
                    index = i;
                    return true;
                }
            }

            return false;
        }

        public static void AddRoomTimeout(this Room self)
        {
            int playerCount = self.GetPlayerCount();
            if (playerCount != 0)
            {
                return;
            }

            // 60秒超时销毁房间
            Log.Debug($"房间添加超时销毁组件:{self.Num}");
            self.AddComponent<RoomTimeout>();
        }

        /// <summary>
        /// 获取玩家数量
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int GetPlayerCount(this Room self)
        {
            int count = 0;
            for (int i = 0; i < self.Players.Length; i++)
            {
                if (self.Players[i] != null)
                {
                    ++count;
                }
            }

            return count;
        }

        public static bool TryGetSeatIndex(this Room self, long uid, out int seatIndex)
        {
            seatIndex = 1;
            for (int i = 0; i < self.Players.Length; i++)
            {
                Player item = self.Players[i];
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
            for (int i = 0; i < self.Players.Length; i++)
            {
                Player player = self.Players[i];
                if (player == null)
                {
                    continue;
                }

                self.SendActor(player, new RoomOpMessage { Op = RoomOpType.Destroy });
                player.Dispose();
                self.Players[i] = null;
            }

            self.Dispose();
            Log.Debug($"销毁房间: num={self.Num}");
        }

        private static bool IsReadyAll(this Room self)
        {
            int readyCount = 0;
            foreach (Player player in self.Players)
            {
                if (player != null && player.IsReady)
                {
                    ++readyCount;
                }
            }

            // 全部准备
            return readyCount == self.Players.Length;
        }
    }
}