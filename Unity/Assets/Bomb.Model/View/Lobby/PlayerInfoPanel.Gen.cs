
// 此代码是Editor自动生成的，请不要进行修改。

using AkaUI;
using UnityEngine.UI;


namespace Bomb.View
{
    public partial class PlayerInfoPanel
    {
        // 字段 
		private Text _nickNameText;
		private Text _uIDText;
		private Text _coinText;
		private Text _cardText;
		private Button _coinAddBtn;
		private Button _cardAddBtn;

        protected override void OnInit()
        {
            base.OnInit();

            // 获取引用
			_nickNameText = View.Get<Text>("NickNameText");
			_uIDText = View.Get<Text>("UIDText");
			_coinText = View.Get<Text>("CoinText");
			_cardText = View.Get<Text>("CardText");
			_coinAddBtn = View.Get<Button>("CoinAddBtn");
			_cardAddBtn = View.Get<Button>("CardAddBtn");

        }
    }
}
