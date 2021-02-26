using Bomb.View;
using ET;
using GameEventType;

namespace Bomb
{
    public class OnRoundEnd: AEvent<RoundEndEvent>
    {
        public override async ETTask Run(RoundEndEvent a)
        {
            var args = new Dialog.Args("游戏", "游戏结束");
            args.OkAction = () => { Dialog.Close(); };
            Dialog.Open(args);

            await ETTask.CompletedTask;
        }
    }
}