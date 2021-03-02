
// 此代码是Editor自动生成的，请不要进行修改。

using AkaUI;
using UnityEngine.UI;


namespace Bomb.View
{
    public partial class TestButtonGroup
    {
        // 字段 
		private Button _addPlayer;
		private Button _removePlayer;
		private Button _readyPlayer;
		private InputField _inputField;
		private Button _postPlayer;

        protected override void OnInit()
        {
            base.OnInit();

            // 获取引用
			_addPlayer = View.Get<Button>("AddPlayer");
			_removePlayer = View.Get<Button>("RemovePlayer");
			_readyPlayer = View.Get<Button>("ReadyPlayer");
			_inputField = View.Get<InputField>("InputField");
			_postPlayer = View.Get<Button>("PostPlayer");

        }
    }
}
