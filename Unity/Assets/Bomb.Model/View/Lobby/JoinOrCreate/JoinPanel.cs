using AkaUI;
using UnityEngine;
using UnityEngine.UI;

namespace Bomb.View
{
    public partial class JoinPanel: UIPanel
    {
        protected override void OnCreate()
        {
            foreach (Transform tf in this._content.transform)
            {
                string name = tf.name;
                if (name.StartsWith("N"))
                {
                    tf.GetComponent<Button>().onClick.AddListener(() => { this._text.text += name.Substring(1); });
                }
            }

            this._clearBtn.onClick.AddListener(() => this._text.text = "");
            this._joinBtn.onClick.AddListener(() => { LobbyPlayer.Ins.EnterRoom(int.Parse(this._text.text)).Coroutine(); });
        }
    }
}