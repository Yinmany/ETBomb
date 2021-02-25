using UnityEngine.UI;

namespace AkaUI
{
    /// <summary>
    /// 页面
    /// </summary>
    public abstract class UIPage : UIBase
    {
        private Button _btnGoBack;

        protected override void OnInit()
        {
            _btnGoBack = this.UIConfig.GoBackBtn;
            if (_btnGoBack != null)
            {
                _btnGoBack.onClick.AddListener(OnGoBack);
            }
        }

        protected virtual void OnGoBack()
        {
        }

        protected override void OnCreate()
        {
        }

        protected override void OnOpen(object args = null)
        {
        }

        protected override bool OnClosing(object args = null) => true;

        protected override void OnClose(object args = null)
        {
        }

        protected override void OnDestroy()
        {
 
        }
    }
}