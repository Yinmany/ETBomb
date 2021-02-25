using ET;

namespace Bomb
{
    [MessageHandler]
    public class PlayerEnterRoomHandler: AMHandler<PlayerEnterRoom>
    {
        protected override async ETTask Run(Session session, PlayerEnterRoom message)
        {
            Room room = Game.Scene.GetComponent<Room>();

            // 创建Player
            Player p = EntityFactory.Create<Player, long, Room>(Game.Scene, message.Info.UId, room);
            p.AddComponent<HandCardsComponent>();
            p.AddComponent<TeamComponent>();

            // Player信息
            var playerInfo = p.AddComponent<PlayerBaseInfo>();
            playerInfo.UId = message.Info.UId;
            playerInfo.NickName = message.Info.NickName;
            
            // 进入房间
            p.SeatIndex = message.SeatIndex;
            p.IsReady = message.Ready;
            room.Enter(p);
        }
    }
}