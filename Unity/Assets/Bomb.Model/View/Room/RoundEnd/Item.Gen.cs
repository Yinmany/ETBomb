
// 此代码是Editor自动生成的，请不要进行修改。

using AkaUI;
using UnityEngine.UI;
using UnityEngine;


namespace Bomb
{
    public partial class Item
    {
        // 字段 
		private Text _text;
		private GameObject _playCard;

        protected override void OnInit()
        {
            base.OnInit();

            // 获取引用
			_text = View.Get<Text>("Text");
			_playCard = View.Get<GameObject>("PlayCard");

        }
    }
}
