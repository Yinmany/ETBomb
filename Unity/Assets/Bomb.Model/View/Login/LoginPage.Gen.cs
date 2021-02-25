
// 此代码是Editor自动生成的，请不要进行修改。

using AkaUI;
using UnityEngine.UI;


namespace Bomb
{
    public partial class LoginPage
    {
        // 字段 
		private InputField _account;
		private InputField _password;
		private Button _loginBtn;

        protected override void OnInit()
        {
            base.OnInit();

            // 获取引用
			_account = View.Get<InputField>("Account");
			_password = View.Get<InputField>("Password");
			_loginBtn = View.Get<Button>("LoginBtn");

        }
    }
}
