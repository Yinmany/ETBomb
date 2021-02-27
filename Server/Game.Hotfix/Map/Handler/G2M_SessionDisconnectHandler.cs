using ET;

namespace Bomb
{
    [ActorMessageHandler]
    public class G2M_SessionDisconnectHandler: AMActorLocationHandler<Player, G2M_SessionDisconnect>
    {
        protected override async ETTask Run(Player player, G2M_SessionDisconnect message)
        {
            Room room = player.Room;

            Log.Debug($"玩家下线了...");
            player.IsDisconnect = true;

            player.GetComponent<PlayerServer>().IsNetSync = false;

            // 如果房间没有锁定，直接让掉线玩家退出房间。
            if (!room.Locked)
            {
                room.Exit(player);
            }

            await ETTask.CompletedTask;
        }
    }
}