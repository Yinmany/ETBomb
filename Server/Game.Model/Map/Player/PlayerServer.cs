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

        public PlayerFlags Flags { get; set; }

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
            this.Flags = PlayerFlags.Player;
        }
    }
}