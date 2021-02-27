using System;
using System.Collections.Generic;
using System.Linq;
using AkaUI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Bomb
{
    public partial class HandCardPanel: UIPanel
    {
        private readonly List<CardComponent> _selectedCards = new List<CardComponent>();

        /// <summary>
        /// 显示手牌
        /// </summary>
        public void Reflush()
        {
            // 展开
            CardsHelper.ComputerCardPos(this.View.transform.childCount, 25, (i, x, y) =>
            {
                var rect = this.View.transform.GetChild(i).GetComponent<RectTransform>();
                rect.SetSiblingIndex(i);
                rect.anchoredPosition = new Vector2(x, y);
            });
        }

        /// <summary>
        /// 进行出牌网络请求时对手牌进行锁定
        /// </summary>
        public void Lock()
        {
            CardComponent.Lock = true;
        }

        /// <summary>
        /// 解锁
        /// </summary>
        public void UnLock()
        {
            CardComponent.Lock = false;
        }

        /// <summary>
        /// 获取选中的牌
        /// </summary>
        /// <returns></returns>
        public List<Card> GetSelectedCards()
        {
            this._selectedCards.Clear();
            for (int i = 0; i < this.View.transform.childCount; i++)
            {
                var cardComp = this.View.transform.GetChild(i).GetComponent<CardComponent>();
                if (cardComp.IsSelect)
                {
                    _selectedCards.Add(cardComp);
                }
            }

            return this._selectedCards.Select(f => f.Card).ToList();
        }

        /// <summary>
        /// 出牌成功后，移除出的牌
        /// </summary>
        public void RemoveSelectedCard()
        {
            foreach (CardComponent cardComponent in this._selectedCards)
            {
                cardComponent.transform.SetParent(null);
                Object.Destroy(cardComponent.gameObject);
            }

            if (this._selectedCards.Count > 0)
            {
                Reflush();
            }

            this._selectedCards.Clear();
        }

        public void SelectPrompt(List<Card> cards)
        {
            for (int i = 0; i < this.View.transform.childCount; i++)
            {
                var item = this.View.transform.GetChild(i).GetComponent<CardComponent>();
                item.NotCardSelect();
            }

            foreach (Card card in cards)
            {
                for (int i = 0; i < this.View.transform.childCount; i++)
                {
                    var item = this.View.transform.GetChild(i).GetComponent<CardComponent>();
                    if (!item.IsSelect && item.Card.Equals(card))
                    {
                        item.CardSelect();
                        break;
                    }
                }
            }
        }

        public void Reset()
        {
            _selectedCards.Clear();
            for (int i = 0; i < this.View.transform.childCount; i++)
            {
                Object.Destroy(this.View.transform.GetChild(i).gameObject);
            }
        }
    }
}