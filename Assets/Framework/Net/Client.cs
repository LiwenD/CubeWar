using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace YummyGame.Framework
{
    public enum NetState:byte
    {
        Connect = 0,
        ReceiveMsg = 1,
        ConnectTimeout = 2,
        Disconnect = 3,
        Error = 4
    }

    public abstract class Client
    {
        public class ClientState
        {
            public Signal0 onConnect;
            public Signal1<string> onTimeOut;

            public ClientState()
            {
                onConnect = new Signal0();
                onTimeOut = new Signal1<string>();
            }

            public ClientState OnConnect(Action action)
            {
                onConnect.AddOnce(action);
                return this;
            }

            public ClientState OnTimeout(Action<string> action)
            {
                onTimeOut.AddOnce(action);
                return this;
            }
        }

        protected Socket socket;
        protected string host;
        protected int port;

        protected ClientState _state;


        private Queue<byte[]> events;
        private Queue<byte[]> wait_queue;

        public bool IsConnect
        {
            get
            {
                return socket != null && socket.Connected;
            }
        }

        public Client(string host,int port)
        {
            this.host = host;
            this.port = port;
            events = new Queue<byte[]>();
            wait_queue = new Queue<byte[]>();
            socket = OnSocketInit();

            _state = new ClientState();
        }

        public void PopEvent()
        {

            lock (events)
            {
                if (events.Count > 0)
                {
                    byte[] data = events.Dequeue();
                    NetState state = (NetState)data[0];
                    switch (state)
                    {
                        case NetState.Connect:
                            _state.onConnect.Dispatch();
                            break;
                        case NetState.ConnectTimeout:
                            string msg = Encoding.UTF8.GetString(data, 1, data.Length - 1);
                            _state.onTimeOut.Dispatch(msg);
                            break;
                        default:
                            break;
                    }
                }
            }
            
        }

        protected abstract Socket OnSocketInit();
        protected virtual void StartReceive(Socket s) { }

        public ClientState Connect()
        {
            try
            {
                IAsyncResult result = socket.BeginConnect(new IPEndPoint(IPAddress.Parse(host), port), (ar) =>
                {
                    try
                    {
                        Socket _s = (Socket)ar.AsyncState;
                        _s.EndConnect(ar);

                        PushEvent(NetState.Connect);

                        StartReceive(_s);
                    }
                    catch(Exception e)
                    {
                        PushEvent(NetState.ConnectTimeout, e.Message);
                    }
                    
                }, socket);
                ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, WaitTimeOut, null, 1000, true);
            }catch(Exception e)
            {
                PushEvent(NetState.Error, e.Message);
            }
            
            return _state;
        }

        private void WaitTimeOut(object state ,bool timeout)
        {
            if (timeout)
            {
                Debug.LogError("连接超时");
                PushEvent(NetState.ConnectTimeout,"连接超时");
            }
        }

        protected void PushEvent(NetState state)
        {
            byte b = (byte)state;
            PushEvent(b);
        }

        protected void PushEvent(NetState state, string msg)
        {
            byte b = (byte)state;
            PushEvent(b, Encoding.UTF8.GetBytes(msg));
        }

        protected void PushEvent(NetState state,byte[] data)
        {
            byte b = (byte)state;
            PushEvent(b,data);
        }

        private void PushEvent(byte state)
        {
            lock (events)
            {
                events.Enqueue(new byte[] { state });
            }
        }

        private void PushEvent(byte state,byte[] data)
        {
            List<byte> buffer = new List<byte>();
            buffer.Add(state);
            buffer.AddRange(data);
            lock (events)
            {
                events.Enqueue(buffer.ToArray());
            }
        }
    }
}
