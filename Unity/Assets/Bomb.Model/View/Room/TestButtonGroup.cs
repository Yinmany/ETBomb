using AkaUI;
using UnityEngine;

namespace Bomb.View
{
    public partial class TestButtonGroup: UIPanel
    {
        private bool isCards = false;

        protected override void OnCreate()
        {
            this._addPlayer.onClick.AddListener(() => { LobbyPlayer.Ins.RoomOp(RoomOpType.MockPlayer_Add).Coroutine(); });

            this._removePlayer.onClick.AddListener(() => { LobbyPlayer.Ins.RoomOp(RoomOpType.MockPlayer_Remove).Coroutine(); });
            this._readyPlayer.onClick.AddListener(() => LobbyPlayer.Ins.RoomOp(RoomOpType.MockPlayer_Ready).Coroutine());
        }
    }
}