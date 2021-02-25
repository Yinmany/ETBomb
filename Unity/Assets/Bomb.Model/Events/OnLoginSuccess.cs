using AkaUI;
using Bomb.View;
using ET;
using GameEventType;

namespace Bomb
{
    public class OnLoginSuccess: AEvent<LoginSuccess>
    {
        public override async ETTask Run(LoginSuccess a)
        {
            // 登录成功,跳转UI
            Akau.Open(nameof (LobbyPage), true);
            await ETTask.CompletedTask;
        }
    }
}