using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace YummyGame.Framework
{
    public class UIWindowMapping
    {
        private Dictionary<string, Dictionary<string, string>> m_instInject = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, List<string>> m_autocloseInject = new Dictionary<string, List<string>>();


        List<FieldInfo> auto_delay = new List<FieldInfo>();
        public void BuildWindow(Type windowType,UIWindow window)
        {
            if (m_instInject.ContainsKey(window.ResourcePath))
            {
                _dicInst(windowType, window);
            }
            else
            {
                _buildInst(windowType, window);
            }
        }

        private void _dicInst(Type windowType,UIWindow window)
        {
            foreach (var info in m_instInject[window.ResourcePath])
            {
                FieldInfo field = windowType.GetField(info.Key);
                _inst(window, field, info.Value);
            }
            foreach (var info in m_autocloseInject[window.ResourcePath])
            {
                FieldInfo field = windowType.GetField(info);
                _autoClick(windowType,window, field);
            }
        }

        private void _buildInst(Type windowType, UIWindow window)
        {
            m_instInject.Add(window.ResourcePath, new Dictionary<string, string>());
            m_autocloseInject.Add(window.ResourcePath, new List<string>());
            FieldInfo[] fields = windowType.GetFields(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance);
            auto_delay.Clear();
            foreach (var field in fields)
            {
                if ((field.FieldType != typeof(GameObject)) &&
                    !field.FieldType.IsSubclassOf(typeof(Component))) continue;
                object[] attrs = field.GetCustomAttributes(false);
                if (attrs == null || attrs.Length == 0) continue;
                foreach(var attr in attrs)
                {
                    if(attr is InstAttribute)
                    {
                        InstAttribute inst = attr as InstAttribute;
                        m_instInject[window.ResourcePath].Add(field.Name, inst.path);
                        _inst(window, field, inst.path);
                    }else if(attr is AutoCloseAttribute)
                    {
                        if (field.FieldType != typeof(Button)) continue;
                        auto_delay.Add(field);
                    }
                }
                
            }

            foreach (var field in auto_delay)
            {
                m_autocloseInject[window.ResourcePath].Add(field.Name);
                _autoClick(windowType, window, field);
            }
        }

        private void _inst(UIWindow obj, FieldInfo field,string path)
        {
            Transform tran = obj.root.transform.Find(path);
            if (tran == null) return;
            if(field.FieldType == typeof(GameObject))
            {
                field.SetValue(obj, tran.gameObject);
            }
            else
            {
                field.SetValue(obj, tran.GetComponent(field.FieldType));
            }
        }

        private void _autoClick(Type windowType, UIWindow obj, FieldInfo field)
        {
            if (obj == null) return;
            MethodInfo method = windowType.GetMethod("Close",BindingFlags.NonPublic|BindingFlags.Instance);
            Button btn =(Button) field.GetValue(obj);
            btn.onClick.AddListener(() => method.Invoke(obj, null));
        }
    }
}
