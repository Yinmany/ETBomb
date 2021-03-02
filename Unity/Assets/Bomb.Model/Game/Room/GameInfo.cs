using System.Collections.Generic;
using ET;

namespace Bomb
{
    public class GameInfo: Entity
    {
        // 第几局
        public int Count;

        // 是否是双扣结束，双扣结束需要重新找对家(伙伴)。
        public bool IsDoubleEnd = true;

        // 上一局最先出完牌的人
        public int PrevWinSeat;

        // 两幅牌
        public List<Card> Cards = new List<Card>(108);

        // 炸弹封顶个数
        public int BoomTop = 9;

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            this.Count = 0;
            this.IsDoubleEnd = true;
            this.Cards.Clear();
        }
    }
}