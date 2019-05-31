using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace YummyGame.Framework
{
    public static class WWWTask
    {
        public static WWWTask_Internal Get(string url)
        {
            WWWTask_Internal www = new WWWTask_Internal(new WWW(url));
            www.Start();
            return www;
        }

        public static WWWTask_Internal Get(string url, WWWForm form)
        {
            WWWTask_Internal www = new WWWTask_Internal(new WWW(url,form));
            www.Start();
            return www;
        }

        public static WWWTask_Internal Get(string url, byte[] postData)
        {
            WWWTask_Internal www = new WWWTask_Internal(new WWW(url, postData));
            www.Start();
            return www;
        }

        public static WWWTask_Internal Get(string url, byte[] postData, Hashtable headers)
        {
            WWWTask_Internal www = new WWWTask_Internal(new WWW(url, postData,headers));
            www.Start();
            return www;
        }

        public static WWWTask_Internal Get(string url, byte[] postData, Dictionary<string, string> headers)
        {
            WWWTask_Internal www = new WWWTask_Internal(new WWW(url, postData, headers));
            www.Start();
            return www;
        }

        public class WWWTask_Internal : YummyTask<WWW>
        {
            public WWWTask_Internal(WWW www)
            {
                target = www;
            }
            protected override void OnTaskUpdateInternal()
            {
                if (!string.IsNullOrEmpty(target.error))
                {
                    State = TaskState.Failure;
                    error = target.error;
                }
                else
                {
                    if (target.isDone)
                    {
                        State = TaskState.Finish;
                    }
                }
            }

            protected override void OnKilledInternal()
            {
                target?.Dispose();
            }
        }
    }

    
}
