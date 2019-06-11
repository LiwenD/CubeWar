//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using YummyGame.Framework;

////消息定义类,其中的泛型参数代表消息传递时候的参数类型
////每种消息一个对应的实例，并且设成静态对象
//class Msg
//{
//    //一个参数
//    public static readonly MsgWrapper<EventArgs> Event1 = new MsgWrapper<EventArgs>();
//    //两个参数
//    public static readonly MsgWrapper<int,Vector3> Event2 = new MsgWrapper<int, Vector3>();
//    //没有参数
//    public static readonly MsgWrapper Event3 = new MsgWrapper();
//}

//class EventArgs
//{
//    public int id;
//    public string name;
//}


//internal class EventTest:MonoBehaviour
//{
//    private void Start()
//    {
//        //监听
//        Msg.Event1.AddListener(OnEvent1);
//        Msg.Event2.AddListener(OnEvent2);

//    }

//    private void OnEvent2(int args1,Vector3 args2)
//    {
//        Debug.Log(args1);
//        Debug.Log(args2);
//    }

//    private void OnEvent1(EventArgs obj)
//    {
//        Debug.Log(obj.id);
//        Debug.Log(obj.name);
//    }

//    private void Update()
//    {
//        //发送事件
//        if (Input.GetKeyDown(KeyCode.Q))
//        {
//            Msg.Event1.Dispach(new EventArgs() { id = 1, name = "222" });
//        }

//        if (Input.GetKeyDown(KeyCode.W))
//        {
//            Msg.Event2.Dispach(1,Vector3.zero);
//        }

//        if (Input.GetKeyDown(KeyCode.C))
//        {
//            Msg.Event1.RemoveListener(OnEvent1);
//        }
//    }
//}
