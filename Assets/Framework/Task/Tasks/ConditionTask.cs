using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace YummyGame.Framework
{
    public class ConditionTask : YummyTask<NullObject>
    {
        Func<bool> condtion;
        bool finish;
        ITask task;
        public ConditionTask(Func<bool> condtion,ITask task,TaskLooper looper = null) : base(looper)
        {
            this.condtion = condtion;
            this.task = task;
        }

        protected override void OnTaskStartInternal()
        {
            finish = condtion();
            if (!finish) State = TaskState.Finish;
            else task.Start();
        }

        protected override void OnTaskUpdateInternal()
        {
            if(!finish)
            {
                return;
            }
            if (task.GetState() == TaskState.Finish)
            {
                State = TaskState.Finish;
            }else if(task.GetState() == TaskState.Failure)
            {
                error = task.error;
                DebugView.Log("task error:" + error);
                State = TaskState.Failure;
            }else if (task.GetState() == TaskState.Killed)
            {
                State = TaskState.Finish;
            }
        }
    }
}
