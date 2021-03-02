using System.Collections.Generic;
using ET;

namespace Bomb
{
    /// <summary>
    /// 游戏控制组件
    /// </summary>
    public partial class GameController: Entity
    {
        // 当前出牌的玩家座位下标
        public int CurrentSeat;

        // 桌上出的牌
        public List<Card> DeskCards;

        // 桌上出的牌的座位号
        public int DeskSeat;

        public CardType DeskCardType;

        /// <summary>
        /// 最后操作的座位
        /// </summary>
        public int LastOpSeat;

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            CurrentSeat = 0;
            DeskCards = null;
            DeskSeat = -1;
            DeskCardType = CardType.None;

            LastOpSeat = -1;
#if SERVER
            Win.Clear();
            IsWindup = false;
#endif
        }
    }
}