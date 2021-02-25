using System.Collections.Generic;
using UnityEngine;

namespace Bomb
{
    public static class CardViewHelper
    {
        /// <summary>
        /// 创建LocalPlayer的手牌
        /// </summary>
        public static void CreateCards(GameObject cardPrefab, Transform parent, List<Card> cards)
        {
            foreach (Card item in cards)
            {
                var card = Object.Instantiate(cardPrefab, parent);
                var cardComponent = card.GetComponent<CardComponent>();
                cardComponent.Card = item;
                cardComponent.Show();
            }
        }
    }
}