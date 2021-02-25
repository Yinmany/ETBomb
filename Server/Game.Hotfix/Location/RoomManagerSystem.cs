using ET;

namespace Bomb
{
    public class RoomManagerAwakeSystem : AwakeSystem<RoomManager>
    {
        public override void Awake(RoomManager self)
        {
            self.Awake();
        }
    }
}