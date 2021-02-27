using ET;

namespace Bomb
{
    public class Room: Entity
    {
        /// <summary>
        /// 房间号
        /// </summary>
        public long Num { get; private set; }

        /// <summary>
        /// 房主
        /// </summary>
        public long UId { get; private set; }

        /// <summary>
        /// 房间内的每个玩家
        /// </summary>
        public Player[] Seats = new Player[4];

        /// <summary>
        /// 是否已锁定
        /// 游戏开始时，锁定房间。
        /// 游戏结束时，解锁房间。
        /// </summary>
        public bool Locked { get; private set; }

        public void Awake(long roomNum, long uid)
        {
            this.Num = roomNum;
            this.UId = uid;
        }

        public void Lock()
        {
            this.Locked = true;
        }

        public void Unlock()
        {
            this.Locked = false;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            Num = 0;
            UId = 0;

            foreach (Player player in this.Seats)
            {
                player?.Dispose();
            }

            Locked = false;
        }
    }
}