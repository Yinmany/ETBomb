using System.Collections.Generic;
using AkaUI;
using ET;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Bomb.View
{
    public partial class PlayerPanel: UIPanel
    {
        private readonly int _viewSeatIndex;

        private bool _isFirstShow;

        private bool _isLocalPlayer;

        public PlayerPanel(int viewSeatIndex)
        {
            _viewSeatIndex = viewSeatIndex;
        }

        protected override void OnCreate()
        {
            this.Reset();
        }

        /// <summary>
        /// 刷新Player
        /// </summary>
        /// <param name="player"></param>
        public void Refresh(Player player)
        {
            // 重置UI
            if (player == null)
            {
                this.Reset();
                return;
            }

            if (this._isFirstShow)
            {
                this._isLocalPlayer = player.SeatIndex == LocalPlayerComponent.Instance.LocalPlayerSeatIndex;

                // 显示Score隐藏换位图片
                this._scoreImage.gameObject.SetActive(true);
                this._downImg.gameObject.SetActive(false);
                this._isFirstShow = false;
            }

            // 根据状态显示Player
            this._readyImage.gameObject.SetActive(player.IsReady);

            // 移除换位事件
            this._headImage.onClick.RemoveAllListeners();

            var playerInfo = player.GetComponent<PlayerBaseInfo>();
            this._nickNameText.text = playerInfo.NickName;
        }

        /// <summary>
        /// 该显示的显示，该隐藏的隐藏
        /// </summary>
        private void Reset()
        {
            _isFirstShow = true;
            _isLocalPlayer = false;

            this._downImg.gameObject.SetActive(true);
            this._firendImage.gameObject.SetActive(false);
            this._readyImage.gameObject.SetActive(false);
            this._timeImage.gameObject.SetActive(false);
            this._warningImage.gameObject.SetActive(false);
            this._pokerImage.gameObject.SetActive(false);
            this._pokerNumText.text = "0";
            this._nickNameText.text = "";
            this._scoreText.text = "";
            this._scoreImage.gameObject.SetActive(false);
            this._slider.gameObject.SetActive(false);

            // 没有人时，可以进行换座位。
            this._headImage.onClick.AddListener(() => { Log.Debug($"点击换座位:{this._viewSeatIndex}"); });
        }

        public void StartGame()
        {
            this._readyImage.gameObject.SetActive(false);

            // 不是LocalPlayer的显示牌数
            if (this._isLocalPlayer)
            {
                return;
            }

            this._pokerImage.gameObject.SetActive(true);
            this._pokerNumText.text = "27";
        }

        public void ShowTeam()
        {
            this._firendImage.gameObject.SetActive(true);
        }

        public void ShowPopTime(bool show = true)
        {
            this._timeImage.gameObject.SetActive(show);
        }

        public void ShowPopCards(GameObject prefab, List<Card> cards)
        {
            CardViewHelper.CreateCards(prefab, this._playCard.transform, cards);
        }

        public void ClearPlayCards()
        {
            for (int i = 0; i < this._playCard.transform.childCount; i++)
            {
                Object.Destroy(this._playCard.transform.GetChild(i).gameObject);
            }
        }
    }
}