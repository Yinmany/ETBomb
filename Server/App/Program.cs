namespace ET
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            new Boostrap()
                    .AddAssemblyPart(typeof (OuterOpcode).Assembly, DllHelper.GetHotfixAssembly("Game.Hotfix"))
                    .Run(args);
        }
    }
}