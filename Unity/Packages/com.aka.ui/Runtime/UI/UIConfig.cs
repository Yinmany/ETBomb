using UnityEngine;
using UnityEngine.UI;

namespace AkaUI
{
    /// <summary>
    /// UI配置
    /// </summary>
    public class UIConfig : MonoBehaviour
    {
        public enum Layer
        {
            Hidden,
            Bottom,
            Medium,
            Top,
            TopMost
        }

        [SerializeField] public bool attachMode = false;
        [SerializeField] private bool attachedShow = false;

        [Space(15)] public Layer layer = Layer.Bottom;
        [Header("返回按钮")] public Button GoBackBtn;
        [Header("是否需要遮罩")] public bool IsMask;
        [Header("点击遮罩关闭窗口")] public bool IsMaskClose;
        [Header("遮罩颜色")] public Color Color = new Color(0.3f, 0.3f, 0.3f, 0.6f);

        private void Start()
        {
            Attach();
        }

        public void Attach()
        {
            if (attachMode)
            {
                Akau.Attach(this.gameObject, !attachedShow);
            }
        }
    }
}