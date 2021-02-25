using System;
using System.Collections.Generic;
using UnityEngine;

namespace AkaUI
{
    /// <summary>
    /// 事件中心
    /// </summary>
    public class EventBus
    {
        private static Dictionary<Type, List<EventDesc>> _allEvents = new Dictionary<Type, List<EventDesc>>();

        /// <summary>
        /// 发布事件
        /// </summary>
        public static void Publish<T>(T a) where T : struct
        {
            if (!_allEvents.TryGetValue(typeof (T), out var events))
            {
                return;
            }

            foreach (EventDesc obj in events)
            {
                try
                {
                    obj.Invoke(a);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        public static SubscribeHandle Subscribe<T>(Action<T> listener) where T : struct
        {
            if (!_allEvents.TryGetValue(typeof (T), out var list))
            {
                list = new List<EventDesc>();
                _allEvents.Add(typeof (T), list);
            }

            list.Add(new EventDesc(listener, true));

            return new SubscribeHandle(() => Unsubscribe(listener));
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        public static void Subscribe(Type listener)
        {
            IEventReceiver e = Activator.CreateInstance(listener) as IEventReceiver;
            if (e == null)
            {
                return;
            }

            if (!_allEvents.TryGetValue(e.GetEventType(), out var list))
            {
                list = new List<EventDesc>();
                _allEvents.Add(e.GetEventType(), list);
            }

            list.Add(new EventDesc(e, false));
        }

        public static void Unsubscribe<T>(Action<T> listener) where T : struct
        {
            if (!_allEvents.TryGetValue(typeof (T), out var list))
            {
                return;
            }

            int removeIndex = -1;
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (!item.IsAction)
                {
                    continue;
                }

                if (!(item.Obj is Action<T> tmp)) continue;

                if (tmp != listener) continue;

                // 每次只移除匹配的一个
                removeIndex = i;
                break;
            }

            list.RemoveAt(removeIndex);
        }
    }
}