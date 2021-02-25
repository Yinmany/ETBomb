
// 此代码是Editor自动生成的，请不要进行修改。

using AkaUI;
using UnityEngine.UI;
using UnityEngine;


namespace Bomb.View
{
    public partial class PlayerPanel
    {
        // 字段 
		private Text _nickNameText;
		private Text _scoreText;
		private Image _firendImage;
		private Image _readyImage;
		private Image _timeImage;
		private Image _warningImage;
		private Text _pokerNumText;
		private Image _pokerImage;
		private Image _scoreImage;
		private Image _slider;
		private Image _downImg;
		private Button _headImage;
		private GameObject _playCard;

        protected override void OnInit()
        {
            base.OnInit();

            // 获取引用
			_nickNameText = View.Get<Text>("NickNameText");
			_scoreText = View.Get<Text>("ScoreText");
			_firendImage = View.Get<Image>("FirendImage");
			_readyImage = View.Get<Image>("ReadyImage");
			_timeImage = View.Get<Image>("TimeImage");
			_warningImage = View.Get<Image>("WarningImage");
			_pokerNumText = View.Get<Text>("PokerNumText");
			_pokerImage = View.Get<Image>("PokerImage");
			_scoreImage = View.Get<Image>("ScoreImage");
			_slider = View.Get<Image>("Slider");
			_downImg = View.Get<Image>("DownImg");
			_headImage = View.Get<Button>("HeadImage");
			_playCard = View.Get<GameObject>("PlayCard");

        }
    }
}
