using AkaUI;
using Bomb.View;
using ET;
using GameEventType;

namespace Bomb
{
    public class OnExitRoom: AEvent<ExitRoom>
    {
        public override async ETTask Run(ExitRoom a)
        {
            Game.Scene.RemoveComponent<Room>();
            Akau.Open(nameof (LobbyPage));
        }
    }
}