using ET;

namespace Bomb
{
    public class RoomTimeoutAwakeSystem: AwakeSystem<RoomTimeout>
    {
        public override void Awake(RoomTimeout self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 房间超时销毁组件
    /// </summary>
    public class RoomTimeout: Entity
    {
        private long timeoutId;

        public void Awake()
        {
            TimerComponent.Instance.NewOnceTimer(TimeHelper.Now() + 60 * 1000, b => { this.Parent.Dispose(); });
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            TimerComponent.Instance.Remove(this.timeoutId);
            this.timeoutId = 0;
        }
    }
}