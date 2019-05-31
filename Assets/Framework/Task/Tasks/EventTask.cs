using System;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.Framework
{
    public class EventTask : YummyTask<NullObject>
    {
        private Action func;
        public EventTask(Action func, TaskLooper looper = null):base(looper)
        {
            this.func = func;
        }

        protected override void OnTaskUpdateInternal()
        {
            try
            {
                func.Invoke();
                State = TaskState.Finish;
            }catch(Exception e)
            {
                error = e.Message+"\n"+e.StackTrace;
                State = TaskState.Failure;
                Debug.LogError(error);
            }
        }
    }
}
