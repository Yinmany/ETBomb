using ET;

namespace Bomb
{
    [MessageHandler]
    public class RoomOpMessageHandler: AMHandler<RoomOpMessage>
    {
        protected override async ETTask Run(Session session, RoomOpMessage message)
        {
            var room = Game.Scene.GetComponent<Room>();

            switch (message.Op)
            {
                case RoomOpType.Ready:
                {
                    room.Ready(message.SeatIndex);

                    break;
                }
                case RoomOpType.Destroy:
                {
                    Game.EventSystem.Publish(new GameEventType.ExitRoom());
                    break;
                }
            }
        }
    }
}