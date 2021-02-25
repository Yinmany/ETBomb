
// 此代码是Editor自动生成的，请不要进行修改。

using AkaUI;
using UnityEngine.UI;


namespace Bomb.View
{
    public partial class LoadingPage
    {
        // 字段 
		private Image _image;
		private Text _tips;

        protected override void OnInit()
        {
            base.OnInit();

            // 获取引用
			_image = View.Get<Image>("Image");
			_tips = View.Get<Text>("Tips");

        }
    }
}
