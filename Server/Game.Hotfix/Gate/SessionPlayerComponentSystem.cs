using ET;

namespace Bomb
{
    public class SessionUserComponentDestroySystem: DestroySystem<GateSessionActorId>
    {
        public override void Destroy(GateSessionActorId self)
        {
            // 0 还没有在游戏服创建Player
            if (self.ActorId != 0)
            {
                // 发送断线消息
                ActorMessageHelper.SendToLocationActor(self.ActorId, new G2M_SessionDisconnect());

                Log.Debug($"Session 被销毁...");
            }
        }
    }
}