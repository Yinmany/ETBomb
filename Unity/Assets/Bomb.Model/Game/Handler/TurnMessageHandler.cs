using System.Linq;
using AkaUI;
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
            var game = room.GetComponent<GameControllerComponent>();

            // 记录当前出牌
            game.CurrentSeat = message.CurrentSeat;
            game.LastOpSeat = message.LastOpSeat;
            game.LastOp = (GameOp) message.LastOp;

            // 最后一次的操作是出的牌，才会同步牌桌的信息。
            if (game.LastOp == GameOp.Play)
            {
                game.DeskSeat = message.DeskSeat;
                game.DeskCardType = (CardType) message.DeskCardType;
                game.DeskCards = message.DeskCards.Select(f => new Card { Color = (CardColor) f.Color, Weight = (CardWeight) f.Weight }).ToList();

                // 减少牌的数量
                var player = room.Get(game.DeskSeat);
                if (player != LocalPlayerComponent.Instance.Player)
                {
                    NetworkPlayerComponent networkPlayerComponent = player.GetComponent<NetworkPlayerComponent>();
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