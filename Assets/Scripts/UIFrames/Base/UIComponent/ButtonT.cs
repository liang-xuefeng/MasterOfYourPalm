using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.Events;

namespace Titan.UI
{
    public class ButtonT : Button
    {
        #region Public Attributes
        public MsgOption m_clickMsg = new MsgOption();
        #endregion

        #region Private Attributes
        private Panel m_messenger = null;
        #endregion

        #region Public Methods
        //ButtonT shouldn't send msg to reciver directily, because it's not a messenger
        public void SetMessenger(Panel register)
        {
            if (m_clickMsg.MsgSendType != MsgSendType.None)
            {
                //As some value maybe change,the equal judge shouldn't be apply
                UnRegisterMsg();
                m_messenger = register;
                RegisterMsg();
            }
        }
        #endregion

        #region Override Methods
        protected override void OnEnable()
        {
            base.OnEnable();
            RegisterMsg();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UnRegisterMsg();
        }

        public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            SendMsg();
        }
        #endregion

        #region Private Methods
        protected virtual void RegisterMsg()
        {
            if (m_clickMsg.MsgSendType == MsgSendType.ParentPanel)
            {
                if (m_messenger == null)
                {
                    m_messenger = GetComponentInParent<Panel>();
                }

                if (m_messenger == null)
                {
                    Debug.Log("<color=red> Can't find Messenger in UI" + gameObject.name + " parent </color> ");
                }
                else
                {
                    m_messenger.RegisterClickMsg(m_clickMsg.MsgName, gameObject);
                }
            }
        }

        protected virtual void UnRegisterMsg()
        {
            if (m_clickMsg.MsgSendType != MsgSendType.None)
            {
                if (m_messenger != null)
                {
                    m_messenger.UnRegisterClickMsg(m_clickMsg.MsgName, gameObject);
                }
            }
        }

        protected virtual void SendMsg()
        {
            if (m_clickMsg.MsgSendType != MsgSendType.None)
            {
                if (m_messenger == null)
                {
                    Debug.Log("<color=red> Want Send Msg ,But can't find Messenger at " + gameObject.name + "</color> ");
                    return;
                }

                if (m_messenger != null && !m_messenger.isReciverAvaliable())
                {
                    Debug.Log("<color=red> Want Send Msg at [" + gameObject.name + "],But can't find Reciver In Panel [" + m_messenger.name + "] </color> ", m_messenger);
                    return;
                }

                if (m_messenger != null && !m_messenger.IsClickHandlerAvaliable())
                {
                    Debug.Log("<color=red> Want Send Msg at [" + gameObject.name + "] ,But can't find Handle In Panel [" + m_messenger.name + "] </color> ", m_messenger);
                    return;
                }

                switch (m_clickMsg.DataType)
                {
                    case MsgOption.ConstDataType.None:
                        m_messenger.SendMsg(m_clickMsg.MsgName, gameObject, new UIEventArgs(m_clickMsg.MsgName));
                        break;
                    case MsgOption.ConstDataType.Int:
                        m_messenger.SendMsg(m_clickMsg.MsgName, gameObject, new UIEventArgs<int>(m_clickMsg.MsgName, m_clickMsg.intData));
                        break;
                    case MsgOption.ConstDataType.Float:
                        m_messenger.SendMsg(m_clickMsg.MsgName, gameObject, new UIEventArgs<float>(m_clickMsg.MsgName, m_clickMsg.floatData));
                        break;
                    case MsgOption.ConstDataType.String:
                        m_messenger.SendMsg(m_clickMsg.MsgName, gameObject, new UIEventArgs<string>(m_clickMsg.MsgName, m_clickMsg.stringData));
                        break;
                    case MsgOption.ConstDataType.Customer:
                        var callEvent = m_clickMsg.callEvent;
                        string methodName = callEvent.MethodName;
                        if (methodName.Length > 0)
                        {
                            MethodInfo method = UnityEventBase.GetValidMethodInfo(callEvent.Script, methodName, new Type[] { typeof(string) });
                            var returnType = method.Invoke(callEvent.Script, new System.Object[] { m_clickMsg.MsgName });

                            m_messenger.SendMsg(m_clickMsg.MsgName, gameObject, (UIEventArgs)returnType);
                        }
                        else
                        {
                            m_messenger.SendMsg(m_clickMsg.MsgName, gameObject, new UIEventArgs<string>(m_clickMsg.MsgName, "MethodName Error! No Method [ " + m_clickMsg.callEvent.MethodName + " ] in [" + m_clickMsg.callEvent.Target + "]"));
                        }
                        break;
                }
            }
        }

        #endregion

        #region Inner
        #endregion
    }
}
