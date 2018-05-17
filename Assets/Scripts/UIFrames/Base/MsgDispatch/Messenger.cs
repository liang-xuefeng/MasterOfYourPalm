using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace Titan.UI
{
    public abstract class Messenger : MonoBehaviour, IMsgRegister
    {
        private Dictionary<string, EventHandler> m_eventCollection = new Dictionary<string, EventHandler>();
        
        public virtual void RegisterMsg(string msgType,object sender,EventHandler handler)
        {
            if (handler != null)
            {
                AddHandler(msgType, handler);
            }
        }

        public virtual void UnRegisterMsg(string msgType, object sender, EventHandler handler)
        {

            if (m_eventCollection.ContainsKey(msgType))
            {
                m_eventCollection[msgType] -= handler;
                m_eventCollection.Remove(msgType);
            }
        }

        public virtual void SendMsg(string msgType,object sender,EventArgs eventArgs)
        {
            if (!m_eventCollection.ContainsKey(msgType))
            {
                Debug.Log("<color=red>Send Msg ,but no msg deleget [ " + msgType + " ] was registered before</color>");
            }
            else
            {
                m_eventCollection[msgType].Invoke(sender, eventArgs);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Message Reciver Info:\n\nName: ").Append(name).Append("\n\nRegisters:\n");
            foreach (var each in m_eventCollection)
            {
                sb.Append("\nKey: ").Append(each.Key).Append("\nHandler: \n").Append(handleInfo(each.Value));
            }
            return sb.ToString();
        }

        private void AddHandler(string msgType, EventHandler handle)
        {
            if (m_eventCollection.ContainsKey(msgType))
            {
                //make sure it's not mutiply
                m_eventCollection[msgType] -= handle;
                m_eventCollection[msgType] += handle;
            }
            else
            {
                m_eventCollection[msgType] = handle;
            }
        }

        private string handleInfo(EventHandler handler)
        {
            StringBuilder sb = new StringBuilder();
            var list = handler.GetInvocationList();
            for (int i = 0; i < list.Length; ++i)
            {
                sb.Append(list[i].Method.Name).Append("@").Append(getTargetPath(list[i].Target)).Append("\n");
            }
            return sb.ToString();
        }

        private string getTargetPath(object target)
        {
            if (target is MonoBehaviour)
            {
                GameObject obj = ((MonoBehaviour)target).gameObject;
                StringBuilder sb = new StringBuilder();
                while (obj != null)
                {
                    sb.Insert(0, "/" + obj.name);
                    if (obj.transform.parent != null)
                    {
                        obj = obj.transform.parent.gameObject;
                    }
                    else
                    {
                        obj = null;
                    }
                }
                sb.Append("(" + target.ToString() + ")");
                return sb.ToString();
            }
            else
            {
                return target.ToString();
            }
        }
    }
}