using ET;
using MongoDB.Bson.Serialization.Attributes;

namespace Bomb
{
    /// <summary>
    /// 玩家信息
    /// </summary>
    public class PlayerModel: Entity, ISerializeToEntity
    {
        /// <summary>
        /// UserId
        /// </summary>
        public long UId { get; set; }

        public string NickName { get; set; }

        public int Coin { get; set; }
        public int RoomCard { get; set; }

        /// <summary>
        /// 如果为True就需要保存
        /// </summary>
        [BsonIgnore]
        public bool IsDirty { get; set; }
    }
}