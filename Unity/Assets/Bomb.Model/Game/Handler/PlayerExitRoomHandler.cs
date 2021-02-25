using AkaUI;
using ET;

namespace Bomb
{
    [MessageHandler]
    public class PlayerExitRoomHandler: AMHandler<PlayerExitRoom>
    {
        protected override async ETTask Run(Session session, PlayerExitRoom message)
        {
            Game.Scene.GetComponent<Room>().Exit(message.SeatIndex);
        }
    }
}