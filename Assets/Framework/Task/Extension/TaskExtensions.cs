using System;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.Framework
{
    public static class TaskExtensions
    {
        public static T Alloc<T>(this T task)where T : class, ITask
        {
            IPool<T> pool = Pools.GetDefaultTaskPool<T>();
            return pool.Get();
        }

        public static ITask AsyncAsTask(this AsyncOperation async)
        {
            var task = new UnityOperationTask(async,Facade.Instance.taskLooper);
            task.Start();
            return task;
        }

        public static ITaskChain AsChain(this ITask task,TaskLooper looper = null)
        {
            var seq = new SequenceTaskChain(looper).Append(task);
            seq.Start();
            return seq;
        }
    }
}
