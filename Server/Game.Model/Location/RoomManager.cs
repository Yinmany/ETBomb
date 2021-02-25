using System.Collections.Generic;
using ET;

namespace Bomb
{
    public class RoomManager: Entity
    {
        public static RoomManager Instance { get; private set; }

        public Dictionary<long, long> Numbers { get; private set; } = new Dictionary<long, long>();

        private long _ids = 1000;
        private Queue<long> _freeIds = new Queue<long>();

        public void Awake()
        {
            Instance = this;
        }

        public long GetId()
        {
            if (!this._freeIds.TryDequeue(out var id))
            {
                id = ++this._ids;
            }

            Log.Debug($"get id={id}");
            return id;
        }

        public void Remove(long num)
        {
            Numbers.Remove(num);
            _freeIds.Enqueue(num);
            Log.Debug($"remove room: num={num}");
        }

        public void Add(long num, long id)
        {
            this.Numbers.Add(num, id);
            Log.Debug($"add room: num={num} id={id}");
        }
    }
}