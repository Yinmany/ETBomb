using ET;

namespace Bomb
{
    /// <summary>
    /// 挂在客户端的房间上，用于记录LocalPlayer.
    /// </summary>
    public class LocalPlayerComponent: Entity
    {
        public static LocalPlayerComponent Instance { get; private set; }

        public Player Player { get; set; }

        public int LocalPlayerSeatIndex { get; set; }

        public void Awake()
        {
            Instance = this;
        }
    }

    public class LocalPlayerComponentAwakeSystem: AwakeSystem<LocalPlayerComponent>
    {
        public override void Awake(LocalPlayerComponent self)
        {
            self.Awake();
        }
    }
}