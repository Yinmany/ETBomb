using System.Collections.Generic;
using ET;

namespace Bomb
{
    public enum GameOp: byte
    {
        None,
        Play,
        NotPlay
    }

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

        public CardsType DeskCardsType { get; set; }

        /// <summary>
        /// 最后操作的座位
        /// </summary>
        public int LastOpSeat { get; set; }

        /// <summary>
        /// 最后的操作
        /// </summary>
        public GameOp LastOp { get; set; }
    }
}