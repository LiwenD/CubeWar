using System;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.Framework
{
    public class WaitFrameTask:YummyTask<NullObject>
    {
        private int _startFrame;
        private int total;

        public WaitFrameTask(int frameCount)
        {
            total = frameCount;
        }
        protected override void OnTaskUpdateInternal()
        {
            if (Time.frameCount - _startFrame >= total)
            {
                State = TaskState.Finish;
            }
        }

        public override void Start()
        {
            base.Start();
            _startFrame = Time.frameCount;
        }
    }
}
