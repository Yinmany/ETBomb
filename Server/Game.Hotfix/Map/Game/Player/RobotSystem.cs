using System.Collections.Generic;
using System.Text;
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
                case RoundEndMessage roundEndMessage:
                {
                    self.RoundEnd = true;
                    break;
                }
                case TurnMessage turnMessage:
                {
                    if (self.RoundEnd)
                    {
                        return;
                    }

                    TimerComponent.Instance.NewOnceTimer(TimeHelper.Now() + RandomHelper.RandomNumber(100, 1000), (timeout) =>
                    {
                        var game = player.Room.GetComponent<GameControllerComponent>();
                        if (player.SeatIndex == turnMessage.CurrentSeat)
                        {
                            self.Log($"*********【机器人开始出牌】*********");
                            var handCards = player.GetComponent<HandCardsComponent>();
                            handCards.Reprompt();

                            self.Log(handCards.Cards.ToText());
                            if (game.DeskCards != null)
                            {
                                self.Log($"目标:{game.DeskCards.ToText()}|{game.DeskCardType}");
                            }

                            if (game.DeskCardType == CardType.None || game.DeskSeat == player.SeatIndex)
                            {
                                self.Log($"直接出牌:{handCards.Cards[0]}");
                                game.Pop(player, new List<Card> { handCards.Cards[0] });
                            }
                            else
                            {
                                List<Card> prompCardsList = handCards.GetPrompt();
                                if (prompCardsList != null)
                                {
                                    self.Log($"提示出牌:{prompCardsList.ToText()}");

                                    game.Pop(player, prompCardsList);
                                }
                                else
                                {
                                    self.Log($"提示出牌:没有大过桌上的牌!");
                                    player.NotPop();
                                }
                            }
                        }
                    });

                    break;
                }
            }

            await ETTask.CompletedTask;
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