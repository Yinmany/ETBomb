using System;
using AkaUI;
using Bomb.View;
using ET;
using GameEventType;

namespace Bomb
{
    public static class LoginHelper
    {
        public static async ETVoid Login(string account, string password)
        {
            try
            {
                NetworkLoading.Open(true);

                // 创建一个ETModel层的Session
                R2C_Login r2CLogin;
                using (Session session = Game.Scene.GetComponent<NetOuterComponent>().Create("127.0.0.1:10002"))
                {
                    r2CLogin = (R2C_Login) await session.Call(new C2R_Login() { Account = account, Password = password });
                }

                // 创建一个gate Session,并且保存到SessionComponent中
                Session gateSession = Game.Scene.GetComponent<NetOuterComponent>().Create(r2CLogin.Address);

                G2C_LoginGate g2CLoginGate =
                        (G2C_LoginGate) await gateSession.Call(new C2G_LoginGate() { Key = r2CLogin.Key, GateId = r2CLogin.GateId });
                Game.Scene.AddComponent<SessionComponent, Session, long>(gateSession, g2CLoginGate.UId);

                if (g2CLoginGate.Error == ErrorCode.ERR_Success)
                {
                    OnLoginSuccess(g2CLoginGate);
                }
                else
                {
                    Dialog.Open(new Dialog.Args { Title = "登录", Content = $"登录失败:{g2CLoginGate.Error}" });
                }
            }
            catch (Exception e)
            {
                Dialog.Open(new Dialog.Args { Title = "网络连接", Content = "无法连接网络!" });
            }
            finally
            {
                NetworkLoading.Close();
            }
        }

        private static void OnLoginSuccess(G2C_LoginGate data)
        {
            EventSystem.Instance.Publish(new LoginSuccess());
        }
    }
}