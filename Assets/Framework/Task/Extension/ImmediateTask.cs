using System;
using System.Collections.Generic;

namespace YummyGame.Framework
{
    public static class ImmediateTask
    {
        public static ImmediateTask_Internal Start()
        {
            return new ImmediateTask_Internal();
        }
        public class ImmediateTask_Internal : YummyTask<NullObject>
        {
            public override void Start()
            {
                throw new Exception("这个任务不能被启动");
            }
            protected override void OnTaskUpdateInternal()
            {
                
            }

            public override YummyTask<NullObject> OnTaskFinish(Action<NullObject> callback)
            {
                callback?.Invoke(NullObject.Default);
                return this;
            }
        }
    }
}
