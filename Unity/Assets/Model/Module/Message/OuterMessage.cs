using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[Message(OuterOpcode.C2R_Ping)]
	[ProtoContract]
	public partial class C2R_Ping: IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.R2C_Ping)]
	[ProtoContract]
	public partial class R2C_Ping: IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.C2R_Login)]
	[ProtoContract]
	public partial class C2R_Login: IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.R2C_Login)]
	[ProtoContract]
	public partial class R2C_Login: IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Address { get; set; }

		[ProtoMember(2)]
		public long Key { get; set; }

		[ProtoMember(3)]
		public long GateId { get; set; }

	}

	[Message(OuterOpcode.C2G_LoginGate)]
	[ProtoContract]
	public partial class C2G_LoginGate: IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long GateId { get; set; }

	}

	[Message(OuterOpcode.G2C_LoginGate)]
	[ProtoContract]
	public partial class G2C_LoginGate: IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long UId { get; set; }

		[ProtoMember(2)]
		public string NickName { get; set; }

		[ProtoMember(3)]
		public int Coin { get; set; }

		[ProtoMember(4)]
		public int RoomCard { get; set; }

	}

	[Message(OuterOpcode.PlayerInfo)]
	[ProtoContract]
	public partial class PlayerInfo
	{
		[ProtoMember(1)]
		public long UId { get; set; }

		[ProtoMember(2)]
		public string NickName { get; set; }

		[ProtoMember(3)]
		public int Coin { get; set; }

		[ProtoMember(4)]
		public int RoomCard { get; set; }

	}

	[Message(OuterOpcode.PlayerInfoRefresh)]
	[ProtoContract]
	public partial class PlayerInfoRefresh: IActorMessage
	{
		[ProtoMember(94)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public PlayerInfo Info { get; set; }

	}

	[Message(OuterOpcode.C2G_CreateRoomRequest)]
	[ProtoContract]
	public partial class C2G_CreateRoomRequest: IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_CreateRoomResponse)]
	[ProtoContract]
	public partial class G2C_CreateRoomResponse: IResponse
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

	[Message(OuterOpcode.C2G_EnterRoomRequest)]
	[ProtoContract]
	public partial class C2G_EnterRoomRequest: IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoomNum { get; set; }

	}

	[Message(OuterOpcode.G2C_EnterRoomResponse)]
	[ProtoContract]
	public partial class G2C_EnterRoomResponse: IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long RoomMaster { get; set; }

		[ProtoMember(2)]
		public int SeatIndex { get; set; }

	}

	[Message(OuterOpcode.C2G_ExitRoomRequest)]
	[ProtoContract]
	public partial class C2G_ExitRoomRequest: IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public bool IsDestroyRoom { get; set; }

	}

	[Message(OuterOpcode.G2C_ExitRoomResponse)]
	[ProtoContract]
	public partial class G2C_ExitRoomResponse: IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.C2M_RoomOpRequest)]
	[ProtoContract]
	public partial class C2M_RoomOpRequest: IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int Op { get; set; }

		[ProtoMember(2)]
		public List<CardProto> Cards = new List<CardProto>();

	}

	[Message(OuterOpcode.M2C_RoomOpResponse)]
	[ProtoContract]
	public partial class M2C_RoomOpResponse: IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.C2M_PlayCardRequest)]
	[ProtoContract]
	public partial class C2M_PlayCardRequest: IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public List<CardProto> Cards = new List<CardProto>();

	}

	[Message(OuterOpcode.M2C_PlayCardResponse)]
	[ProtoContract]
	public partial class M2C_PlayCardResponse: IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.RoomOpMessage)]
	[ProtoContract]
	public partial class RoomOpMessage: IActorMessage
	{
		[ProtoMember(94)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int Op { get; set; }

		[ProtoMember(2)]
		public int SeatIndex { get; set; }

		[ProtoMember(3)]
		public long UId { get; set; }

	}

	[Message(OuterOpcode.PlayerEnterRoom)]
	[ProtoContract]
	public partial class PlayerEnterRoom: IActorMessage
	{
		[ProtoMember(94)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public PlayerInfo Info { get; set; }

		[ProtoMember(2)]
		public int SeatIndex { get; set; }

		[ProtoMember(3)]
		public bool Ready { get; set; }

	}

	[Message(OuterOpcode.PlayerExitRoom)]
	[ProtoContract]
	public partial class PlayerExitRoom: IActorMessage
	{
		[ProtoMember(94)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UId { get; set; }

		[ProtoMember(2)]
		public int SeatIndex { get; set; }

	}

	[Message(OuterOpcode.CardProto)]
	[ProtoContract]
	public partial class CardProto
	{
		[ProtoMember(1)]
		public int Color { get; set; }

		[ProtoMember(2)]
		public int Weight { get; set; }

	}

	[Message(OuterOpcode.GameStartMessage)]
	[ProtoContract]
	public partial class GameStartMessage: IActorMessage
	{
		[ProtoMember(94)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.HandCardMessage)]
	[ProtoContract]
	public partial class HandCardMessage: IActorMessage
	{
		[ProtoMember(94)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public List<CardProto> Cards = new List<CardProto>();

		[ProtoMember(2)]
		public int Seat { get; set; }

	}

	[Message(OuterOpcode.TeamMessage)]
	[ProtoContract]
	public partial class TeamMessage: IActorMessage
	{
		[ProtoMember(94)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int Team { get; set; }

		[ProtoMember(2)]
		public int SeatIndex { get; set; }

	}

	[Message(OuterOpcode.TurnMessage)]
	[ProtoContract]
	public partial class TurnMessage: IActorMessage
	{
		[ProtoMember(94)]
		public long ActorId { get; set; }

// 最后操作的人
// 最后操作的人
		[ProtoMember(1)]
		public int LastOpSeat { get; set; }

		[ProtoMember(2)]
		public int LastOp { get; set; }

		[ProtoMember(3)]
		public int DeskSeat { get; set; }

		[ProtoMember(4)]
		public List<CardProto> DeskCards = new List<CardProto>();

		[ProtoMember(5)]
		public int DeskCardType { get; set; }

		[ProtoMember(6)]
		public int CurrentSeat { get; set; }

	}

// 回合结束
	[Message(OuterOpcode.RoundEndMessage)]
	[ProtoContract]
	public partial class RoundEndMessage: IActorMessage
	{
		[ProtoMember(94)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.ScoreMessage)]
	[ProtoContract]
	public partial class ScoreMessage: IActorMessage
	{
		[ProtoMember(94)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int Score { get; set; }

		[ProtoMember(2)]
		public int Seat { get; set; }

	}

}
