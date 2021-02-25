using ET;

namespace Bomb
{
    public class OnPreloadCompleted: AEvent<PreloadCompleted>
    {
        public override async ETTask Run(PreloadCompleted a)
        {
            await ETTask.CompletedTask;
        }
    }
}