using AkaUI;

namespace Bomb.View
{
    [UI(Preload = true)]
    public partial class NetworkLoading: UIWindow
    {
        protected override void OnOpen(object args = null)
        {
            bool isMask = (bool) args;
            this._image.SetActive(isMask);
        }

        public static void Open(bool mask = false)
        {
            Akau.Open(nameof (NetworkLoading), false, mask);
        }

        public static void Close()
        {
            Akau.Close(nameof (NetworkLoading));
        }
    }
}