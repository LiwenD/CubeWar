using System;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.Framework
{
    public class DelayTask : YummyTask<NullObject>
    {
        private float delay;
        private float startTime;
        public DelayTask(float delay, TaskLooper looper = null) : base(looper)
        {
            this.delay = delay;
        }

        protected override void OnTaskStartInternal()
        {
            startTime = Time.time;
        }

        protected override void OnTaskUpdateInternal()
        {
            if(Time.time - startTime >= delay)
            {
                State = TaskState.Finish;
            }
        }
    }
}
