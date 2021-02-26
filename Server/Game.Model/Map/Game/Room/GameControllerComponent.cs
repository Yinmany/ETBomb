using System.Collections.Generic;
using ET;

namespace Bomb
{
    /// <summary>
    /// 游戏控制组件
    /// </summary>
    public partial class GameControllerComponent: Entity
    {
        // 两幅牌
        public List<Card> Cards = new List<Card>(108);
        public Queue<int> Win = new Queue<int>(2);
        public bool IsWindup = false;
    }
}