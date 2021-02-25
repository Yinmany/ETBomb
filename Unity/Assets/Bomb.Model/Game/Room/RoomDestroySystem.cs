using ET;

namespace Bomb
{
    public class RoomDestroySystem: DestroySystem<Room>
    {
        public override void Destroy(Room self)
        {
            Log.Debug($"销毁房间:num={self.Num}, master={self.UId}");
#if SERVER
            long locationInstanceId = StartSceneConfigCategory.Instance.LocationConfig.SceneId;
            ActorMessageHelper.SendActor(locationInstanceId, new M2L_RoomRemoveMessage { RoomNum = self.Num });
#endif
        }
    }
}