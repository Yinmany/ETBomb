using System;
using ET;

namespace Bomb
{
    [ActorMessageHandler]
    public class G2M_ExitRoomHandler: AMActorLocationRpcHandler<Player, G2M_ExitRoomRequest, M2G_ExitRoomResponse>
    {
        protected override async ETTask Run(Player player, G2M_ExitRoomRequest message, M2G_ExitRoomResponse response, Action reply)
        {
            Room room = player.Room;
            if (message.IsDestroyRoom && room.UId == player.UId) // 房主可以销毁房间
            {
                room.Destroy();
            }
            else
            {
                room.Exit(player);
            }

            reply();
        }
    }
}