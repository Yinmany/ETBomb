using AkaUI;
using GameEventType;

namespace Bomb
{
    public static class RoomSystem
    {
        public static void Enter(this Room self, Player player)
        {
            // 设置LocalPlayer
            if (player.UId == SessionComponent.Instance.UId)
            {
                LocalPlayerComponent.Instance.Player = player;
            }

            self.Seats[player.SeatIndex] = player;

            EventBus.Publish(new PlayerRoomEvent { Action = PlayerRoomEvent.ActionState.Enter, Seat = player.SeatIndex });
        }

        public static void Exit(this Room self, int seat)
        {
            EventBus.Publish(new PlayerRoomEvent { Action = PlayerRoomEvent.ActionState.Exit, Seat = seat });
            Player player = self.Seats[seat];
            player?.Dispose();
        }

        public static void Ready(this Room self, int seat)
        {
            self.Seats[seat].IsReady = true;
            EventBus.Publish(new PlayerRoomEvent { Action = PlayerRoomEvent.ActionState.Ready, Seat = seat });
        }

        public static Player Get(this Room self, int seat)
        {
            return self.Seats[seat];
        }

        /// <summary>
        /// 获取同组Player
        /// </summary>
        /// <param name="self"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static Player GetSameTeam(this Room self, Player player)
        {
            var team = player.GetComponent<TeamComponent>().Team;
            for (int i = 0; i < self.Seats.Length; i++)
            {
                var item = self.Seats[i];
                if (item != player && item.GetComponent<TeamComponent>().Team == team)
                {
                    return item;
                }
            }

            return null;
        }
    }
}