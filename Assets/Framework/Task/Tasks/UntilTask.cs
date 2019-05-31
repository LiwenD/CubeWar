using System;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.Framework
{
    public class UntilTask : YummyTask<NullObject>
    {
        Func<bool> predicate;
        public UntilTask(Func<bool> predicate,TaskLooper looper = null):base(looper)
        {
            this.predicate = predicate;
        }
        protected override void OnTaskUpdateInternal()
        {
            try
            {
                if (predicate())
                {
                    State = TaskState.Finish;
                }
            }catch(Exception e)
            {
                State = TaskState.Failure;
                error = e.Message + "\n" + e.StackTrace;
                Debug.LogError(error);
            }
        }
    }
}
