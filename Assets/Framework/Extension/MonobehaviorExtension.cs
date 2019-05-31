using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace YummyGame.Framework
{
    public static class MonobehaviorExtension
    {
        public static ITaskChain Task(this MonoBehaviour mono)
        {
            SequenceTaskChain task = new SequenceTaskChain(mono.gameObject.GetOrAddComponent<TaskLooper>());
            return task;
        }

        public static ITaskChain ParallelTask(this MonoBehaviour mono)
        {
            ParallelTaskChain task = new ParallelTaskChain(mono.gameObject.GetOrAddComponent<TaskLooper>());
            return task;
        }

        public static ITaskChain Until(this ITaskChain task,Func<bool> predicate)
        {
            task.Append(new UntilTask(predicate, task.Looper));
            return task;
        }

        public static ITaskChain Event(this ITaskChain task, Action func)
        {
            task.Append(new EventTask(func, task.Looper));
            return task;
        }

        public static ITaskChain RepeatEvent(this ITaskChain task, Action func,int repeat)
        {
            task.Append(new RepeatEventTask(func,repeat, task.Looper));
            return task;
        }

        public static ITaskChain Delay(this ITaskChain task, float delay)
        {
            task.Append(new DelayTask(delay, task.Looper));
            return task;
        }

        public static ITaskChain Wait(this ITaskChain task, ITask target)
        {
            task.Append(new WaitTask(target, task.Looper));
            return task;
        }

        public static ITaskChain If(this ITaskChain task,Func<bool> condition, ITask target)
        {
            task.Append(new ConditionTask(condition, target, task.Looper));
            return task;
        }

        public static ITaskChain IfNot(this ITaskChain task, Func<bool> condition, ITask target)
        {
            task.Append(new ConditionTask(()=>!condition(), target, task.Looper));
            return task;
        }
    }
}
