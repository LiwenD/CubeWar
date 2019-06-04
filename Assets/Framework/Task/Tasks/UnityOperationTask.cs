using System;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.Framework
{
    public class UnityOperationTask : YummyTask<NullObject>
    {
        AsyncOperation async;

        public UnityOperationTask(AsyncOperation async,TaskLooper looper = null):base(looper)
        {
            this.async = async;
        }

        protected override void OnTaskUpdateInternal()
        {
            if (async.isDone)
            {
                State = TaskState.Finish;
            }
        }
    }
}
