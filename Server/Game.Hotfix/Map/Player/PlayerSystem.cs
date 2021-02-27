using System.Collections.Generic;

namespace Bomb
{
    public static class PlayerSystem
    {
        public static void NotPop(this Player self)
        {
            if (!self.Room.GetComponent<GameControllerComponent>().NotPop(self))
            {
                return;
            }

            self.Action = PlayerAction.NotPlay;
            self.LastPlayCards.Clear();
        }

        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cards">需要出的牌</param>
        public static bool Pop(this Player self, List<Card> cards)
        {
            return self.Room.GetComponent<GameControllerComponent>().Pop(self, cards);
        }
    }
}