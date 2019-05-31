using System;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.Framework
{
    public class WaitTask : YummyTask<NullObject>
    {
        private ITask task;
        public WaitTask(ITask target,TaskLooper looper = null) : base(looper)
        {
            task = target;
        }

        public override void Start()
        {
            if (task.GetState() == TaskState.Create) task.Start();
            base.Start();
        }

        protected override void OnTaskUpdateInternal()
        {
            if(task.GetState() == TaskState.Finish)
            {
                State = TaskState.Finish;
            }else if(task.GetState() == TaskState.Failure)
            {
                error = task.error;
                State = TaskState.Failure;
            }else if(task.GetState() == TaskState.Killed)
            {
                State = TaskState.Failure;
                error = "任务执行异常";
            }
        }
    }
}
