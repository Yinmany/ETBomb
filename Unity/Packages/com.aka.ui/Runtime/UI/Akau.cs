using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace AkaUI
{
    /// <summary>
    /// UI管理器
    /// </summary>
    public static class Akau
    {
        private static readonly List<string> PreloadNames = new List<string>();

        private static readonly Dictionary<string, UIProvider> Providers = new Dictionary<string, UIProvider>();

        private static readonly Dictionary<string, UIBase> UIMaps = new Dictionary<string, UIBase>();

        private static readonly Stack<PageTrack> UIPages = new Stack<PageTrack>();

        private static readonly Stack<UIBase> UIMaskStack = new Stack<UIBase>();

        private static readonly GameObject UIRoot;

        private static PageTrack _curPage;

        private static readonly GameObject Mask;

        static Akau()
        {
            UIRoot = GameObject.Find("AkaUI");
            Mask = UIRoot.Get("Mask");
            SettingMask();
            Object.DontDestroyOnLoad(UIRoot);
        }

        private static void SettingMask()
        {
            // 点击遮罩关闭窗口
            EventTrigger trigger = Mask.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerDown};
            entry.callback.AddListener(data =>
            {
                if (UIMaskStack.Count == 0 || !UIMaskStack.Peek().UIConfig.IsMaskClose)
                    return;
                Close(UIMaskStack.Peek().Name);
            });
            trigger.triggers.Add(entry);
        }

        public static Task Preload()
        {
            Task[] tasks = new Task[PreloadNames.Count];
            for (int i = 0; i < PreloadNames.Count; i++)
            {
                tasks[i] = Load(PreloadNames[i]).Task;
            }

            return Task.WhenAll(tasks);
        }

        public static UILoader Load(string uiName)
        {
            if (UIMaps.ContainsKey(uiName))
            {
                throw new Exception($"UI重复加载警告: uiName={uiName}");
            }

            if (!Providers.TryGetValue(uiName, out UIProvider provider))
            {
                throw new Exception($"UI:{uiName} 没有UIProvider!");
            }

            UILoader loader = new UILoader {UI = provider.CreateInstance(), Task = provider.LoadAssets()};

            loader.Task.GetAwaiter().OnCompleted(() =>
            {
                Debug.Log($"加载:" + uiName + " 成功.");
                if (loader.Task.Result == null)
                {
                    return;
                }

                try
                {
                    Attach(loader.UI, loader.Task.Result);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            });

            return loader;
        }

        public static void Attach(GameObject gameObject, bool autoHide = true)
        {
            if (!Providers.TryGetValue(gameObject.name, out UIProvider provider))
            {
                Debug.LogError($"UI:{gameObject.name} 没有UIProvider!");
                return;
            }

            Attach(provider.CreateInstance(), gameObject);

            if (!autoHide)
            {
                Open(gameObject.name);
            }

            Debug.Log($"挂载: uiName={gameObject.name}, autoHide={autoHide}");
        }

        /// <summary>
        /// 挂载一个UI
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="uiGameObject"></param>
        private static void Attach(UIBase ui, GameObject uiGameObject)
        {
            UIConfig config = uiGameObject.GetComponent<UIConfig>();

            if (config == null)
            {
                Debug.LogError($"UI:{ui.View.name},没有UIConfig.");
                return;
            }

            // 设置层级
            Transform parent = UIRoot.GetEx<Transform>(config.layer.ToString());
            uiGameObject.layer = LayerMask.NameToLayer("UI");
            uiGameObject.transform.SetParent(parent, false);
            uiGameObject.SetActive(false);
            UIMaps.Add(ui.Name, ui);
            ui.Init(config, uiGameObject);
        }

        private static void CloseAll()
        {
            foreach (var item in UIMaps.Values)
            {
                if (item.IsOpen)
                    Close(item.Name);
            }
        }

        public static bool Has(string uiName)
        {
            return Get(uiName) != null;
        }

        /// <summary>
        /// 关闭UI
        /// </summary>
        /// <param name="uiName"></param>
        /// <param name="isDestroy">是否销毁</param>
        /// <param name="args"></param>
        public static void Close(string uiName, bool isDestroy = false, object args = null)
        {
            if (UIMaps.TryGetValue(uiName, out var ui))
            {
                if (ui.View.activeSelf)
                {
                    ui.Close(args);
                    if (ui.UIConfig.IsMask)
                    {
                        CloseMask(ui);
                    }
                }

                if (isDestroy)
                {
                    UIMaps.Remove(uiName);

                    if (ui.UIConfig.attachMode)
                    {
                        Object.Destroy(ui.View);
                    }
                    else
                    {
                        Addressables.ReleaseInstance(ui.View);
                    }

                    ui.Dispose();
                }
            }
        }

        private static void InternalOpen(string uiName, object args = null)
        {
            UIBase ui = UIMaps[uiName];
            if (ui.IsOpen) return;

            if (ui.UIConfig.IsMask)
            {
                OpenMask(ui);
            }

            ui.Open(args);
            ui.View.transform.SetAsLastSibling();
        }

        /// <summary>
        /// 打开遮罩
        /// </summary>
        /// <param name="base"></param>
        public static void OpenMask(UIBase @base)
        {
            if (!Mask.activeSelf)
            {
                Mask.GetComponent<Image>().color = @base.UIConfig.Color;
                Mask.SetActive(true);
                Mask.transform.SetAsLastSibling();
            }

            UIMaskStack.Push(@base);
        }

        public static void CloseMask(UIBase @base)
        {
            UIMaskStack.Pop();
            if (UIMaskStack.Count > 0)
            {
                Mask.transform.SetAsLastSibling();
                UIMaskStack.Peek().View.transform.SetAsLastSibling();
            }

            if (UIMaskStack.Count == 0)
            {
                Mask.SetActive(false);
                Mask.transform.SetAsFirstSibling();
            }
        }

        #region 页面操作

        public static void OpenPage(string pageName, bool destroyPrevPage = true)
        {
            if (!UIMaps.TryGetValue(pageName, out UIBase ui))
            {
                Debug.LogWarning($"无法打开: uiName={pageName}");
                return;
            }

            OpenPage(pageName, null, destroyPrevPage);
        }

        private static void OpenPage(string pageName, object arg = null, bool destroyPrevPage = false)
        {
            if (_curPage != null)
            {
                UIPages.Push(_curPage);
            }

            if (UIPages.Count != 0)
            {
                PageTrack pageTrack = UIPages.Peek();
                Close(pageTrack.Name, destroyPrevPage);
                if (destroyPrevPage)
                {
                    UIPages.Pop();
                }
            }

            OpenPageWork(pageName, arg);
        }

        private static void OpenPageWork(string pageName, object args = null)
        {
            _curPage = new PageTrack {Name = pageName};

            CloseAll();

            InternalOpen(pageName, args);
        }

        #endregion

        /// <summary>
        /// 获取UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static UIBase Get(string uiName)
        {
            UIMaps.TryGetValue(uiName, out var t);
            return t;
        }

        public static bool TryGetOpenUI(string uiName, out UIBase ui)
        {
            if (UIMaps.TryGetValue(uiName, out ui))
            {
                if (ui.IsOpen)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool TryGetOpenUI<T>(out T ui) where T : UIBase
        {
            ui = null;
            bool b = TryGetOpenUI(typeof(T).Name, out var uiBase);
            if (b)
            {
                ui = (T) uiBase;
            }

            return b;
        }

        public static void AddProvider(UIProvider provider)
        {
            if (!Providers.ContainsKey(provider.TypeName))
            {
                Providers.Add(provider.TypeName, provider);
            }
        }

        public static bool AddProvider(Type type)
        {
            var attr = type.GetCustomAttribute<UIAttribute>(false);
            if (attr == null)
            {
                return false;
            }

            AddProvider(type, attr);
            return true;
        }

        public static void AddProvider(Type type, UIAttribute attr)
        {
            if (attr.Preload)
            {
                PreloadNames.Add(type.Name);
            }

            AddProvider(new DefaultProvider(type));
        }

        public static void Open(string uiName, bool destroyPrevPage = false, object args = null)
        {
            if (!UIMaps.TryGetValue(uiName, out UIBase ui))
            {
                Debug.LogWarning($"已加载UI列表中不存在, 无法打开: uiName={uiName}, args={args}");
                return;
            }

            try
            {
                switch (ui)
                {
                    case UIWindow window:
                        InternalOpen(uiName, args);
                        break;
                    case UIPage page:
                        OpenPage(uiName, args, destroyPrevPage);
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public static void GoBackPage(bool destroyPrevPage = false)
        {
            if (UIPages.Count <= 0) return;
            var track = UIPages.Pop();
            if (destroyPrevPage && _curPage != null)
            {
                Close(_curPage.Name, true);
            }

            OpenPageWork(track.Name, null);
        }
    }
}