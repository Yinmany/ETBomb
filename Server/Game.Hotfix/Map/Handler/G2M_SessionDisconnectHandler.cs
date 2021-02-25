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

            if (!room.IsGame)
            {
                room.Exit(player);
            }

            await ETTask.CompletedTask;
        }
    }
}