using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[Message(InnerOpcode.R2G_GetLoginKey)]
	[ProtoContract]
	public partial class R2G_GetLoginKey: IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UId { get; set; }

	}

	[Message(InnerOpcode.G2R_GetLoginKey)]
	[ProtoContract]
	public partial class G2R_GetLoginKey: IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long GateId { get; set; }

	}

	[Message(InnerOpcode.M2G_SessionConnect)]
	[ProtoContract]
	public partial class M2G_SessionConnect: IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long PlayerActorId { get; set; }

	}

	[Message(InnerOpcode.G2M_SessionDisconnect)]
	[ProtoContract]
	public partial class G2M_SessionDisconnect: IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(94)]
		public long ActorId { get; set; }

	}

	[Message(InnerOpcode.M2L_GetRoomNumRequest)]
	[ProtoContract]
	public partial class M2L_GetRoomNumRequest: IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(InnerOpcode.L2M_GetRoomNumResponse)]
	[ProtoContract]
	public partial class L2M_GetRoomNumResponse: IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long RoomNum { get; set; }

	}

	[Message(InnerOpcode.M2L_RoomAddMessage)]
	[ProtoContract]
	public partial class M2L_RoomAddMessage: IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long RoomNum { get; set; }

		[ProtoMember(2)]
		public long RoomId { get; set; }

	}

	[Message(InnerOpcode.M2L_RoomRemoveMessage)]
	[ProtoContract]
	public partial class M2L_RoomRemoveMessage: IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long RoomNum { get; set; }

	}

	[Message(InnerOpcode.A2L_RoomGetRequest)]
	[ProtoContract]
	public partial class A2L_RoomGetRequest: IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long RoomNum { get; set; }

	}

	[Message(InnerOpcode.L2A_RoomGetResponse)]
	[ProtoContract]
	public partial class L2A_RoomGetResponse: IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public long RoomId { get; set; }

	}

	[Message(InnerOpcode.G2M_CreateRoomRequest)]
	[ProtoContract]
	public partial class G2M_CreateRoomRequest: IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UId { get; set; }

	}

	[Message(InnerOpcode.M2G_CreateRoomResponse)]
	[ProtoContract]
	public partial class M2G_CreateRoomResponse: IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public long RoomNum { get; set; }

	}

	[Message(InnerOpcode.G2M_EnterRoomRequest)]
	[ProtoContract]
	public partial class G2M_EnterRoomRequest: IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UId { get; set; }

		[ProtoMember(2)]
		public long GateSessionId { get; set; }

	}

	[Message(InnerOpcode.M2G_EnterRoomResponse)]
	[ProtoContract]
	public partial class M2G_EnterRoomResponse: IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long PlayerActorId { get; set; }

		[ProtoMember(2)]
		public int SeatIndex { get; set; }

		[ProtoMember(4)]
		public long RoomMaster { get; set; }

	}

	[Message(InnerOpcode.G2M_ExitRoomRequest)]
	[ProtoContract]
	public partial class G2M_ExitRoomRequest: IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public bool IsDestroyRoom { get; set; }

	}

	[Message(InnerOpcode.M2G_ExitRoomResponse)]
	[ProtoContract]
	public partial class M2G_ExitRoomResponse: IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

}
