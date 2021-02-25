namespace Bomb
{
    public static class RoomOpType
    {
        public const int Ready = 1;
        public const int Destroy = 2;
        public const int CancelReady = 4;

        public const int MockPlayer_Add = 5;
        public const int MockPlayer_Remove = 6;
        public const int MockPlayer_SwitchSeat = 7;
        public const int MockPlayer_Ready = 8;
        public const int MockPlayer_CancelReady = 9;

        // 不出
        public const int NotPlay = 10;
    }

    public static class GameErrorCode
    {
        // 房间没有位置了
        public const int ERR_RoomNotSeat = 200001;
        public const int ERR_NotFoundRoom = 200002;
    }
}