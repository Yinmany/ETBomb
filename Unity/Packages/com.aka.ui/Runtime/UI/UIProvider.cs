using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AkaUI
{
    /// <summary>
    /// UI实例提供者
    /// </summary>
    public abstract class UIProvider
    {
        /// <summary>
        /// UI名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 实例
        /// </summary>
        public abstract UIBase CreateInstance();

        /// <summary>
        /// 加载UI资源
        /// </summary>
        /// <returns></returns>
        public virtual Task<GameObject> LoadAssets()
        {
            TaskCompletionSource<GameObject> tcs = new TaskCompletionSource<GameObject>();
            var ao = Addressables.InstantiateAsync($"UI/{TypeName}.prefab");
            ao.Completed += a =>
            {
                if (a.Status == AsyncOperationStatus.Succeeded)
                {
                    tcs.SetResult(a.Result);
                }
                else
                {
                    tcs.SetException(a.OperationException);
                }
            };
            return tcs.Task;
        }
    }

    public class DefaultProvider : UIProvider
    {
        private Type _type;

        public DefaultProvider(Type type)
        {
            _type = type;
            TypeName = type.Name;
        }

        public override UIBase CreateInstance()
        {
            var ins = Activator.CreateInstance(_type) as UIBase;
            if (ins != null)
            {
                ins.Name = TypeName;
            }

            return ins;
        }
    }
}