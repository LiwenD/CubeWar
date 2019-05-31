using System;
using System.Collections.Generic;
using UnityEngine;


namespace YummyGame.Framework
{
    public class YummyTaskState: ITaskState
    {
        private ITask task;
        public YummyTaskState(ITask task)
        {
            this.task = task;
        }


        public TaskState State { get; protected set; }
        public virtual bool IsEnd => State == TaskState.Failure
                                        || State == TaskState.Interrupt
                                        || State == TaskState.Finish
                                        || State == TaskState.Killed;
        public virtual bool AutoKill { get => _autoKill; set { _autoKill = value; } }
        public string error { get; set; }

        protected int _loop = 1;
        protected int _originalLoop = 1;
        protected bool _autoKill = false;

        public virtual ITask SetLoop(int loopCount)
        {
            _loop = loopCount;
            _originalLoop = loopCount;
            return task;
        }

        public TaskState GetState()
        {
            return State;
        }
    }
}
