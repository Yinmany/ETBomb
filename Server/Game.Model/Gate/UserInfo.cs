using ET;

namespace Bomb
{
    public class UserInfo: Entity
    {
        public long UId { get; private set; }

        public void Awake(long uid) => this.UId = uid;
    }
}