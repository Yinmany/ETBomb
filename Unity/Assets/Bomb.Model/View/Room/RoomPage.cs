using System.Linq;
using AkaUI;
using ET;
using GameEventType;

namespace Bomb.View
{
    [UI(Preload = true)]
    public partial class RoomPage: UIPage
    {
        private PlayerPanel[] playerPanels;
        private HandCardPanel handCardPanel;

        protected override void OnCreate()
        {
            // 测试按钮面板
            this.AddPanel<TestButtonGroup>().handCards = this._handCardPanel;

            this.handCardPanel = this.AddPanel<HandCardPanel>();
            CreatePlayerPanels();

            // 房间按钮事件
            this._destroyRoomBtn.onClick.AddListener(() => { LobbyPlayer.Ins.ExitRoom(true).Coroutine(); });
            this._exitRoomBtn.onClick.AddListener(() => { LobbyPlayer.Ins.ExitRoom().Coroutine(); });
            this._readyBtn.onClick.AddListener(() =>
                    LobbyPlayer.Ins.RoomOp(RoomOpType.Ready).Coroutine());
            this._notPopBtn.onClick.AddListener(() => { LobbyPlayer.Ins.RoomOp(RoomOpType.NotPlay).Coroutine(); });

            // Player事件
            this.Subscribe<PlayerRoomEvent>(On);
            this.Subscribe<StartGameEvent>(On);
            this.Subscribe<TeamChangedEvent>(On);
            this.Subscribe<TurnGameEvent>(On);
        }

        private void On(TurnGameEvent e)
        {
            var game = Game.Scene.GetComponent<Room>().GetComponent<GameControllerComponent>();

            var panel = GetPlayerPanel(game.LastOpSeat);
            panel.ShowPopTime(false);

            // 玩家出了牌
            if (game.LastOp == GameOp.Play)
            {
                panel.ShowPopCards(this._card, game.DeskCards);
            }

            // 当前出牌的玩家
            var curPanel = GetPlayerPanel(game.CurrentSeat);
            curPanel.ClearPlayCards();

            ShowInteractionUI(false);

            // LocalPlayer出牌 显示按钮
            if (game.CurrentSeat != LocalPlayerComponent.Instance.LocalPlayerSeatIndex)
            {
                curPanel.ShowPopTime();
                return;
            }

            ShowInteractionUI();
        }

        /// <summary>
        /// 显示出牌交互UI
        /// </summary>
        /// <param name="show"></param>
        private void ShowInteractionUI(bool show = true)
        {
            this._popBtn.gameObject.SetActive(show);
            this._promptBtn.gameObject.SetActive(show);
            this._notPopBtn.gameObject.SetActive(show);
        }

        private void On(TeamChangedEvent e)
        {
            Player player = Game.Scene.GetComponent<Room>().GetSameTeam(LocalPlayerComponent.Instance.Player);
            GetPlayerPanel(player.SeatIndex).ShowTeam();
        }

        private void CreatePlayerPanels()
        {
            // 创建玩家面板
            playerPanels = new PlayerPanel[4];
            for (int i = 0; i < this.playerPanels.Length; i++)
            {
                var panel = new PlayerPanel(i);
                this.AddPanel(panel, nameof (PlayerPanel) + i);
                this.playerPanels[i] = panel;
            }
        }

        protected override void OnOpen(object args = null)
        {
            this._roomInfoText.text = $"房间号: {Game.Scene.GetComponent<Room>().Num}";
            Reset();
        }

        private void On(StartGameEvent e)
        {
            this._firendBtn.gameObject.SetActive(false);
            this._exitRoomBtn.gameObject.SetActive(false);
            this._destroyRoomBtn.gameObject.SetActive(false);

            // 创建手牌
            var handCards = LocalPlayerComponent.Instance.Player.GetComponent<HandCardsComponent>().Cards;
            CardViewHelper.CreateCards(this._card, this._handCardPanel.transform, handCards);
            this.handCardPanel.Show();

            foreach (PlayerPanel playerPanel in playerPanels)
            {
                playerPanel.StartGame();
            }
        }

        private void On(PlayerRoomEvent e)
        {
            PlayerPanel panel = GetPlayerPanel(e.Seat);
            Player player = Game.Scene.GetComponent<Room>().Get(e.Seat);
            if (e.Action == PlayerRoomEvent.ActionState.Exit)
            {
                player = null;
            }

            panel.Refresh(player);

            // LocalPlayer准备
            if (player.IsReady && e.Seat == LocalPlayerComponent.Instance.LocalPlayerSeatIndex)
            {
                LocalPlayerReady(player.IsReady);
            }
        }

        private void LocalPlayerReady(bool ready)
        {
            this._readyBtn.gameObject.SetActive(!ready);
        }

        private PlayerPanel GetPlayerPanel(int seat)
        {
            // 映射   
            int uiSeat = SeatHelper.MappingToView(LocalPlayerComponent.Instance.LocalPlayerSeatIndex, seat, this.playerPanels.Length);
            return this.playerPanels[uiSeat];
        }

        private void Reset()
        {
            this._firendBtn.gameObject.SetActive(true);
            this._exitRoomBtn.gameObject.SetActive(true);
            this._destroyRoomBtn.gameObject.SetActive(true);
            this._readyBtn.gameObject.SetActive(true);

            this._cancelReadyBtn.gameObject.SetActive(false);

            this._popBtn.gameObject.SetActive(false);
            this._notPopBtn.gameObject.SetActive(false);
            this._promptBtn.gameObject.SetActive(false);
        }
    }
}