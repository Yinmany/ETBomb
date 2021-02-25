using System;
using ET;
using MongoDB.Bson;

namespace Bomb
{
    public class PlayerModelDestroySystem: DestroySystem<PlayerModel>
    {
        public override async void Destroy(PlayerModel self)
        {
            if (!self.IsDirty)
            {
                return;
            }

            try
            {
                // Player持久化
                await DBComponent.Instance.Save(self);
            }
            catch (Exception e)
            {
                Log.Error($"持久化PlayerModel异常:{self.ToJson()} {e}");
            }
        }
    }

    public static class PlayerModelSystem
    {
        public static ETTask WriteAsync(this PlayerModel self)
        {
            return DBComponent.Instance.Save(self);
        }
    }
}