using System.Linq;
using ET;

namespace Bomb
{
    [MessageHandler]
    public class HandCardMessageHandler: AMHandler<HandCardMessage>
    {
        protected override async ETTask Run(Session session, HandCardMessage message)
        {
            var player = Game.Scene.GetComponent<Room>().Get(message.Seat);

            var cards = player.GetComponent<HandCardsComponent>().Cards;
            cards.Clear();

            // 添加手牌
            cards.AddRange(message.Cards.Select(f => new Card { Color = (CardColor) f.Color, Weight = (CardWeight) f.Weight }).ToList());
        }
    }
}