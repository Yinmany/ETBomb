
// 此代码是Editor自动生成的，请不要进行修改。

using AkaUI;
using UnityEngine.UI;
using UnityEngine;


namespace Bomb.View
{
    public partial class RoomPage
    {
        // 字段 
		private Button _cancelReadyBtn;
		private GameObject _card;
		private Button _destroyRoomBtn;
		private Button _exitRoomBtn;
		private Button _firendBtn;
		private GameObject _handCardPanel;
		private Button _notPopBtn;
		private Button _popBtn;
		private Button _promptBtn;
		private Button _readyBtn;
		private Text _roomInfoText;

        protected override void OnInit()
        {
            base.OnInit();

            // 获取引用
			_cancelReadyBtn = View.Get<Button>("CancelReadyBtn");
			_card = View.Get<GameObject>("Card");
			_destroyRoomBtn = View.Get<Button>("DestroyRoomBtn");
			_exitRoomBtn = View.Get<Button>("ExitRoomBtn");
			_firendBtn = View.Get<Button>("FirendBtn");
			_handCardPanel = View.Get<GameObject>("HandCardPanel");
			_notPopBtn = View.Get<Button>("NotPopBtn");
			_popBtn = View.Get<Button>("PopBtn");
			_promptBtn = View.Get<Button>("PromptBtn");
			_readyBtn = View.Get<Button>("ReadyBtn");
			_roomInfoText = View.Get<Text>("RoomInfoText");

        }
    }
}
