namespace AkaUI
{
    /// <summary>
    /// 窗口
    /// </summary>
    public abstract class UIWindow : UIPage
    {
        protected override void OnGoBack()
        {
            if (OnClosing())
            {
                // 调用关闭窗口
                Close();
            }
        }
    }
}