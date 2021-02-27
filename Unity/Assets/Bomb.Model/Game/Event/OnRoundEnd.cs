using ET;
using GameEventType;

namespace Bomb
{
    public class OnRoundEnd: AEvent<RoundEndEvent>
    {
        public override async ETTask Run(RoundEndEvent a)
        {


            await ETTask.CompletedTask;
        }
    }
}