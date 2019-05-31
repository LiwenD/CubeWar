using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace YummyGame.Framework
{
    public static class ResourcesTask
    {
        public static ResourcesTask_Internal Load(string resPath, Type type)
        {
            ResourcesTask_Internal task = new ResourcesTask_Internal(resPath, type);
            task.Start();
            return task;
        }
        public class ResourcesTask_Internal : YummyTask<Asset>
        {
            ResourceRequest request;
            string resPath;
            Type resType;
            public ResourcesTask_Internal(string resPath, Type type, TaskLooper looper = null) : base(looper)
            {
                this.resPath = resPath;
                this.resType = type;
            }

            protected override void OnTaskStartInternal()
            {
                request = Resources.LoadAsync(resPath, resType);
            }

            protected override void OnTaskUpdateInternal()
            {
                if (request.isDone)
                {
                    if (request.asset == null)
                    {
                        State = TaskState.Failure;
                        error = "不能找到对应的资源:" + resPath;
                    }
                    else
                    {
                        target = new Asset(resPath, resType, request.asset);
                        State = TaskState.Finish;
                    }
                }
            }
        }
    }
    
}
