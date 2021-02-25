using System;

namespace AkaUI
{
    public interface IEventReceiver<in T>
    {
        void OnEvent(T e);
    }

    public interface IEventReceiver
    {
        Type GetEventType();
    }

    [EventReceiver]
    public abstract class AEventReceiver<T>: IEventReceiver where T : struct
    {
        public Type GetEventType()
        {
            return typeof (T);
        }

        public abstract void Run(T a);
    }
}