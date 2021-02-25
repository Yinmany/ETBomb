using System.Collections.Generic;
using AkaUI;
using UnityEngine;

namespace Bomb
{
    public partial class HandCardPanel: UIPanel
    {


        public void Show()
        {
            // 展开
            CardsHelper.ComputerCardPos(this.View.transform.childCount, 25, (i, x, y) =>
            {
                var rect = this.View.transform.GetChild(i).GetComponent<RectTransform>();
                rect.SetSiblingIndex(i);
                rect.anchoredPosition = new Vector2(x, y);
            });
        }

        public void Refresh()
        {
        }
    }
}