using System.Threading.Tasks;
using ET;

namespace Bomb
{
    [MessageHandler]
    public class PlayerInfoRefreshHandler: AMHandler<PlayerInfoRefresh>
    {
        protected override async ETTask Run(Session session, PlayerInfoRefresh message)
        {
            PlayerBaseInfo info = null;

            // 创建大厅玩家
            if (LobbyPlayer.Ins == null)
            {
                LobbyPlayer lobbyPlayer = EntityFactory.Create<LobbyPlayer>(Game.Scene);
                info = lobbyPlayer.AddComponent<PlayerBaseInfo>();
            }
            else
            {
                info = LobbyPlayer.Ins.GetComponent<PlayerBaseInfo>();
            }

            info.UId = message.Info.UId;
            info.NickName = message.Info.NickName;
            info.RoomCard = message.Info.RoomCard;
            info.Coin = message.Info.Coin;

            LobbyPlayer.Ins.OnLobbyPlayerInfoChanged();

            await Task.CompletedTask;
        }
    }
}