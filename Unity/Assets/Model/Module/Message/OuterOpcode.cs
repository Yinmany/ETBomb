namespace ET
{
	public static partial class OuterOpcode
	{
		 public const ushort C2R_Ping = 20001;
		 public const ushort R2C_Ping = 20002;
		 public const ushort C2R_Login = 20003;
		 public const ushort R2C_Login = 20004;
		 public const ushort C2G_LoginGate = 20005;
		 public const ushort G2C_LoginGate = 20006;
		 public const ushort PlayerInfo = 20007;
		 public const ushort PlayerInfoRefresh = 20008;
		 public const ushort C2G_CreateRoomRequest = 20009;
		 public const ushort G2C_CreateRoomResponse = 20010;
		 public const ushort C2G_EnterRoomRequest = 20011;
		 public const ushort G2C_EnterRoomResponse = 20012;
		 public const ushort C2G_ExitRoomRequest = 20013;
		 public const ushort G2C_ExitRoomResponse = 20014;
		 public const ushort C2M_RoomOpRequest = 20015;
		 public const ushort M2C_RoomOpResponse = 20016;
		 public const ushort RoomOpMessage = 20017;
		 public const ushort PlayerEnterRoom = 20018;
		 public const ushort PlayerExitRoom = 20019;
		 public const ushort CardProto = 20020;
		 public const ushort HandCardsMessage = 20021;
		 public const ushort TeamMessage = 20022;
		 public const ushort TurnMessage = 20023;
	}
}
