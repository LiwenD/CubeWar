using System;
using System.Collections;

namespace YummyGame.Framework
{
    public interface ITask:ILooper,IEnumerator,IDisposable, ITaskState,IPoolObject
    {
        void Start();

        void Pause();

        void Interrupt();

        void Continue();

        void Kill();

    }

    public interface ITaskState
    {
        ITask SetLoop(int loopCount);

        TaskState GetState();

        bool AutoKill { get; set; }

        bool IsEnd { get; }

        string error { get; set; }
    }

    public interface ITaskChain:ITask
    {
        TaskLooper Looper { get; }
        ITaskChain Append(ITask task);
        new ITask Start();
    }

    public interface ITaskCallback<T>
    {
        ITaskCallback<T> SetLoop(int loopCount);
        ITaskCallback<T> OnTaskStart(Action callback);
        ITaskCallback<T> OnTaskUpdate(Action callback);
        ITaskCallback<T> OnTaskFinish(Action<T> callback);
        ITaskCallback<T> OnTaskError(Action<string> callback);
        ITaskCallback<T> OnTaskKill(Action callback);
    }
}
