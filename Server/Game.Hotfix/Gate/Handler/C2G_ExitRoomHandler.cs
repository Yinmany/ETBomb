using System;
using ET;

namespace Bomb
{
    [MessageHandler]
    public class C2G_ExitRoomHandler: AMRpcHandler<C2G_ExitRoomRequest, G2C_ExitRoomResponse>
    {
        protected override async ETTask Run(Session session, C2G_ExitRoomRequest request, G2C_ExitRoomResponse response, Action reply)
        {
            var id = session.GetComponent<GateSessionActorId>();
            if (id == null)
            {
                reply();
                return;
            }

            var actorId = id.ActorId;
            var resp = (M2G_ExitRoomResponse) await ActorMessageHelper.CallLocationActor(actorId,
                new G2M_ExitRoomRequest { IsDestroyRoom = request.IsDestroyRoom });
            response.Error = resp.Error;
            response.Message = resp.Message;

            // 已经离开房间
            session.GetComponent<GateSessionActorId>().ActorId = 0;

            reply();
        }
    }
}