using ET;

namespace Bomb
{
    public class SessionComponent: Entity
    {
        public static SessionComponent Instance { get; private set; }

        public Session Session { get; private set; }

        public long UId { get; private set; }

        public void Awake(Session session, long uid)
        {
            this.Session = session;
            this.UId = uid;
            Instance = this;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            this.Session = null;
        }
    }
}