using ET;

namespace Bomb
{
    public class PlayerAwakeSystem: AwakeSystem<Player, long, Room>
    {
        public override void Awake(Player self, long a, Room b)
        {
            self.AddComponent<ScoreComponent>();
            self.Awake(a, b);
        }
    }
}