using AkaUI;

namespace Bomb.View
{
    public partial class PlayerInfoPanel: UIPanel
    {
        protected override void OnCreate()
        {
        }

        public void Refresh()
        {
            var info = LobbyPlayer.Ins.GetComponent<PlayerBaseInfo>();
            this._uIDText.text = info.UId.ToString();
            _nickNameText.text = info.NickName;
            this._coinText.text = info.Coin.ToString();
            this._cardText.text = info.RoomCard.ToString();
        }
    }
}