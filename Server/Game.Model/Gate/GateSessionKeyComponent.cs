using System.Collections.Generic;
using ET;

namespace Bomb
{
    public class GateSessionKeyComponent: Entity
    {
        private readonly Dictionary<long, long> sessionKey = new Dictionary<long, long>();

        public void Add(long key, long uid)
        {
            this.sessionKey.Add(key, uid);
            this.TimeoutRemoveKey(key).Coroutine();
        }

        public long Get(long key)
        {
            this.sessionKey.TryGetValue(key, out var uid);
            return uid;
        }

        public void Remove(long key)
        {
            this.sessionKey.Remove(key);
        }

        private async ETVoid TimeoutRemoveKey(long key)
        {
            await TimerComponent.Instance.WaitAsync(20000);
            this.sessionKey.Remove(key);
        }
    }
}