using System.Linq;
using AkaUI;
using Bomb.View;
using ET;
using GameEventType;

namespace Bomb
{
    [MessageHandler]
    public class TurnMessageHandler: AMHandler<TurnMessage>
    {
        protected override async ETTask Run(Session session, TurnMessage message)
        {
            Room room = Game.Scene.GetComponent<Room>();
            var game = room.GetComponent<GameController>();

            // 记录当前出牌
            game.CurrentSeat = message.CurrentSeat;
            game.LastOpSeat = message.LastOpSeat;

            Player lastPlayer = room.Get(message.LastOpSeat);
            lastPlayer.Action = (PlayerAction) message.LastOp;

            // 最后一次的操作是出的牌，才会同步牌桌的信息。
            if (lastPlayer.Action == PlayerAction.Play)
            {
                game.DeskSeat = message.DeskSeat;
                game.DeskCardType = (CardType) message.DeskCardType;
                game.DeskCards = message.DeskCards.Select(f => new Card { Color = (CardColor) f.Color, Weight = (CardWeight) f.Weight }).ToList();

                // 减少牌的数量
                if (lastPlayer != LocalPlayerComponent.Instance.Player)
                {
                    NetworkPlayerComponent networkPlayerComponent = lastPlayer.GetComponent<NetworkPlayerComponent>();
                    networkPlayerComponent.CardNumber -= game.DeskCards.Count;
                }
            }

            // 轮到LocalPlayer出牌时，需要重置提示。
            if (game.CurrentSeat == LocalPlayerComponent.Instance.Player.SeatIndex)
            {
                LocalPlayerComponent.Instance.Player.GetComponent<HandCardsComponent>().Reprompt();
            }

            EventBus.Publish(new TurnGameEvent());
        }
    }
}