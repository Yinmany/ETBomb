
// 此代码是Editor自动生成的，请不要进行修改。

using AkaUI;
using UnityEngine;
using UnityEngine.UI;


namespace Bomb.View
{
    public partial class JoinPanel
    {
        // 字段 
		private GameObject _content;
		private Button _clearBtn;
		private Button _joinBtn;
		private Text _text;

        protected override void OnInit()
        {
            base.OnInit();

            // 获取引用
			_content = View.Get<GameObject>("Content");
			_clearBtn = View.Get<Button>("ClearBtn");
			_joinBtn = View.Get<Button>("JoinBtn");
			_text = View.Get<Text>("Text");

        }
    }
}
