using System;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.Framework
{
    public static class UIWindowTask
    {
        public static YummyTask<GameObject> OpenUI(UIWindow window)
        {
            UIWindowTask_Internal task = new UIWindowTask_Internal(window);
            task.Start();
            return task;
        }

        public class UIWindowTask_Internal:YummyTask<GameObject>
        {
            private UIWindow window;
            TaskState state;

            public UIWindowTask_Internal(UIWindow window)
            {
                this.window = window;
            }

            protected override void OnTaskStartInternal()
            {
                state = TaskState.Excuting;
                if(window.root==null)
                    window.loader.LoadAssetAsync<GameObject>(window.ResourcePath, (go) =>
                    {
                        state = TaskState.Finish;
                        target = GameObject.Instantiate<GameObject>(go);
                    });
                else
                {
                    target = window.root;
                    window.root.SetActive(true);
                    state = TaskState.Finish;
                }
            }

            protected override void OnTaskUpdateInternal()
            {
                State = state;
            }
        }
    }
}
