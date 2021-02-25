using AkaUI;

namespace Bomb.View
{
    [UI(Preload = true)]
    public class JoinOrCreateDialog: UIWindow
    {
        private CreatePanel createPanel;
        private JoinPanel joinPanel;

        protected override void OnCreate()
        {
            this.createPanel = this.AddPanel<CreatePanel>();
            this.joinPanel = this.AddPanel<JoinPanel>();
        }
        
        protected override void OnOpen(object args = null)
        {
            string name = args as string;
            switch (name)
            {
                case nameof (CreatePanel):
                    this.createPanel.Show();
                    break;
                case nameof (JoinPanel):
                    this.joinPanel.Show();
                    break;
            }
        }

        protected override void OnClose(object args = null)
        {
            this.createPanel.Hidden();
            this.joinPanel.Hidden();
        }
    }
}