using System;
using ET;

namespace Bomb
{
    [ActorMessageHandler]
    public class G2M_CreateRoomHandler: AMActorRpcHandler<Scene, G2M_CreateRoomRequest, M2G_CreateRoomResponse>
    {
        protected override async ETTask Run(Scene scene, G2M_CreateRoomRequest message, M2G_CreateRoomResponse response, Action reply)
        {
            // 获取一个房间号
            long locationInstanceId = StartSceneConfigCategory.Instance.LocationConfig.SceneId;
            L2M_GetRoomNumResponse getRoomNumResponse =
                    (L2M_GetRoomNumResponse) await ActorMessageHelper.CallActor(locationInstanceId, new M2L_GetRoomNumRequest());
            if (getRoomNumResponse.Error != ErrorCode.ERR_Success)
            {
                response.Error = getRoomNumResponse.Error;
                response.Message = getRoomNumResponse.Message;
                reply();
                return;
            }

            Room room = EntityFactory.Create<Room, long, long>(scene, getRoomNumResponse.RoomNum, message.UId);
            room.Parent = scene;
            room.AddComponent<MailBoxComponent>();
            await room.AddLocation();
            response.RoomNum = room.Num;
            reply();
            
            Log.Info($"创建房间:{room.Num}");
        }
    }
}