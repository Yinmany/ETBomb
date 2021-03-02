using AkaUI;
using UnityEngine;

namespace Bomb
{
    public partial class Item: UIPanel
    {
        private readonly GameObject _prefab;

        public Item(GameObject prefab) => this._prefab = prefab;

        public void Reflush(Player player)
        {
            this._text.text = player.GetComponent<PlayerBaseInfo>().NickName;

            var cards = player.GetComponent<HandCardsComponent>().Cards;

            // 先移除正确的
            for (int i = 0; i < this._playCard.transform.childCount; i++)
            {
                Object.Destroy(this._playCard.transform.GetChild(i).gameObject);
            }

            // 创建现在的
            CardViewHelper.CreateCards(_prefab, this._playCard.transform, cards, false);

            // 展开
            CardsHelper.ComputerCardPos(this._playCard.transform.childCount, 25, (i, x, y) =>
            {
                var rect = this._playCard.transform.GetChild(i).GetComponent<RectTransform>();
                rect.SetSiblingIndex(i);
                rect.anchoredPosition = new Vector2(x, y);
            });
        }
    }
}