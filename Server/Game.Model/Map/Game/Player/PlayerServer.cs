using ET;

namespace Bomb
{
    public enum PlayerFlags
    {
        Player, // 正常的玩家    
        Robot, // 机器人
    }

    public class PlayerServer: Entity
    {
        public long GateSessionId { get; private set; }
        public bool IsNetSync { get; set; } = true; // 是否对其发送同步数据
        private PlayerFlags _flags;

        public PlayerFlags Flags
        {
            get => this._flags;
            set
            {
                // 非玩家自动屏蔽消息的发送
                this.IsNetSync = value == PlayerFlags.Player;
                this._flags = value;
            }
        }

        public void Awake(long gateSessionId) => this.GateSessionId = gateSessionId;

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.GateSessionId = 0;
            this.IsNetSync = true;
            this._flags = PlayerFlags.Player;
        }
    }
}