using ET;
using GameEventType;

namespace Bomb
{
    [MessageHandler]
    public class RoundEndHandler: AMHandler<RoundEndMessage>
    {
        protected override async ETTask Run(Session session, RoundEndMessage message)
        {
            var room = Game.Scene.GetComponent<Room>();
            room.GetComponent<GameControllerComponent>().RoundEnd = true;

            EventSystem.Instance.Publish(new RoundEndEvent());
            await ETTask.CompletedTask;
        }
    }
}