using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace YummyGame.Framework
{
    public class RepeatEventTask : YummyTask<NullObject>
    {
        private Action func;
        private int repeat;
        public RepeatEventTask(Action func,int repeat,TaskLooper looper = null) : base(looper)
        {
            this.func = func;
            this.repeat = repeat;
        }

        protected override void OnTaskUpdateInternal()
        {
            try
            {
                for (int i = 0; i < repeat; i++)
                {
                    func.Invoke();
                }
                State = TaskState.Finish;
            }
            catch (Exception e)
            {
                error = e.Message + "\n" + e.StackTrace;
                State = TaskState.Failure;
                Debug.LogError(error);
            }
        }
    }
}
