namespace ET
{
	public static partial class InnerOpcode
	{
		 public const ushort R2G_GetLoginKey = 10001;
		 public const ushort G2R_GetLoginKey = 10002;
		 public const ushort M2G_SessionConnect = 10003;
		 public const ushort G2M_SessionDisconnect = 10004;
		 public const ushort M2L_GetRoomNumRequest = 10005;
		 public const ushort L2M_GetRoomNumResponse = 10006;
		 public const ushort M2L_RoomAddMessage = 10007;
		 public const ushort M2L_RoomRemoveMessage = 10008;
		 public const ushort A2L_RoomGetRequest = 10009;
		 public const ushort L2A_RoomGetResponse = 10010;
		 public const ushort G2M_CreateRoomRequest = 10011;
		 public const ushort M2G_CreateRoomResponse = 10012;
		 public const ushort G2M_EnterRoomRequest = 10013;
		 public const ushort M2G_EnterRoomResponse = 10014;
		 public const ushort G2M_ExitRoomRequest = 10015;
		 public const ushort M2G_ExitRoomResponse = 10016;
	}
}
