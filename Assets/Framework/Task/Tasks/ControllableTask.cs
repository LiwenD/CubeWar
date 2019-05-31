using System;
using System.Collections.Generic;

namespace YummyGame.Framework
{
    public class ControllableTask : YummyTask<NullObject>
    {
        Action<ITask> func;
        public ControllableTask(Action<ITask> func,TaskLooper looper = null) : base(looper)
        {
            this.func = func;
        }
        protected override void OnTaskUpdateInternal()
        {
            
        }
    }
}
