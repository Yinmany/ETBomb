using AkaUI;

namespace Bomb.View
{
    [UI]
    public partial class LoadingPage: UIPage
    {
        protected override void OnCreate()
        {
            this.Subscribe<ResUpdateEvent>(On);
        }

        private void On(ResUpdateEvent e)
        {
            if (e.IsPreload)
            {
                this._tips.text = $"正在加载[{e.PreloadName}]资源...";
            }
            else
            {
                this._image.fillAmount = e.Percent;
                this._tips.text = $"下载[packages]资源: {(int) (e.Percent * 100)}%";
            }
        }
    }
}