using ET;

namespace Bomb
{
    public class SessionComponentAwakeSystem: AwakeSystem<SessionComponent, Session, long>
    {
        public override void Awake(SessionComponent self, Session session, long uid)
        {
            self.Awake(session, uid);
        }
    }

    public class SessionComponentDestroySystem: DestroySystem<SessionComponent>
    {
        public override void Destroy(SessionComponent self)
        {
            self.Session.Dispose();
            Log.Info($"断开连接...");
        }
    }
}