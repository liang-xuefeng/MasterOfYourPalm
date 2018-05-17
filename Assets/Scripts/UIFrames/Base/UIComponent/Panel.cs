using UnityEngine;
using System.Collections;
using System;

namespace Titan.UI
{
    public class Panel : Messenger
    {
        #region Public Attributes

        /// <summary>
        /// you should use += or -= to operate this value
        /// </summary>
        public event EventHandler ClickHandler
        {
            add
            {
                //if (m_clickHandler == null && m_reciver != null)
                if (value != null)
                {
                    if (value.Target is Reciver)
                    {
                        m_reciver = (Reciver)value.Target;
                    }
                    m_clickHandler -= value;
                    m_clickHandler += value;
                    updateChildrenBtnMessenger();
                }
                //return m_clickHandler;
            }
            remove
            {
                if (value != null)
                {
                    if (value.Target is Reciver)
                    {
                        m_reciver = (Reciver)value.Target;
                    }
                    m_clickHandler -= value;
                    updateChildrenBtnMessenger();
                }
            }
        }
        #endregion

        #region Private Attributes
        [SerializeField]
        //Set value from editor or form handler
        private Reciver m_reciver;
        [SerializeField]
        private event EventHandler m_clickHandler;
        #endregion

        #region Unity Messages

        void Awake()
        {
            //Make attached reciver register Click Handler
            if (m_reciver != null)
            {
                ClickHandler += m_reciver.onClick;
            }
        }

        //void OnEnable()
        //{
        //}
        //
        //void Start()
        //{

        //}
        //	
        //	void Update() 
        //	{
        //	
        //	}
        //
        //	void OnDisable()
        //	{
        //
        //	}
        //
        #endregion

        #region Public Methods
        public bool isReciverAvaliable()
        {
            //Reciver serves for Handler 
            //So Handler Avaliable will be fine
            if (IsClickHandlerAvaliable()) 
            {
                return true;
            }
            else
            {
                return m_reciver != null;
            }
        }

        public bool IsClickHandlerAvaliable()
        {
            return m_clickHandler != null;
        }

        public void RegisterClickMsg(string msgType, object sender)
        {
            base.RegisterMsg(msgType, sender, m_clickHandler);
        }

        public void UnRegisterClickMsg(string msgType, object sender)
        {
            base.UnRegisterMsg(msgType, sender, m_clickHandler);
        }


        #endregion

        #region Override Methods
        public override string ToString()
        {
            return base.ToString();
        }

        #endregion

        #region Private Methods
        protected void updateChildrenBtnMessenger()
        {
            var list = GetComponentsInChildren<ButtonT>();
            for (int i = 0; i < list.Length; ++i)
            {
                list[i].SetMessenger(this);
            }
        }
        #endregion

        #region Inner

        #endregion
    }
}
