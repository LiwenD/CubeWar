using System;
using System.Collections.Generic;

namespace YummyGame.Framework
{
    public class DecoratorTask<T>:YummyTask<T>
    {
        private ITask task;
        private Func<T> callback;
        public DecoratorTask(ITask task,Func<T> callback, TaskLooper looper = null) : base(looper)
        {
            this.task = task;
            this.callback = callback;
        }


        protected override void OnTaskUpdateInternal()
        {
            State = task.GetState();
            if(State == TaskState.Finish)
            {
                target = this.callback();
            }
        }
    }
}
