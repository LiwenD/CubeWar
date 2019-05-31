using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace YummyGame.Framework
{
    public class TaskListener:YummyTask<NullObject>
    {
        ITask targetListener;
        public TaskListener(ITask task)
        {
            targetListener = task;
        }
        protected override void OnTaskUpdateInternal()
        {
            State = targetListener.GetState();
            //目标还没启动
            if(State == TaskState.Create)
            {
                State = TaskState.Excuting;
            }
            if(State == TaskState.Failure)
            {
                error = targetListener.error;
            }
        }
    }
    
}
