using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YummyGame.Framework
{
    public static class Event
    {
        public interface IBaseEvent
        {
            void Dispatch(object t);
            Delegate EventAction { get; set; }
        }

        public class EventNormal : IBaseEvent
        {
            public Action action;

            public Delegate EventAction { get => action; set => action = (Action)value; }

            public void Dispatch(object t)
            {
                action();
            }
        }

        public class EventGeneric<T> : IBaseEvent
        {
            public Action<T> action;

            public Delegate EventAction { get => action; set => action = (Action<T>)value; }

            public void Dispatch(object t)
            {
                action((T)t);
            }
        }

        public class EventGeneric<T1, T2> : IBaseEvent
        {
            public Action<T1, T2> action;

            public Delegate EventAction { get => action; set => action = (Action<T1, T2>)value; }

            public void Dispatch(object t)
            {
                Tuple<T1, T2> tuple = t as Tuple<T1, T2>;
                action(tuple.Item1, tuple.Item2);
            }
        }

        public class EventGeneric<T1, T2, T3> : IBaseEvent
        {
            public Action<T1, T2, T3> action;

            public Delegate EventAction { get => action; set => action = (Action<T1, T2, T3>)value; }

            public void Dispatch(object t)
            {
                Tuple<T1, T2, T3> tuple = t as Tuple<T1, T2, T3>;
                action(tuple.Item1, tuple.Item2, tuple.Item3);
            }
        }

        public class EventGeneric<T1, T2, T3, T4> : IBaseEvent
        {
            public Action<T1, T2, T3, T4> action;

            public Delegate EventAction { get => action; set => action = (Action<T1, T2, T3, T4>)value; }

            public void Dispatch(object t)
            {
                Tuple<T1, T2, T3, T4> tuple = t as Tuple<T1, T2, T3, T4>;
                action(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
            }
        }

        public static Dictionary<ulong, List<IBaseEvent>> dic = new Dictionary<ulong, List<IBaseEvent>>();
        public static void AddEventListener<T>(ulong uuid, Action<T> callback)
        {
            AddEventListenerInternal<EventGeneric<T>>(uuid, callback);
        }

        public static void Dispatch<T>(ulong uuid, T args)
        {
            DispatchInternal(uuid, args);
        }

        public static void RemoveListener<T>(ulong uuid, Action<T> callback)
        {
            RemoveListenerInternal<EventGeneric<T>>(uuid, callback);
        }

        public static void AddEventListener(ulong uuid, Action callback)
        {
            AddEventListenerInternal<EventNormal>(uuid, callback);
        }

        public static void Dispatch(ulong uuid)
        {
            DispatchInternal(uuid, NullObject.Default);
        }

        public static void RemoveListener(ulong uuid, Action callback)
        {
            RemoveListenerInternal<EventNormal>(uuid, callback);
        }

        public static void AddEventListener<T1,T2>(ulong uuid, Action<T1,T2> callback)
        {
            AddEventListenerInternal<EventGeneric<T1, T2>>(uuid, callback);
        }

        public static void Dispatch<T1,T2>(ulong uuid, T1 args1,T2 args2)
        {
            DispatchInternal(uuid, new Tuple<T1, T2>(args1, args2));
        }

        public static void RemoveListener<T1,T2>(ulong uuid, Action<T1,T2> callback)
        {
            RemoveListenerInternal<EventGeneric<T1, T2>>(uuid, callback);
        }

        public static void AddEventListener<T1, T2, T3>(ulong uuid, Action<T1, T2, T3> callback)
        {
            AddEventListenerInternal<EventGeneric<T1, T2, T3>>(uuid, callback);
        }

        public static void Dispatch<T1, T2, T3>(ulong uuid, T1 args1, T2 args2, T3 args3)
        {
            DispatchInternal(uuid, new Tuple<T1, T2, T3>(args1, args2, args3));
        }

        public static void RemoveListener<T1, T2, T3>(ulong uuid, Action<T1, T2, T3> callback)
        {
            RemoveListenerInternal<EventGeneric<T1, T2, T3>>(uuid, callback);
        }

        public static void AddEventListener<T1, T2, T3, T4>(ulong uuid, Action<T1, T2, T3, T4> callback)
        {
            AddEventListenerInternal<EventGeneric<T1, T2, T3, T4>>(uuid, callback);
        }

        public static void Dispatch<T1, T2, T3, T4>(ulong uuid, T1 args1, T2 args2, T3 args3, T4 args4)
        {
            DispatchInternal(uuid, new Tuple<T1, T2, T3, T4>(args1, args2, args3, args4));
        }

        public static void RemoveListener<T1, T2, T3, T4>(ulong uuid, Action<T1, T2, T3, T4> callback)
        {
            RemoveListenerInternal<EventGeneric<T1, T2, T3, T4>>(uuid, callback);
        }

        private static void AddEventListenerInternal<T>(ulong uuid, Delegate callback)
            where T:IBaseEvent
        {
            T temp;
            if (dic.Count == 0 || !dic.ContainsKey(uuid))
            {
                temp = Activator.CreateInstance<T>();
                temp.EventAction = callback;
                dic.Add(uuid, new List<IBaseEvent>());
                dic[uuid].Add(temp);
                return;
            }
            temp = Activator.CreateInstance<T>();
            temp.EventAction = callback;
            dic[uuid].Add(temp);
        }

        private static void DispatchInternal<T>(ulong uuid,T value)
        {
            if (!dic.ContainsKey(uuid)) return;
            foreach (var item in dic[uuid])
            {
                item.Dispatch(value);
            }
        }

        public static void RemoveListenerInternal<T>(ulong uuid, Delegate callback)where T:IBaseEvent
        {
            if (!dic.ContainsKey(uuid)) return;
            T temp = default(T);
            foreach (var item in dic[uuid])
            {
                temp = (T)item;
                if (temp.EventAction == callback)
                {
                    break;
                }
            }
            if (temp != null)
            {
                dic[uuid].Remove(temp);
                if (dic[uuid].Count == 0)
                {
                    dic.Remove(uuid);
                }
            }
        }
    }
}

