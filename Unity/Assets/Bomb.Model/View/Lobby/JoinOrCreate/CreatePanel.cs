using AkaUI;

namespace Bomb.View
{
    public partial class CreatePanel: UIPanel
    {
        protected override void OnCreate()
        {
            this._createBtn.onClick.AddListener(() =>
            {
                LobbyPlayer.Ins.CreateRoom().Coroutine();
            });
        }
    }
}