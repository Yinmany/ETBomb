using ET;

namespace Bomb
{
    public class AccountModel: Entity, ISerializeToEntity
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public long UId { get; set; }
    }
}