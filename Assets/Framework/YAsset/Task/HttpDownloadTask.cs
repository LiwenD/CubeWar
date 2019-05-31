using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace YummyGame.Framework
{
    public class HttpDownloadTask : YummyTask<NullObject>
    {
        private string url;
        private string fileName;
        byte[] buffer = new byte[2048];
        public HttpDownloadTask(string url,string fileName, TaskLooper looper = null) : base(looper)
        {
            this.url = url;
            this.fileName = fileName;
        }

        protected override void OnTaskStartInternal()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.BeginGetResponse(DownloadFinished, request);
        }

        protected override void OnTaskUpdateInternal()
        {
            
        }

        private void DownloadFinished(IAsyncResult ar)
        {
            try
            {
                WebRequest request = ar.AsyncState as WebRequest;
                WebResponse response = request.EndGetResponse(ar);
                Stream s = response.GetResponseStream();
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                using(FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    int n = 0;
                    while ((n = s.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fs.Write(buffer, 0, n);
                    }
                }
                State = TaskState.Finish;
            }catch(Exception e)
            {
                State = TaskState.Failure;
                error = e.Message + "\n" + e.StackTrace;
            }
        }
    }
}
