using AkaUI;
using GameEventType;

namespace Bomb
{
    public static class RoomClientSystem
    {
        public static void Enter(this Room self, Player player)
        {
            // 设置LocalPlayer
            if (player.UId == SessionComponent.Instance.UId)
            {
                LocalPlayerComponent.Instance.Player = player;
            }
            else
            {
                player.AddComponent<NetworkPlayerComponent>();
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
    }
}