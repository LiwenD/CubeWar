using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace YummyGame.Framework
{
    public class UIWindowMapping
    {
        private Dictionary<string, Dictionary<string, string>> m_instInject = new Dictionary<string, Dictionary<string, string>>();
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
        }

        private void _buildInst(Type windowType, UIWindow window)
        {
            m_instInject.Add(window.ResourcePath, new Dictionary<string, string>());
            FieldInfo[] fields = windowType.GetFields();
            foreach (var field in fields)
            {
                if ((field.FieldType != typeof(GameObject)) &&
                    !field.FieldType.IsSubclassOf(typeof(Component))) continue;
                object[] attrs = field.GetCustomAttributes(typeof(InstAttribute), false);
                if (attrs == null || attrs.Length == 0) continue;
                InstAttribute inst = (InstAttribute)attrs[0];
                m_instInject[window.ResourcePath].Add(field.Name, inst.path);
                _inst(window, field, inst.path);
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
    }
}
