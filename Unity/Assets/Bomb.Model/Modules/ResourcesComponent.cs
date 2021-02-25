using System.Collections.Generic;
using AkaUI;
using ET;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Bomb
{
    public struct ResUpdateEvent
    {
        public float Percent;
        public AsyncOperationStatus Status;
        public bool IsPreload;
        public string PreloadName;
    }

    /// <summary>
    /// 预加载完成
    /// </summary>
    public struct PreloadCompleted
    {
    }

    public class ResourcesComponent: Entity
    {
        public AsyncOperationHandle DownloadHandle;
        public long TimerId;

        public async ETTask Load()
        {
            // 预先加载的资源
            var resourceLocations = Addressables.LoadResourceLocationsAsync("preload");
            IList<IResourceLocation> locations = await resourceLocations.Task;

            // 预先下载的资源大小
            long downloadSize = await Addressables.GetDownloadSizeAsync("preload").Task;
            Log.Info($"预先下载资源大小:{downloadSize}");

            this.DownloadHandle = Addressables.DownloadDependenciesAsync(locations, false);

            this.TimerId = Game.Scene.GetComponent<TimerComponent>()
                    .NewRepeatedTimer(1000, b =>
                    {
                        var e = new ResUpdateEvent
                        {
                            Percent = this.DownloadHandle.GetDownloadStatus().Percent, Status = this.DownloadHandle.Status,
                        };

                        EventBus.Publish(e);
                    });

            await this.DownloadHandle.Task;
            TimerComponent.Instance.Remove(this.TimerId);

            // 加载完成事件 
            EventBus.Publish(new ResUpdateEvent { Percent = this.DownloadHandle.GetDownloadStatus().Percent, Status = this.DownloadHandle.Status });

            // 资源下载完成，开始加载预加载资源。
            if (this.DownloadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                await Preload();
            }

            // 清理
            Addressables.Release(this.DownloadHandle);
        }

        private async ETTask Preload()
        {
            // 开始预加载资源
            EventBus.Publish(new ResUpdateEvent { IsPreload = true, PreloadName = "UI" });
            await Akau.Preload();

            EventSystem.Instance.Publish(new PreloadCompleted());
        }
    }
}