using ET;

namespace Bomb
{
    public class PlayerServerAwakeSystem: AwakeSystem<PlayerServer, long>
    {
        public override void Awake(PlayerServer self, long a)
        {
            self.Awake(a);
        }
    }
}