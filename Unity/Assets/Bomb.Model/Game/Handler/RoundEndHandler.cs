using ET;
using GameEventType;

namespace Bomb
{
    /// <summary>
    ///
    /// </summary>
    [MessageHandler]
    public class RoundEndHandler: AMHandler<RoundEndMessage>
    {
        protected override async ETTask Run(Session session, RoundEndMessage message)
        {
            ResetPlayers();
            
            EventSystem.Instance.Publish(new RoundEndEvent());

            await ETTask.CompletedTask;
        }

        /// <summary>
        /// 大于1局需要重置一下Player的状态
        /// </summary>
        private void ResetPlayers()
        {
            var room = Game.Scene.GetComponent<Room>();
            var gameInfo = room.GetComponent<GameInfo>();

            foreach (Player player in room.Players)
            {
                // 重置玩家状态
                player.IsReady = false;
                player.Action = PlayerAction.None;
                player.LastPlayCards = null;
                var n = player.GetComponent<NetworkPlayerComponent>();
                if (n != null)
                {
                    n.CardNumber = 27;
                }
            }
        }
    }
}