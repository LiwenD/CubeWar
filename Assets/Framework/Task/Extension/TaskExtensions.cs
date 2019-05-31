using System;
using System.Collections.Generic;

namespace YummyGame.Framework
{
    public static class TaskExtensions
    {
        public static T Alloc<T>(this T task)where T : class, ITask
        {
            IPool<T> pool = Pools.GetDefaultTaskPool<T>();
            return pool.Get();
        }
    }
}
