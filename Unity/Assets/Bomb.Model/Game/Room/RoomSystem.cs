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
            for (int i = 0; i < self.Players.Length; i++)
            {
                var item = self.Players[i];
                if (item != player && item.GetComponent<TeamComponent>().Team == team)
                {
                    return item;
                }
            }

            return null;
        }

        public static Player Get(this Room self, int seat)
        {
            return self.Players[seat];
        }
    }
}