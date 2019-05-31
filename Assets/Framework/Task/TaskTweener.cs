using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyGame.Framework
{
    public class TaskTweener : YummyTask<NullObject>
    {
        protected List<ITask> tasks;

        public TaskTweener()
        {
            tasks = new List<ITask>();
        }

        public TaskTweener(ITask task)
        {
            tasks = new List<ITask>();
            Join(task);
        }

        public override bool AutoKill
        {
            get => base.AutoKill;
            set
            {
                tasks.ForEach((task) => task.AutoKill = value);
                base._autoKill = value;
            }
        }

        public override bool IsEnd
        {
            get
            {
                foreach (var task in tasks)
                {
                    if (!task.IsEnd) return false;
                }
                return true;
            }
        }

        public TaskTweener Join(ITask task)
        {
            task.AutoKill = false;
            tasks.Add(task);
            return this;
        }

        protected override void OnTaskUpdateInternal()
        {
            if (IsEnd)
            {
                State = TaskState.Finish;
            }
        }

        public override void Continue()
        {
            base.Continue();
            tasks.ForEach((task) => task.Continue());
        }

        public override void Interrupt()
        {
            base.Interrupt();
            tasks.ForEach((task) => task.Interrupt());
        }

        public override void Pause()
        {
            base.Pause();
            tasks.ForEach((task) => task.Pause());
        }

        public override void Reset()
        {
            base.Reset();
            tasks.ForEach((task) => task.Reset());
        }

        public override void Start()
        {
            tasks.ForEach((task) => task.Start());
            base.Start();
        }

        public override void Kill()
        {
            tasks.ForEach((task) => task.Kill());
            base.Kill();
        }

        public override ITask SetLoop(int loopCount)
        {
            if (loopCount != 1)
            {
                tasks.ForEach((task) => task.AutoKill = false);
            }
            return base.SetLoop(loopCount);
        }
    }
}
