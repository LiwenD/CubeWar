using System;
using System.Collections.Generic;


namespace YummyGame.Framework
{
    public class SingletonPool<T>:Singleton<SingletonPool<T>>, IPool<T> where T: class,IPoolObject
    {
        protected readonly Stack<T> m_cachedObjects = new Stack<T>();
        protected IFactory<T> m_Factory;
        public SingletonPool() { }
        public IPool<T> Create(Func<T> allocFunc)
        {
            if(m_Factory==null)
                m_Factory = new Factorys<T>(allocFunc);
            return Instance;
        }
        public T Get()
        {
            T res = m_cachedObjects.Count == 0 ? m_Factory.Get() : m_cachedObjects.Pop();
            res.OnEnable();
            return res;
        }

        public void Recover(T obj)
        {
            obj.OnDisable();
            m_cachedObjects.Push(obj);
        }
    }
}
