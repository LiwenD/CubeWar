using System;
using System.Collections.Generic;

namespace YummyGame.Framework
{
    public class Pool<T> : IPool<T> where T:IPoolObject
    {
        protected readonly Stack<T> m_cachedObjects = new Stack<T>();
        protected IFactory<T> m_Factory;
        protected Pool()
        {

        }
        public T Get()
        {
            return m_cachedObjects.Count == 0 ? m_Factory.Get() : m_cachedObjects.Pop();
        }

        public void Recover(T obj)
        {
            obj.OnDisable();
            m_cachedObjects.Push(obj);
        }
    }
}
