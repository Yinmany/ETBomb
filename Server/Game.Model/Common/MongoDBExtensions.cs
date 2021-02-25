using ET;
using MongoDB.Driver;

namespace Bomb
{
    /// <summary>
    /// MongoDb自增Id
    /// </summary>
    public static class MongoDBExtensions
    {
        public class CollectionIdentity
        {
            public string Id { get; set; }
            public long Value { get; set; }
        }

        private static FindOneAndUpdateOptions<CollectionIdentity> options =
                new FindOneAndUpdateOptions<CollectionIdentity>() { ReturnDocument = ReturnDocument.After, IsUpsert = true };

        public static async ETTask<long> GetIncrementId<T>(this DBComponent self)
        {
            var collection = self.GetCollection<CollectionIdentity>();
            CollectionIdentity id = await collection.FindOneAndUpdateAsync(Builders<CollectionIdentity>.Filter.Eq(f => f.Id, typeof (T).Name),
                Builders<CollectionIdentity>.Update.Inc(f => f.Value, 1), options);
            return id.Value;
        }
    }
}