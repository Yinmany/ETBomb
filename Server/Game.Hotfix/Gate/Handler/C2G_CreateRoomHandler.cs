using System;
using ET;

namespace Bomb
{
    [MessageHandler]
    public class C2G_CreateRoomHandler: AMRpcHandler<C2G_CreateRoomRequest, G2C_CreateRoomResponse>
    {
        protected override async ETTask Run(Session session, C2G_CreateRoomRequest request, G2C_CreateRoomResponse response, Action reply)
        {
            long uid = session.GetComponent<UserInfo>().UId;

            // 地图SceneActorId
            long mapInstanceId = SceneHelper.RandomMapsInstanceId(session.DomainZone());
            M2G_CreateRoomResponse createRoomResponse =
                    (M2G_CreateRoomResponse) await ActorMessageSenderComponent.Instance.Call(mapInstanceId,
                        new G2M_CreateRoomRequest() { UId = uid });
            response.Error = createRoomResponse.Error;
            response.Message = createRoomResponse.Message;
            response.RoomNum = createRoomResponse.RoomNum;
            reply();
        }
    }
}