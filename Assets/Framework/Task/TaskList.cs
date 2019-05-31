using System;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.Framework
{
    public class TaskList : YummyTask<NullObject>
    {
        private List<TaskTweener> taskList;
        private int cur = 0;
        public TaskList()
        {
            taskList = new List<TaskTweener>();
        }

        public int TaskCount { get => taskList.Count; }

        public YummyTask<NullObject> Append(ITask task)
        {
            task.AutoKill = false;
            var tweener = new TaskTweener(task);
            tweener.SetAutoKill(false);
            taskList.Add(tweener);
            return this;
        }

        public YummyTask<NullObject> Join(ITask task)
        {
            if(taskList.Count == 0)
            {
                Append(task);
            }
            else
            {
                taskList[taskList.Count - 1].Join(task);
            }
            return this;
        }

        public override bool AutoKill
        {
            get => base.AutoKill;
            set
            {
                taskList.ForEach((task) => task.AutoKill = value);
                base._autoKill = value;
            }
        }

        public override void Start()
        {
            if (taskList.Count > 0)
            {
                taskList[0].Start();
            }
            base.Start();
        }

        public override void Pause()
        {
            base.Pause();
            if (cur < taskList.Count && taskList[cur].State == TaskState.Excuting)
            {
                taskList[cur].Pause();
            }
        }

        protected override void OnTaskUpdateInternal()
        {
            if (cur >= taskList.Count)
            {
                State = TaskState.Finish;
            }
            else
            {
                if(taskList[cur].State == TaskState.Finish || taskList[cur].State == TaskState.Killed)
                {
                    cur++;
                    if (cur >= taskList.Count)
                    {
                        State = TaskState.Finish;
                    }
                    else
                    {
                        taskList[cur].Start();
                    }
                }else if(taskList[cur].State == TaskState.Failure)
                {
                    error = taskList[cur].error;
                    State = TaskState.Failure;
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            cur = 0;
            foreach (var task in taskList)
            {
                task.Reset();
            }
        }

        public override void Continue()
        {
            base.Continue();
            if (cur < taskList.Count && taskList[cur].State == TaskState.Pause)
            {
                taskList[cur].Continue();
            }
        }

        public override void Interrupt()
        {
            base.Interrupt();
            if (cur < taskList.Count)
            {
                taskList[cur].Interrupt();
            }
        }

        public override void Kill()
        {
            base.Kill();
            taskList.ForEach((task) => task.Kill());
        }

        public override ITask SetLoop(int loopCount)
        {
            if (loopCount != 1)
            {
                taskList.ForEach((task) => task.SetAutoKill(false));
            }
            return base.SetLoop(loopCount);
        }
    }
}
