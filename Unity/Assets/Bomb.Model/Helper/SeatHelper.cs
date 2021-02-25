namespace Bomb
{
    public static class SeatHelper
    {
        /// <summary>
        /// 以自己为主映射座位(UI显示使用)
        /// </summary>
        /// <param name="selfSeat"></param>
        /// <param name="seat"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int MappingToView(int selfSeat, int seat, int count)
        {
            return (seat - selfSeat + count) % count;
        }

        public static int InverseMapping(int selfSeat, int seat, int count)
        {
            return (seat + selfSeat + count) % count;
        }
    }
}