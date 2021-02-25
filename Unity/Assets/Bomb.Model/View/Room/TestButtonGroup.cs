using AkaUI;
using UnityEngine;

namespace Bomb.View
{
    public partial class TestButtonGroup: UIPanel
    {
        public GameObject handCards;

        private bool isCards = false;

        protected override void OnCreate()
        {
            this._addPlayer.onClick.AddListener(() => { LobbyPlayer.Ins.RoomOp(RoomOpType.MockPlayer_Add).Coroutine(); });

            this._removePlayer.onClick.AddListener(() => { LobbyPlayer.Ins.RoomOp(RoomOpType.MockPlayer_Remove).Coroutine(); });
            this._readyPlayer.onClick.AddListener(() => LobbyPlayer.Ins.RoomOp(RoomOpType.MockPlayer_Ready).Coroutine());
            this._testCards.onClick.AddListener(() =>
            {
                if (!this.isCards)
                {
                    // 展开
                    CardsHelper.ComputerCardPos(this.handCards.transform.childCount, 25, (i, x, y) =>
                    {
                        var rect = this.handCards.transform.GetChild(i).GetComponent<RectTransform>();
                        rect.SetSiblingIndex(i);
                        rect.anchoredPosition = new Vector2(x, y);
                    });
                }
            });
        }
    }
}