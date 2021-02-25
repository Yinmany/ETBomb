using ET;

namespace Bomb
{
    public enum TeamType: byte
    {
        A,
        B
    }

    /// <summary>
    /// 队伍组件
    /// </summary>
    public class TeamComponent: Entity
    {
        public TeamType Team { get; set; } = TeamType.A;

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            Team = TeamType.A;
        }
    }
}