using ET;

namespace Bomb
{
    public class RoomAwakeSystem: AwakeSystem<Room, long, long>
    {
        public override void Awake(Room self, long num, long uid)
        {
            self.Awake(num, uid);
            self.AddComponent<GameInfo>();
            Log.Debug($"创建房间:num={num}, master={uid}");

#if SERVER
           long locationInstanceId = StartSceneConfigCategory.Instance.LocationConfig.SceneId;
           ActorMessageHelper.SendActor(locationInstanceId, new M2L_RoomAddMessage { RoomId = self.Id, RoomNum = self.Num });
#endif
        }
    }
}