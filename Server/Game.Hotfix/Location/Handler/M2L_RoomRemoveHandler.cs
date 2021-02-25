using ET;

namespace Bomb
{
    [ActorMessageHandler]
    public class M2L_RoomRemoveHandler: AMActorHandler<Scene, M2L_RoomRemoveMessage>
    {
        protected override async ETTask Run(Scene entity, M2L_RoomRemoveMessage message)
        {
            RoomManager.Instance.Remove(message.RoomNum);
        }
    }
}