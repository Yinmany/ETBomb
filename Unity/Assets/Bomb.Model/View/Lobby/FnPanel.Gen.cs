
// 此代码是Editor自动生成的，请不要进行修改。

using AkaUI;
using UnityEngine.UI;


namespace Bomb.View
{
    public partial class FnPanel
    {
        // 字段 
		private Button _joinBtn;
		private Button _createBtn;

        protected override void OnInit()
        {
            base.OnInit();

            // 获取引用
			_joinBtn = View.Get<Button>("JoinBtn");
			_createBtn = View.Get<Button>("CreateBtn");

        }
    }
}
