using AkaUI;
using GameEventType;

namespace Bomb.View
{
    [UI(Preload = true)]
    public partial class LobbyPage: UIPage
    {
        private PlayerInfoPanel playerInfoPanel;
        private FnPanel fnPanel;

        protected override void OnCreate()
        {
            this.playerInfoPanel = this.AddPanel<PlayerInfoPanel>();
            this.fnPanel = this.AddPanel<FnPanel>();

            this.Subscribe<LobbyPlayerInfoChanged>(this.On);
        }

        private void On(LobbyPlayerInfoChanged e)
        {
            playerInfoPanel.Refresh();
        }
    }
}