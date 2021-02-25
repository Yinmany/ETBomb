using System;
using UnityEngine;

namespace AkaUI
{
    public static class UIExtensions
    {
        public static T AddPanel<T>(this UIBase self) where T : UIPanel, new()
        {
            T t = new T();
            self.AddPanel(t);
            return t;
        }

        public static void AddPanel(this UIBase self, UIPanel panel)
        {
            self.AddPanel(panel, panel.GetType().Name);
        }

        public static void AddPanel(this UIBase self, UIPanel panel, string bindViewName)
        {
            panel.Parent = self;
            panel.Bind(self.View.Get<GameObject>(bindViewName));
            self.AddToDispose(panel.Dispose);
        }

        /// <summary>
        /// 在UI中订阅事件中心的事件
        /// 当前UI被销毁，自动取消订阅。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        public static void Subscribe<T>(this UIBase self, Action<T> action) where T : struct
        {
            self.AddToDispose(EventBus.Subscribe(action).Dispose);
        }

        public static void Subscribe<T>(this UIBase self, Action<T, string> action) where T : struct
        {
        }
    }
}