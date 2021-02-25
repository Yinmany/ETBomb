using AkaUI;

namespace Bomb.View
{
    public partial class FnPanel: UIPanel
    {
        protected override void OnCreate()
        {
            this._joinBtn.onClick.AddListener(() => Akau.Open(nameof (JoinOrCreateDialog), args: nameof (JoinPanel)));
            this._createBtn.onClick.AddListener(() => Akau.Open(nameof (JoinOrCreateDialog), args: nameof (CreatePanel)));
        }
    }
}