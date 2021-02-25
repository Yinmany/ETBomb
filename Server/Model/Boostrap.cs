using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using CommandLine;
using NLog;

namespace ET
{
    public class Boostrap
    {
        private List<Assembly> _assemblies = new List<Assembly>();

        public Boostrap AddAssemblyPart(params Assembly[] assemblies)
        {
            _assemblies.AddRange(assemblies);
            return this;
        }

        public void Run(string[] args)
        {
            // 异步方法全部会回掉到主线程
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

            try
            {
                foreach (Assembly assembly in this._assemblies)
                {
                    Game.EventSystem.Add(assembly);
                }

                MongoHelper.Init();

                // 命令行参数
                Options options = null;
                Parser.Default.ParseArguments<Options>(args)
                        .WithNotParsed(error => throw new Exception($"命令行格式错误!"))
                        .WithParsed(o => { options = o; });

                Game.Scene.AddComponent(options);

                IdGenerater.Process = options.Process;

                LogManager.Configuration.Variables["appIdFormat"] = $"{Game.Scene.Id:0000}";

                Log.Info($"server start........................ {Game.Scene.Id}");

                Game.EventSystem.Publish(new EventType.AppStart()).Coroutine();

                while (true)
                {
                    try
                    {
                        Thread.Sleep(1);
                        OneThreadSynchronizationContext.Instance.Update();
                        Game.EventSystem.Update();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}