using ET;

namespace Bomb
{
    public class PlayerAwakeSystem: AwakeSystem<Player, long, Room>
    {
        public override void Awake(Player self, long a, Room b)
        {
            self.Awake(a, b);
        }
    }
}