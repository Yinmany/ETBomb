using ET;

namespace Bomb
{
    [MessageHandler]
    public class TeamMessageHandler: AMHandler<TeamMessage>
    {
        protected override async ETTask Run(Session session, TeamMessage message)
        {
            var player = Game.Scene.GetComponent<Room>().Seats[message.SeatIndex];
            player.GetComponent<TeamComponent>().Team = (TeamType) message.Team;
        }
    }
}