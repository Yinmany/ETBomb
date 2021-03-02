using System.Collections.Generic;
using ET;

namespace Bomb
{
    [ActorMessageHandler]
    public class TurnMessageHandler: AMActorHandler<Player, TurnMessage>
    {
        protected override async ETTask Run(Player player, TurnMessage message)
        {
            var game = player.Room.GetComponent<GameController>();

            // 一局已结束.
            if (game == null)
            {
                return;
            }

            if (player.SeatIndex != message.CurrentSeat)
            {
                return;
            }

            var robot = player.GetComponent<RobotProxy>();
            robot.Log($"*********【机器人开始出牌】*********");
            var handCards = player.GetComponent<HandCardsComponent>();
            handCards.Reprompt();

            robot.Log(handCards.Cards.ToText());

            if (game.DeskCards != null)
            {
                robot.Log($"目标:{game.DeskCards.ToText()}|{game.DeskCardType}");
            }

            if (game.DeskCardType == CardType.None || game.DeskSeat == player.SeatIndex)
            {
                robot.Log($"直接出牌:{handCards.Cards[0]}");
                game.Pop(player, new List<Card> { handCards.Cards[0] });
            }
            else
            {
                List<Card> prompCardsList = handCards.GetPrompt();
                if (prompCardsList != null)
                {
                    robot.Log($"提示出牌:{prompCardsList.ToText()}");

                    bool b = game.Pop(player, prompCardsList);
                    if (!b)
                    {
                        player.NotPop();
                    }
                }
                else
                {
                    robot.Log($"提示出牌:没有大过桌上的牌!");
                    player.NotPop();
                }
            }

            await ETTask.CompletedTask;
        }
    }
}