using AkaUI;
using ET;

namespace Bomb
{
    public class AppStart: AEvent<ET.EventType.AppStart>
    {
        public override async ETTask Run(ET.EventType.AppStart args)
        {
            Log.Info($"AppStart...");

            AkaBuilder.Create()
                    .UseUI()
                    .UseEventBus()
                    .AddAssemblyPart(typeof (AppStart).Assembly)
                    .Build();

            Game.Scene.AddComponent<TimerComponent>();

            // 更新资源 并预加载资源
            var resources = Game.Scene.AddComponent<ResourcesComponent>();
            await resources.Load();

            // 添加必要组件
            Game.Scene.AddComponent<ConfigComponent>();
            Game.Scene.AddComponent<OpcodeTypeComponent>();
            Game.Scene.AddComponent<MessageDispatcherComponent>();
            Game.Scene.AddComponent<NetOuterComponent>();
            
            // 进入登录界面
            Akau.Open(nameof (LoginPage), true);
        }
    }
}