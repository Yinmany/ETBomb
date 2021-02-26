using System;
using ET;

namespace Bomb
{
    [ActorMessageHandler]
    public class M2L_GetRoomNumHandler: AMActorRpcHandler<Scene, M2L_GetRoomNumRequest, L2M_GetRoomNumResponse>
    {
        protected override async ETTask Run(Scene unit, M2L_GetRoomNumRequest request, L2M_GetRoomNumResponse response, Action reply)
        {
            response.RoomNum = RoomManager.Instance.GetId();
            reply();
            
            await ETTask.CompletedTask;
        }
    }
}