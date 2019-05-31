using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace YummyGame.Framework
{
    public static class WebRequestTask
    {
        public static WebRequestTask_Internal Get(string uri)
        {
            WebRequestTask_Internal task = new WebRequestTask_Internal(UnityWebRequest.Get(uri));
            task.Start();
            return task;
        }
    }

    public class WebRequestTask_Internal : YummyTask<UnityWebRequest>
    {
        public WebRequestTask_Internal(UnityWebRequest request)
        {
            target = request;
        }
        protected override void OnTaskUpdateInternal()
        {
            
        }

        public override void Start()
        {
            base.Start();
            target.SendWebRequest();
        }
    }
}
