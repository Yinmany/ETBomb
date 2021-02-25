using System;
using UnityEngine;

namespace AkaUI
{
    public abstract class UIPanel: IDisposable
    {
        public UIBase Parent { get; internal set; }

        protected GameObject View { get; private set; }

        internal void Bind(GameObject view)
        {
            View = view;
            View = view;
            OnInit();
            OnCreate();
        }

        /// <summary>
        /// 请不要重写此方法，给自动获取View引用的生成代码使用。
        /// </summary>
        protected virtual void OnInit()
        {
        }

        protected virtual void OnCreate()
        {
        }

        public virtual void Show()
        {
            this.View.SetActive(true);
        }

        public virtual void Hidden()
        {
            this.View.SetActive(false);
        }

        public virtual void Dispose()
        {
        }
    }
}