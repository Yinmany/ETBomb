using ProtoBuf;

namespace ET
{
    public static class ActorInnerOpcode
    {
        public const ushort ObjectAddRequest = 1;
        public const ushort ObjectAddResponse = 2;
        public const ushort ObjectLockRequest = 3;
        public const ushort ObjectLockResponse = 4;
        public const ushort ObjectUnLockRequest = 5;
        public const ushort ObjectUnLockResponse = 6;
        public const ushort ObjectRemoveRequest = 7;
        public const ushort ObjectRemoveResponse = 8;
        public const ushort ObjectGetRequest = 9;
        public const ushort ObjectGetResponse = 10;
        public const ushort ActorResponse = 11;
    }

    [Message(ActorInnerOpcode.ObjectAddRequest)]
    [ProtoContract]
    public partial class ObjectAddRequest: IActorRequest
    {
        [ProtoMember(90)]
        public int RpcId { get; set; }

        [ProtoMember(93)]
        public long ActorId { get; set; }

        [ProtoMember(1)]
        public long Key { get; set; }

        [ProtoMember(2)]
        public long InstanceId { get; set; }
    }

    [Message(ActorInnerOpcode.ObjectAddResponse)]
    [ProtoContract]
    public partial class ObjectAddResponse: IActorResponse
    {
        [ProtoMember(90)]
        public int RpcId { get; set; }

        [ProtoMember(91)]
        public int Error { get; set; }

        [ProtoMember(92)]
        public string Message { get; set; }
    }

    [Message(ActorInnerOpcode.ObjectLockRequest)]
    [ProtoContract]
    public partial class ObjectLockRequest: IActorRequest
    {
        [ProtoMember(90)]
        public int RpcId { get; set; }

        [ProtoMember(93)]
        public long ActorId { get; set; }

        [ProtoMember(1)]
        public long Key { get; set; }

        [ProtoMember(2)]
        public long InstanceId { get; set; }

        [ProtoMember(3)]
        public int Time { get; set; }
    }

    [Message(ActorInnerOpcode.ObjectLockResponse)]
    [ProtoContract]
    public partial class ObjectLockResponse: IActorResponse
    {
        [ProtoMember(90)]
        public int RpcId { get; set; }

        [ProtoMember(91)]
        public int Error { get; set; }

        [ProtoMember(92)]
        public string Message { get; set; }
    }

    [Message(ActorInnerOpcode.ObjectUnLockRequest)]
    [ProtoContract]
    public partial class ObjectUnLockRequest: IActorRequest
    {
        [ProtoMember(90)]
        public int RpcId { get; set; }

        [ProtoMember(93)]
        public long ActorId { get; set; }

        [ProtoMember(1)]
        public long Key { get; set; }

        [ProtoMember(2)]
        public long OldInstanceId { get; set; }

        [ProtoMember(3)]
        public long InstanceId { get; set; }
    }

    [Message(ActorInnerOpcode.ObjectUnLockResponse)]
    [ProtoContract]
    public partial class ObjectUnLockResponse: IActorResponse
    {
        [ProtoMember(90)]
        public int RpcId { get; set; }

        [ProtoMember(91)]
        public int Error { get; set; }

        [ProtoMember(92)]
        public string Message { get; set; }
    }

    [Message(ActorInnerOpcode.ObjectRemoveRequest)]
    [ProtoContract]
    public partial class ObjectRemoveRequest: IActorRequest
    {
        [ProtoMember(90)]
        public int RpcId { get; set; }

        [ProtoMember(93)]
        public long ActorId { get; set; }

        [ProtoMember(1)]
        public long Key { get; set; }
    }

    [Message(ActorInnerOpcode.ObjectRemoveResponse)]
    [ProtoContract]
    public partial class ObjectRemoveResponse: IActorResponse
    {
        [ProtoMember(90)]
        public int RpcId { get; set; }

        [ProtoMember(91)]
        public int Error { get; set; }

        [ProtoMember(92)]
        public string Message { get; set; }
    }

    [Message(ActorInnerOpcode.ObjectGetRequest)]
    [ProtoContract]
    public partial class ObjectGetRequest: IActorRequest
    {
        [ProtoMember(90)]
        public int RpcId { get; set; }

        [ProtoMember(93)]
        public long ActorId { get; set; }

        [ProtoMember(1)]
        public long Key { get; set; }
    }

    [Message(ActorInnerOpcode.ObjectGetResponse)]
    [ProtoContract]
    public partial class ObjectGetResponse: IActorResponse
    {
        [ProtoMember(90)]
        public int RpcId { get; set; }

        [ProtoMember(91)]
        public int Error { get; set; }

        [ProtoMember(92)]
        public string Message { get; set; }

        [ProtoMember(1)]
        public long InstanceId { get; set; }
    }
}