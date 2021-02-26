using System.Collections.Generic;
using ET;

namespace Bomb
{
    public enum PlayerAction
    {
        None,

        /// <summary>
        /// 出牌
        /// </summary>
        Play,

        /// <summary>
        /// 不出
        /// </summary>
        NotPlay
    }

    public class Player: Entity
    {
        public long UId { get; set; }

        public bool IsReady { get; set; }

        public bool IsDisconnect { get; set; }

        public Room Room { get; set; }

        public int SeatIndex { get; set; }

        // 最后一次出的牌
        public List<Card> LastPlayCards { get; set; }
        
        public PlayerAction Action { get; set; }

        public void Awake(long uid, Room room)
        {
            this.UId = uid;
            this.Room = room;
            LastPlayCards = new List<Card>();
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.UId = 0;
            this.IsReady = false;
            this.IsDisconnect = false;
            this.Room = null;
            this.SeatIndex = -1;
            this.LastPlayCards = null;
            this.Action = PlayerAction.None;
        }
    }
}