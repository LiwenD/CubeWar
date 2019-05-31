using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace YummyGame.Framework
{
    public enum TaskState
    {
        Create,
        Excuting,
        Pause,
        Finish,
        Interrupt,
        Failure,
        Killed
    }

    public class TaskObservor<T>
    {
        public Action onTaskStart;
        public Action onTaskUpdate;
        public Action<T> onTaskFinish;
        public Action onTaskInterrupted;
        public Action<string> onTaskError;
        public Action onTaskKill;

        public void Clear()
        {
            onTaskStart = null;
            onTaskUpdate = null;
            onTaskFinish = null;
            onTaskInterrupted = null;
            onTaskError = null;
            onTaskKill = null;
        }

        public virtual void OnTaskStartInvoke(out string error)
        {
            error = "";
            try
            {
                onTaskStart?.Invoke();
            }
            catch(Exception e)
            {
                error = e.Message + "\n" + e.StackTrace;
            }
            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError(error);
            }
        }

        public virtual void OnTaskUpdateInvoke(out string error)
        {
            error = "";
            try
            {
                onTaskUpdate?.Invoke();
            }
            catch(Exception e)
            {
                error = e.Message + "\n" + e.StackTrace;
            }
            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError(error);
            }
        }

        public virtual void OnTaskFinishInvoke(T args,out string error)
        {
            error = "";
            try
            {
                onTaskFinish?.Invoke(args);
            }
            catch(Exception e)
            {
                error = e.Message + "\n" + e.StackTrace;
            }
            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError(error);
            }
        }

        public virtual void OnTaskInterruptedInvoke(out string error)
        {
            error = "";
            try
            {
                onTaskInterrupted?.Invoke();
            }
            catch(Exception e)
            {
                error = e.Message + "\n" + e.StackTrace;
            }
            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError(error);
            }
        }

        public virtual void OnTaskErrorInvoke(string error,out string error2)
        {
            error2 = "";
            try
            {
                onTaskError?.Invoke(error);
            }
            catch(Exception e)
            {
                error2 = e.Message + "\n" + e.StackTrace;
            }
            if (!string.IsNullOrEmpty(error2))
            {
                Debug.LogError(error2);
            }
        }

        public virtual void OnTaskKillInvoke()
        {
            try
            {
                onTaskKill?.Invoke();
            }catch(Exception e)
            {
                Debug.LogError(e.Message + "\n" + e.StackTrace);
            }
        }
    }  

    public abstract class YummyTask<T> : ITask
    {
        protected T target = default(T);
        public string error { get; set; }
        protected string error2 = "";
        
        public TaskState State { get; protected set; }
        public virtual bool IsEnd => State == TaskState.Failure 
                                        || State == TaskState.Interrupt 
                                        || State == TaskState.Finish
                                        || State == TaskState.Killed;
        public virtual bool AutoKill { get => _autoKill; set { _autoKill = value; } }

        public object Current => null;

        protected TaskObservor<T> _taskObserver;
        protected TaskLooper looper;
        protected int _loop = 1;
        protected int _originalLoop = 1;
        protected bool _autoKill = false;

        public YummyTask(TaskLooper looper = null)
        {
            State = TaskState.Create;
            this.looper = looper;
            if(looper == null)
            {
                this.looper = Facade.Instance.taskLooper;
            }
            _taskObserver = new TaskObservor<T>();
        }

        public virtual ITask SetLoop(int loopCount)
        {
            _loop = loopCount;
            _originalLoop = loopCount;
            return this;
        }

        public virtual void Start()
        {
            if (State == TaskState.Killed) throw new Exception("The Task is Killed!");
            if(State == TaskState.Create)
                looper.StartTask(this);
        }

        public virtual void Pause()
        {
            if(State == TaskState.Excuting)
            {
                State = TaskState.Pause;
            }
            else
            {
                Debug.LogWarning("Task is not in Excuting State!!");
            }
        }

        public virtual void Interrupt()
        {
            if(State == TaskState.Excuting || State == TaskState.Pause)
                State = TaskState.Interrupt;
        }

        public virtual void Continue()
        {
            if (State == TaskState.Pause)
                State = TaskState.Excuting;
        }

        public virtual void Kill()
        {
            looper.KillTask(this, _InternalKill);
        }

        private void _InternalKill()
        {
            State = TaskState.Killed;
            OnKilledInternal();
            _taskObserver.OnTaskKillInvoke();
            _taskObserver.Clear();
        }

        #region Callback
        public YummyTask<T> SetAutoKill(bool autoKill)
        {
            AutoKill = autoKill;
            return this;
        }

        public YummyTask<T> OnTaskStart(Action callback)
        {
            _taskObserver.onTaskStart += callback;
            return this;
        }

        public YummyTask<T> OnTaskUpdate(Action callback)
        {
            _taskObserver.onTaskUpdate += callback;
            return this;
        }

        public virtual YummyTask<T> OnTaskFinish(Action<T> callback)
        {
            _taskObserver.onTaskFinish += callback;
            return this;
        }
        public YummyTask<T> OnTaskInterrupt(Action callback)
        {
            _taskObserver.onTaskInterrupted += callback;
            return this;
        }

        public YummyTask<T> OnTaskError(Action<string> callback)
        {
            _taskObserver.onTaskError += callback;
            return this;
        }

        public YummyTask<T> OnTaskKill(Action callback)
        {
            _taskObserver.onTaskKill += callback;
            return this;
        }


        #endregion
        protected virtual void OnTaskStartInternal() { }
        protected abstract void OnTaskUpdateInternal();
        protected virtual void OnInterruptInternal() { }
        protected virtual void OnKilledInternal() { }
        

        public bool MoveNext()
        {
            OnTaskUpdateInternal();
            if (State == TaskState.Finish)
            {
                _loop--;
                var curLoop = _loop;
                if (_loop == 0)
                {
                    _taskObserver.OnTaskFinishInvoke(target,out error2);
                    if (!string.IsNullOrEmpty(error2))
                    {
                        Kill();
                        return false;
                    }
                }
                    
                else
                {
                    Reset();
                    Start();
                    Debug.Log("chong qi");
                    //解决循环重置错误
                    _loop = curLoop;
                }
                return false;
            }
            if (State == TaskState.Interrupt)
            {
                OnInterruptInternal();
                _taskObserver.OnTaskInterruptedInvoke(out error2);
                if (!string.IsNullOrEmpty(error2))
                {
                    Kill();
                }
                return false;
            }
            if(State == TaskState.Failure)
            {
                _taskObserver.OnTaskErrorInvoke(error,out error2);
                if (!string.IsNullOrEmpty(error2))
                {
                    Kill();
                }
                return false;
            }

            return true;
        }

        public virtual void OnEnable()
        {
            State = TaskState.Create;
            _loop = _originalLoop;
        }
        public virtual void OnDisable()
        {
            
        }

        protected virtual void ResetInternal()
        {
            if (State == TaskState.Killed || State == TaskState.Excuting) return;
            State = TaskState.Create;
        }


        public virtual void Reset()
        {
            if (State == TaskState.Killed || State == TaskState.Excuting) return;
            State = TaskState.Create;
            _loop = _originalLoop;
        }

        public void onLooperStart()
        {
            State = TaskState.Excuting;
            try
            {
                OnTaskStartInternal();
            }
            catch { }
            _taskObserver.OnTaskStartInvoke(out error2);
            if (!string.IsNullOrEmpty(error2))
            {
                Kill();
            }
        }

        public bool onLooperUpdate()
        {
            if (State == TaskState.Pause) return true;
            return MoveNext();
        }

        public void onLooperEnd()
        {
            // check _loop
            if (AutoKill&&_loop==0)
            {
                looper.KillTask(this,_InternalKill);
            }
        }

        public void Dispose()
        {
            Kill();
        }

        public TaskState GetState()
        {
            return State;
        }

        public TaskListener GetListener()
        {
            TaskListener listener = new TaskListener(this);
            listener.Start();
            return listener;
        }
    }

    public static class Task
    {
        public static ITaskChain Sequence()
        {
            return new SequenceTaskChain();
        }

        public static ITaskChain Parallel()
        {
            return new ParallelTaskChain();
        }
    }
}


