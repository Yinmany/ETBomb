using AkaUI;

namespace Bomb
{
    [UI(Preload = true)]
    public partial class LoginPage: UIPage
    {
        protected override void OnCreate()
        {
            this._loginBtn.onClick.AddListener(OnLogin);

            this._account.text = "test";
            this._password.text = "123";
        }

        private void OnLogin()
        {
            // TODO检验数据
            LoginHelper.Login(this._account.text, this._password.text).Coroutine();
        }
    }
}