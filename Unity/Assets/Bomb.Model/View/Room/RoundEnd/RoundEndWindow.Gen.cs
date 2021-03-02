
// 此代码是Editor自动生成的，请不要进行修改。

using AkaUI;
using UnityEngine;


namespace Bomb
{
    public partial class RoundEndWindow
    {
        // 字段 
		private GameObject _card;
		private GameObject _item0;
		private GameObject _item1;
		private GameObject _item2;
		private GameObject _item3;

        protected override void OnInit()
        {
            base.OnInit();

            // 获取引用
			_card = View.Get<GameObject>("Card");
			_item0 = View.Get<GameObject>("Item0");
			_item1 = View.Get<GameObject>("Item1");
			_item2 = View.Get<GameObject>("Item2");
			_item3 = View.Get<GameObject>("Item3");

        }
    }
}
