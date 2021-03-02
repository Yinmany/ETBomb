using System.Collections.Generic;
using ET;

namespace Bomb
{
    /// <summary>
    /// 游戏控制组件
    /// </summary>
    public partial class GameController: Entity
    {
        // 最近出完牌的两个人
        public Queue<int> Win = new Queue<int>(2);

        // 是否接风
        public bool IsWindup = false;
    }
}