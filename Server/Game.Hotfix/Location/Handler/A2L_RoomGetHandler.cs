using System;
using ET;

namespace Bomb
{
    [ActorMessageHandler]
    public class A2L_RoomGetHandler: AMActorRpcHandler<Scene, A2L_RoomGetRequest, L2A_RoomGetResponse>
    {
        protected override async ETTask Run(Scene unit, A2L_RoomGetRequest request, L2A_RoomGetResponse response, Action reply)
        {
            if (!RoomManager.Instance.Numbers.TryGetValue(request.RoomNum, out long id))
            {
                response.Error = GameErrorCode.ERR_NotFoundRoom;
            }

            response.RoomId = id;
            reply();

            await ETTask.CompletedTask;
        }
    }
}