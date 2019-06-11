using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YummyGame.Framework
{
    class MsgWrapper<T> 
    {
        public MsgWrapper() { this.uuid = ++Event._uuid; }

        public ulong uuid { get;private set; }

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

    class MsgWrapper
    {
        public MsgWrapper() { this.uuid = ++Event._uuid; }

        public ulong uuid { get; private set; }

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

    class MsgWrapper<T1,T2>
    {
        public MsgWrapper() { this.uuid = ++Event._uuid; }

        public ulong uuid { get; private set; }

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

    class MsgWrapper<T1, T2, T3>
    {
        public MsgWrapper() { this.uuid = ++Event._uuid; }

        public ulong uuid { get; private set; }

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

    class MsgWrapper<T1, T2, T3, T4>
    {
        public MsgWrapper() { this.uuid = ++Event._uuid; }

        public ulong uuid { get; private set; }

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

