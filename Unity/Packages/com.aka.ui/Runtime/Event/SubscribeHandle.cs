using System;

namespace AkaUI
{
    public struct SubscribeHandle : IDisposable
    {
        private Action a;

        public SubscribeHandle(Action action)
        {
            a = action;
        }

        public void Dispose()
        {
            a?.Invoke();
        }

        public static SubscribeHandle operator +(SubscribeHandle a, SubscribeHandle b)
        {
            a.a += b.a;
            return a;
        }
    }
}