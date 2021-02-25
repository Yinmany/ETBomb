using System;
using ET;
using MongoDB.Bson;

namespace Bomb
{
    [ActorMessageHandler]
    public class G2M_EnterRoomHandler: AMActorLocationRpcHandler<Room, G2M_EnterRoomRequest, M2G_EnterRoomResponse>
    {
        protected override async ETTask Run(Room room, G2M_EnterRoomRequest message, M2G_EnterRoomResponse response, Action reply)
        {
            if (!room.TryGetSeatIndex(out int index))
            {
                response.Error = GameErrorCode.ERR_RoomNotSeat;
                reply();
                return;
            }

            // 获取用户信息
            PlayerModel playerModel = await DBComponent.Instance.QueryOne<PlayerModel>(f => f.UId == message.UId);
            Player player = EntityFactory.Create<Player, long, Room>(room.Domain, message.UId, room);
            player.AddComponent<PlayerServer, long>(message.GateSessionId);
            player.AddComponent(playerModel);
            player.AddComponent<MailBoxComponent>();
            await player.AddLocation();
            Log.Debug($"Player: Model={playerModel.ToJson()} ");

            response.PlayerActorId = player.Id;
            response.SeatIndex = index;
            response.RoomMaster = room.UId;

            // 先把自己的座位号回给客户端，客户端好以此进行显示。
            reply();

            // 会进行房间内的玩家同步
            room.Enter(player, index);
        }
    }
}