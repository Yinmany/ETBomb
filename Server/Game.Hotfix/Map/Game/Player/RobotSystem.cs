using System.Collections.Generic;
using ET;

namespace Bomb
{
    public static class RobotSystem
    {
        public static async void Handle(this RobotProxy self, IActorMessage message)
        {
            Player player = self.GetParent<Player>();
            switch (message)
            {
                case TurnMessage turnMessage:
                {
                    TimerComponent.Instance.NewOnceTimer(TimeHelper.Now() + RandomHelper.RandomNumber(100, 1000), (timeout) =>
                    {
                        var game = player.Room.GetComponent<GameControllerComponent>();
                        if (player.SeatIndex == turnMessage.CurrentSeat)
                        {
                            var handCards = player.GetComponent<HandCardsComponent>();
                            self.Log("决定出牌.");
                            if (game.DeskCardsType == CardsType.None || game.DeskSeat == player.SeatIndex)
                            {
                                game.Pop(player, new List<Card> { handCards.Cards[0] });
                            }
                            else
                            {
                                List<Card> prompCardsList = handCards.GetPrompt();
                                if (prompCardsList != null && prompCardsList.Count > 0)
                                {
                                    game.Pop(player, prompCardsList);
                                }
                                else
                                {
                                    player.NotPop();
                                }
                            }
                        }
                    });

                    break;
                }
            }
        }

        private static void Log(this RobotProxy self, string msg)
        {
            Player player = self.GetParent<Player>();
            ET.Log.Debug($"[{player.Room.Num}|Robot({player.SeatIndex})]: {msg}");
        }

        private static void LogError(this RobotProxy self, string msg)
        {
            Player player = self.GetParent<Player>();
            ET.Log.Error($"[{player.Room.Num}|Robot({player.SeatIndex})]: {msg}");
        }
    }
}