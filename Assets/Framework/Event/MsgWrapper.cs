using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YummyGame.Framework
{
    public class IMsgWrapper
    {
        static ulong _uuid;
        public ulong uuid;
        public IMsgWrapper() { this.uuid = ++_uuid; }
    }
    public class MsgWrapper<T> : IMsgWrapper
    {
        public void AddListener(Action<T> callback)
        {
            Event.AddEventListener(uuid, callback);
        }

        public void Dispach(T args)
        {
            Event.Dispatch(uuid, args);
        }

        public void RemoveListener(Action<T> callback)
        {
            Event.RemoveListener(uuid, callback);
        }
    }

    public class MsgWrapper:IMsgWrapper
    {

        public void AddListener(Action callback)
        {
            Event.AddEventListener(uuid, callback);
        }

        public void Dispach()
        {
            Event.Dispatch(uuid);
        }

        public void RemoveListener(Action callback)
        {
            Event.RemoveListener(uuid, callback);
        }
    }

    public class MsgWrapper<T1,T2>:IMsgWrapper
    {

        public void AddListener(Action<T1,T2> callback)
        {
            Event.AddEventListener(uuid, callback);
        }

        public void Dispach(T1 args1,T2 args2)
        {
            Event.Dispatch(uuid,args1,args2);
        }

        public void RemoveListener(Action<T1,T2> callback)
        {
            Event.RemoveListener(uuid, callback);
        }
    }

    public class MsgWrapper<T1, T2, T3>:IMsgWrapper
    {

        public void AddListener(Action<T1, T2, T3> callback)
        {
            Event.AddEventListener(uuid, callback);
        }

        public void Dispach(T1 args1, T2 args2, T3 args3)
        {
            Event.Dispatch(uuid, args1, args2, args3);
        }

        public void RemoveListener(Action<T1, T2, T3> callback)
        {
            Event.RemoveListener(uuid, callback);
        }
    }

    public class MsgWrapper<T1, T2, T3, T4>:IMsgWrapper
    {

        public void AddListener(Action<T1, T2, T3, T4> callback)
        {
            Event.AddEventListener(uuid, callback);
        }

        public void Dispach(T1 args1, T2 args2, T3 args3, T4 args4)
        {
            Event.Dispatch(uuid, args1, args2, args3, args4);
        }

        public void RemoveListener(Action<T1, T2, T3, T4> callback)
        {
            Event.RemoveListener(uuid, callback);
        }
    }
}

