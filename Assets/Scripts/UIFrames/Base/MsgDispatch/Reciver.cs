using UnityEngine;
using System.Collections;
using System;

namespace Titan.UI
{
    public class Reciver : MonoBehaviour, IReciver
    {
        public static bool s_uiMsgLog = false;

        public virtual void onClick(object sender, EventArgs e)
        {
            if(s_uiMsgLog){
                UIEventArgs uiEvent = e as UIEventArgs;
                Debug.Log("Recv UI Message [" + uiEvent.MsgType + "]");
            }
        }
    }
}
