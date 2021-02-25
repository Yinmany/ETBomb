using System;
using System.Collections.Generic;
using UnityEngine;

namespace AkaUI
{
    /// <summary>
    /// 所有UI的基类
    /// </summary>
    public abstract class UIBase : IDisposable
    {
        public UIConfig UIConfig { get; private set; }

        public GameObject View { get; private set; }

        public string Name { get; set; }

        /// <summary>
        /// 是否是打开
        /// </summary>
        public bool IsOpen => View.activeSelf;

        private List<Action> _autoDispose;

        public void AddToDispose(Action disposable)
        {
            if (_autoDispose == null)
            {
                _autoDispose = new List<Action>();
            }

            _autoDispose.Add(disposable);
        }

        public void Init(UIConfig config, GameObject view)
        {
            UIConfig = config;
            View = view;
            OnInit();
            OnCreate();
        }

        public virtual void Open(object args = null)
        {
            View.SetActive(true);
            OnOpen(args);
        }

        public virtual void Close(object args = null)
        {
            if (!OnClosing(args)) return;
            OnClose(args);
            View.SetActive(false);
        }

        public void Dispose()
        {
            if (this._autoDispose != null)
            {
                foreach (Action disposable in _autoDispose)
                {
                    disposable?.Invoke();
                }

                _autoDispose.Clear();
                _autoDispose = null;
            }

            OnDestroy();
        }

        #region UI事件

        /// <summary>
        /// 用于获取UI引用
        /// </summary>
        protected abstract void OnInit();

        protected abstract void OnCreate();
        protected abstract void OnDestroy();
        protected abstract void OnOpen(object args = null);
        protected abstract void OnClose(object args = null);
        protected abstract bool OnClosing(object args = null);

        #endregion
    }
}