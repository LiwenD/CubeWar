using System;
using System.Collections.Generic;

namespace YummyGame.Framework
{
    public interface IPool<T>
    {
        T Get();
        void Recover(T obj);
    }

    public interface IPoolObject
    {
        void OnEnable();
        void OnDisable();
    }
}
