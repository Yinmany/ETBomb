using ET;

namespace Bomb
{
    [ActorMessageHandler]
    public class M2L_RoomAddHandler: AMActorHandler<Scene, M2L_RoomAddMessage>
    {
        protected override async ETTask Run(Scene scene, M2L_RoomAddMessage message)
        {
            RoomManager.Instance.Add(message.RoomNum, message.RoomId);
            await ETTask.CompletedTask;
        }
    }
}