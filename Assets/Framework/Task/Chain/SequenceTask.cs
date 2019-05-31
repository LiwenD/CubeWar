using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace YummyGame.Framework
{
    public class SequenceTaskChain : YummyTask<NullObject>,ITaskChain
    {
        private readonly Queue<ITask> m_tasks = new Queue<ITask>();
        private readonly List<ITask> m_tasksList = new List<ITask>();
        public SequenceTaskChain(TaskLooper looper = null) : base(looper) { }

        public TaskLooper Looper => base.looper;

        public ITaskChain Append(ITask task)
        {
            m_tasks.Enqueue(task);
            m_tasksList.Add(task);
            task.AutoKill = false;
            return this;
        }

        protected override void OnTaskStartInternal()
        {
            if (m_tasks.Count > 0)
            {
                m_tasks.Peek().Start();
            }
        }

        protected override void OnTaskUpdateInternal()
        {
            if (m_tasks.Count == 0)
            {
                State = TaskState.Finish;
                return;
            }
            if(m_tasks.Peek().GetState() == TaskState.Finish)
            {
                m_tasks.Dequeue();
                if (m_tasks.Count == 0)
                {
                    State = TaskState.Finish;
                    return;
                }
                m_tasks.Peek().Start();
            }
            else if(m_tasks.Peek().GetState() == TaskState.Failure)
            {
                State = TaskState.Failure;
                error = m_tasks.Peek().error;
                Debug.LogError(error);
            }
            else if (m_tasks.Peek().GetState() == TaskState.Killed)
            {
                State = TaskState.Failure;
                error = m_tasks.Peek().error;
            }
        }

        protected override void OnKilledInternal()
        {
            m_tasks.Clear();
            m_tasksList.Clear();
        }

        public override void Kill()
        {
            m_tasksList.ForEach((task) => task.Kill());
            base.Kill();
        }

        ITask ITaskChain.Start()
        {
            base.Start();
            return this;
        }
    }
}
