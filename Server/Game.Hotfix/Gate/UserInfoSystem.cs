using ET;

namespace Bomb
{
    public class UserInfoAwakeSystem : AwakeSystem<UserInfo,long>
    {
        public override void Awake(UserInfo self, long uid)
        {
            self.Awake(uid);
        }
    }
}