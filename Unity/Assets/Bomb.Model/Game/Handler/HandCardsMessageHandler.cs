using System.Linq;
using AkaUI;
using ET;
using GameEventType;

namespace Bomb
{
    [MessageHandler]
    public class HandCardsMessageHandler: AMHandler<HandCardsMessage>
    {
        protected override async ETTask Run(Session session, HandCardsMessage message)
        {
            var localPlayer = LocalPlayerComponent.Instance.Player;
            if (message.Cards != null)
            {
                // 添加手牌
                localPlayer.GetComponent<HandCardsComponent>().Cards = message.Cards.Select(f => new Card
                {
                    Color = (CardColor) f.Color, Weight = (CardWeight) f.Weight
                }).ToList();

                EventBus.Publish(new StartGameEvent { GameOver = false });
            }

            // 让UI显示队友
            EventBus.Publish(new TeamChangedEvent());
        }
    }
}