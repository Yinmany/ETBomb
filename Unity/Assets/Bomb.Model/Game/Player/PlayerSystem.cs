using System.Collections.Generic;
using System.Linq;
using ET;

namespace Bomb
{
    public static class PlayerSystem
    {
        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cards"></param>
        public static async ETTask<int> Play(this Player self, List<Card> cards)
        {
            Log.Debug($"出牌:{cards.Count}");

            M2C_PlayCardResponse response = (M2C_PlayCardResponse) await SessionComponent.Instance.Session.Call(new C2M_PlayCardRequest
            {
                Cards = cards.Select(f => new CardProto() { Color = (int) f.Color, Weight = (int) f.Weight }).ToList()
            });

            if (response.Error == ErrorCode.ERR_Success)
            {
                // 移除手牌
                var handCard = self.GetComponent<HandCardsComponent>();
                handCard.Remove(cards);
            }

            return response.Error;
        }

        public static List<Card> Promp(this Player self)
        {
            return self.GetComponent<HandCardsComponent>().GetPrompt();
        }
    }
}