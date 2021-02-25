using System;
using ET;

namespace Bomb
{
    [MessageHandler]
    public class C2G_EnterRoomHandler: AMRpcHandler<C2G_EnterRoomRequest, G2C_EnterRoomResponse>
    {
        protected override async ETTask Run(Session session, C2G_EnterRoomRequest request, G2C_EnterRoomResponse response, Action reply)
        {
            // 获取房间Id
            long locationInstanceId = StartSceneConfigCategory.Instance.LocationConfig.SceneId;
            L2A_RoomGetResponse roomGetResponse =
                    (L2A_RoomGetResponse) await ActorMessageHelper.CallActor(locationInstanceId,
                        new A2L_RoomGetRequest { RoomNum = request.RoomNum });
            if (roomGetResponse.Error != ErrorCode.ERR_Success)
            {
                response.Error = roomGetResponse.Error;
                response.Message = roomGetResponse.Message;
                reply();
                return;
            }

            // 向房间请求加入
            long uid = session.GetComponent<UserInfo>().UId;
            M2G_EnterRoomResponse joinRoomResponse =
                    (M2G_EnterRoomResponse) await ActorMessageHelper.CallLocationActor(roomGetResponse.RoomId,
                        new G2M_EnterRoomRequest { UId = uid, GateSessionId = session.InstanceId });
            response.SeatIndex = joinRoomResponse.SeatIndex;
            response.Error = joinRoomResponse.Error;
            response.Message = joinRoomResponse.Message;
            response.RoomMaster = joinRoomResponse.RoomMaster;
            if (response.Error == ErrorCode.ERR_Success)
            {
                GateSessionActorId gateSessionActorId = session.GetComponent<GateSessionActorId>() ?? session.AddComponent<GateSessionActorId>();
                gateSessionActorId.ActorId = joinRoomResponse.PlayerActorId;
            }

            reply();
        }
    }
}