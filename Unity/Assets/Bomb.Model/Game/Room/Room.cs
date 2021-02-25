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
        /// 是否进行游戏
        /// </summary>
        public bool IsGame { get; private set; }

        public void Awake(long roomNum, long uid)
        {
            this.Num = roomNum;
            this.UId = uid;
        }

        public void SetGameStartState()
        {
            IsGame = true;
        }
    }
}