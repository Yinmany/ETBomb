using System;

namespace AkaUI
{
    /// <summary>
    /// 事件描述
    /// </summary>
    public class EventDesc
    {
        public bool IsAction { get; }

        public object Obj { get; }

        public EventDesc(object obj, bool isAction)
        {
            Obj = obj;
            IsAction = isAction;
        }

        public void Invoke<T>(T a) where T : struct
        {
            if (IsAction)
            {
                if (Obj is Action<T> action)
                {
                    action(a);
                }
            }
            else
            {
                if (Obj is AEventReceiver<T> action)
                {
                    action.Run(a);
                }
            }
        }
    }
}