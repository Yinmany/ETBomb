using System;
using AkaUI;

namespace Bomb.View
{
    [UI(Preload = true)]
    public partial class Dialog: UIWindow
    {
        public class Args
        {
            public string Title;
            public string Content;
            public Action OkAction;

            public Args(string title, string content)
            {
                this.Title = title;
                this.Content = content;
            }

            public Args()
            {
            }
        }

        public static void Open(Args args)
        {
            Akau.Open(nameof (Dialog), args: args);
        }

        public static void Close()
        {
            Akau.Close(nameof (Dialog));
        }

        protected override void OnOpen(object args = null)
        {
            this._okBtn.gameObject.SetActive(false);
            if (!(args is Args a))
            {
                return;
            }

            this._content.text = a.Content;
            this._title.text = a.Title;
            if (a.OkAction == null)
            {
                return;
            }

            this._okBtn.onClick.AddListener(() => a.OkAction());
            this._okBtn.gameObject.SetActive(true);
        }

        protected override void OnClose(object args = null)
        {
            this._okBtn.onClick.RemoveAllListeners();
        }
    }
}