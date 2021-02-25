using UnityEngine;

namespace AkaUI
{
    /// <summary>
    /// 把UI放到游戏场景中
    /// 场景加载完成后，自动附加到UI框架下。
    /// </summary>
    public class UIAttachModeCanvas : MonoBehaviour
    {
        [SerializeField] private bool autoDestroy = true;

        [SerializeField] private UIConfig[] uiConfigs = null;

        private void Awake()
        {
            foreach (UIConfig item in uiConfigs)
            {
                item.Attach();
            }

            if (autoDestroy)
            {
                Destroy(gameObject);
            }
        }
    }
}