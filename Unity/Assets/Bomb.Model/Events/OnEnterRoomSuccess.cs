using AkaUI;
using Bomb.View;
using ET;
using GameEventType;

namespace Bomb
{
    public class OnEnterRoomSuccess: AEvent<EnterRoomSuccess>
    {
        public override async ETTask Run(EnterRoomSuccess a)
        {
            // 创建一个房间
            Room room = Game.Scene.AddComponent<Room, long, long>(a.RoomNum, a.RoomMaster);
            room.AddComponent<GameController>();
            room.AddComponent<LocalPlayerComponent>().LocalPlayerSeatIndex = a.CurrentPlayerSeatIndex;

            // 进入房间UI
            Akau.Open(nameof (RoomPage));
        }
    }
}