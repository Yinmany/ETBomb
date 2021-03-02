using AkaUI;
using ET;
using GameEventType;

namespace Bomb
{
    [UI(Preload = true)]
    public partial class RoundEndWindow: UIWindow
    {
        private Item[] items = new Item[4];

        protected override void OnCreate()
        {
            for (int i = 0; i < this.items.Length; i++)
            {
                var item = new Item(this._card);
                this.AddPanel(item, nameof (Item) + i);
                this.items[i] = item;
            }
        }

        protected override void OnOpen(object args = null)
        {
            Room room = Game.Scene.GetComponent<Room>();
            foreach (Player player in room.Players)
            {
                int viewSeat = SeatHelper.MappingToView(LocalPlayerComponent.Instance.LocalPlayerSeatIndex, player.SeatIndex, this.items.Length);
                items[viewSeat].Reflush(player);
            }
        }

        protected override void OnClose(object args = null)
        {
            EventBus.Publish(new GameStartEvent { GameOver = true });
        }
    }
}