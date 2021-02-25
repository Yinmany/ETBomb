using System;
using ET;

namespace Bomb
{
    [MessageHandler]
    public class C2G_LoginGateHandler: AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
    {
        protected override async ETTask Run(Session session, C2G_LoginGate request, G2C_LoginGate response, Action reply)
        {
            Scene scene = Game.Scene.Get(request.GateId);
            if (scene == null)
            {
                return;
            }

            long uid = scene.GetComponent<GateSessionKeyComponent>().Get(request.Key);
            if (uid == 0)
            {
                response.Error = ErrorCode.ERR_ConnectGateKeyError;
                response.Message = "Gate key验证失败!";
                reply();
                return;
            }

            // 获取用户信息
            PlayerModel playerModel = await DBComponent.Instance.QueryOne<PlayerModel>(f => f.UId == uid);

            // 没有进行初始化过
            if (playerModel == null)
            {
                playerModel = EntityFactory.Create<PlayerModel>(session.DomainScene());
                playerModel.UId = uid;
                playerModel.Coin = 0;
                playerModel.RoomCard = 100;
                await playerModel.WriteAsync();
            }

            session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);
            session.AddComponent<UserInfo, long>(playerModel.UId);
            session.Send(new PlayerInfoRefresh
            {
                Info = new PlayerInfo
                {
                    UId = playerModel.UId, NickName = playerModel.NickName, Coin = playerModel.Coin, RoomCard = playerModel.RoomCard
                }
            });

            response.UId = uid;
            reply();
        }
    }
}