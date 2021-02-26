namespace Bomb
{
    public static class RoomSystem
    {
        /// <summary>
        /// 获取同组Player
        /// </summary>
        /// <param name="self"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static Player GetSameTeam(this Room self, Player player)
        {
            var team = player.GetComponent<TeamComponent>().Team;
            for (int i = 0; i < self.Seats.Length; i++)
            {
                var item = self.Seats[i];
                if (item != player && item.GetComponent<TeamComponent>().Team == team)
                {
                    return item;
                }
            }

            return null;
        }
    }
}