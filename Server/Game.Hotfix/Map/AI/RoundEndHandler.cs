using ET;

namespace Bomb
{
    // [ActorMessageHandler]
    public class RoundEndHandler: AMActorHandler<Player, RoundEndMessage>
    {
        protected override async ETTask Run(Player player, RoundEndMessage message)
        {
            var robot = player.GetComponent<RobotProxy>();
            robot.Log($"*********【机器人游戏结束】*********");

            await ETTask.CompletedTask;
        }
    }
}