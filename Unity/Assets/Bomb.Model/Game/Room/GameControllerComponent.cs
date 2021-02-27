using System.Collections.Generic;
using ET;

namespace Bomb
{
    /// <summary>
    /// 游戏控制组件
    /// </summary>
    public partial class GameControllerComponent: Entity
    {
        // 当前出牌的玩家座位下标
        public int CurrentSeat { get; set; }

        // 第几局
        public int Count { get; set; }

        // 是否是双扣结束，双扣结束需要重新找对家(伙伴)。
        public bool IsDoubleEnd { get; set; }

        // 同个玩家拿到了两张黑桃3
        public bool IsDoubleThree { get; set; }

        // 桌上出的牌
        public List<Card> DeskCards { get; set; }

        // 桌上出的牌的座位号
        public int DeskSeat { get; set; }

        public CardType DeskCardType { get; set; }

        /// <summary>
        /// 最后操作的座位
        /// </summary>
        public int LastOpSeat { get; set; }

        public bool RoundEnd { get; set; }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            CurrentSeat = 0;
            Count = 0;
            IsDoubleEnd = false;
            DeskCards = null;
            DeskSeat = -1;
            DeskCardType = CardType.None;
            LastOpSeat = -1;
            this.RoundEnd = false;
#if SERVER
            this.Cards.Clear();
            Win.Clear();
            IsWindup = false;
#endif
        }
    }
}