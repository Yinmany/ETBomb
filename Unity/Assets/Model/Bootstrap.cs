using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using  UnityEditorInternal;
#endif
namespace ET
{
    /// <summary>
    /// ET启动类
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class Bootstrap: MonoBehaviour
    {
        public List<string> assemblyNames = new List<string>();

        private void Awake()
        {
            try
            {
                SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
                DontDestroyOnLoad(gameObject);

                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    string assemblyName = assembly.ManifestModule.Name;
                    if (!assemblyNames.Contains(assemblyName) || assemblyName == "Unity.Model.dll")
                    {
                        continue;
                    }

                    Game.EventSystem.Add(assembly);
                }

                Game.EventSystem.Publish(new EventType.AppStart()).Coroutine();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private void Update()
        {
            OneThreadSynchronizationContext.Instance.Update();
            Game.EventSystem.Update();
        }

        private void LateUpdate()
        {
            Game.EventSystem.LateUpdate();
        }

        private void OnApplicationQuit()
        {
            Game.Close();
        }
    }
}