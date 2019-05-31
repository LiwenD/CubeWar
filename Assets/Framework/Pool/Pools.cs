using System;
using System.Collections.Generic;

namespace YummyGame.Framework
{
    public static class Pools
    {
        public static readonly IPool<TaskList> SeqTaskPool;
        public static readonly IPool<TaskTweener> TweenerTaskPool;
        static Pools()
        {
            SeqTaskPool = SingletonPool<TaskList>.Instance.Create(() => new TaskList());
            TweenerTaskPool = SingletonPool<TaskTweener>.Instance.Create(() => new TaskTweener());
        }

        public static IPool<T> GetDefaultTaskPool<T>() where T :class, ITask
        {
            return SingletonPool<T>.Instance.Create(() => Activator.CreateInstance<T>());
        }
    }
}
