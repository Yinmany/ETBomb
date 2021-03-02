namespace Bomb
{
    public static class RobotSystem
    {
        public static void Log(this RobotProxy self, string msg)
        {
            Player player = self.GetParent<Player>();
            ET.Log.Debug($"[{player.Room.Num}|Robot({player.SeatIndex})]: {msg}");
        }

        public static void LogError(this RobotProxy self, string msg)
        {
            Player player = self.GetParent<Player>();
            ET.Log.Error($"[{player.Room.Num}|Robot({player.SeatIndex})]: {msg}");
        }
    }
}