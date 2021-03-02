using System.Linq;
using AkaUI;
using ET;
using GameEventType;

namespace Bomb
{
    [MessageHandler]
    public class GameStartMessageHandler: AMHandler<GameStartMessage>
    {
        protected override async ETTask Run(Session session, GameStartMessage message)
        {
            var room = Game.Scene.GetComponent<Room>();
            var gameInfo = room.GetComponent<GameInfo>();
            var gameCtrl = room.GetComponent<GameController>();
            ++gameInfo.Count;

            EventBus.Publish(new GameStartEvent { GameOver = false });
            // 让UI显示队友
            EventBus.Publish(new TeamChangedEvent());
        }

      
    }
}