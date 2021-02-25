using System;
using System.Collections.Generic;
using ET;

namespace Bomb
{
    [MessageHandler]
    public class C2R_LoginHandler: AMRpcHandler<C2R_Login, R2C_Login>
    {
        protected override async ETTask Run(Session session, C2R_Login request, R2C_Login response, Action reply)
        {
            var db = DBComponent.Instance;
            IList<AccountModel> accountModels = await db.Query<AccountModel>(f => f.Account == request.Account && f.Password == request.Password);
            AccountModel account = null;
            if (accountModels.Count > 0)
            {
                account = accountModels[0];
            }

            account ??= await this.Register(session, request.Account, request.Password);

            // 随机分配一个Gate
            StartSceneConfig config = RealmGateAddressHelper.GetGate(session.DomainZone());
            //Log.Debug($"gate address: {MongoHelper.ToJson(config)}");

            // 向gate请求一个key,客户端可以拿着这个key连接gate
            G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey) await ActorMessageSenderComponent.Instance.Call(
                config.SceneId, new R2G_GetLoginKey() { UId = account.UId });

            response.Address = config.OuterAddress;
            response.Key = g2RGetLoginKey.Key;
            response.GateId = g2RGetLoginKey.GateId;
            reply();
        }

        private async ETTask<AccountModel> Register(Session session, string account, string pwd)
        {
            AccountModel accountModel = EntityFactory.Create<AccountModel>(session.DomainScene());
            accountModel.Account = account;
            accountModel.Password = pwd;
            accountModel.UId = await DBComponent.Instance.GetIncrementId<AccountModel>();
            await DBComponent.Instance.Save(accountModel);
            return accountModel;
        }
    }
}