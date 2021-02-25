
// 此代码是Editor自动生成的，请不要进行修改。

using AkaUI;
using UnityEngine.UI;


namespace Bomb.View
{
    public partial class Dialog
    {
        // 字段 
		private Text _title;
		private Text _content;
		private Button _okBtn;

        protected override void OnInit()
        {
            base.OnInit();

            // 获取引用
			_title = View.Get<Text>("Title");
			_content = View.Get<Text>("Content");
			_okBtn = View.Get<Button>("OkBtn");

        }
    }
}
