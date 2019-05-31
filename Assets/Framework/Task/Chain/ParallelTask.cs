using System;
using System.Collections.Generic;

namespace YummyGame.Framework
{
    public class ParallelTaskChain : YummyTask<NullObject>, ITaskChain
    {
        public TaskLooper Looper => looper;

        private List<ITask> m_tasks = new List<ITask>();

        public ParallelTaskChain(TaskLooper looper = null) : base(looper) { }

        public ITaskChain Append(ITask task)
        {
            m_tasks.Add(task);
            return this;
        }

        protected override void OnTaskUpdateInternal()
        {
            int finishCount = 0;
            foreach (var task in m_tasks)
            {
                if (task.GetState() == TaskState.Finish) finishCount++;
                else if (task.GetState() == TaskState.Failure)
                {
                    error = task.error;
                    State = TaskState.Failure;
                    return;
                }
            }
            if(finishCount == m_tasks.Count)
            {
                State = TaskState.Finish;
            }
        }

        protected override void OnKilledInternal()
        {
            base.OnKilledInternal();
            m_tasks.Clear();
        }

        public override void Kill()
        {
            m_tasks.ForEach((task) => task.Kill());
            base.Kill();
        }

        ITask ITaskChain.Start()
        {
            foreach (var task in m_tasks)
            {
                task.AutoKill = false;
                task.Start();
            }
            base.Start();
            return this;
        }
    }
}
