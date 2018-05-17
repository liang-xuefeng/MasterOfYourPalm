using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;

namespace Titan.UI
{
    public enum MsgSendType
    {
        None,
        ParentPanel,
        //MsgCenter, //Wait to develop
    };

    [Serializable]
    public class CallEvent
    {
        public UnityEngine.Object Target;
        public UnityEngine.Object Script;
        public string MethodName = null;
    }

    [Serializable]
    public class MsgOption
    { 
        public enum ConstDataType //If want to extend,pay attention to the index in MsgOptionPropertyDrawer
        {
            Int,
            Float,
            String,
            Customer,
            None,
        }

        public MsgSendType MsgSendType = MsgSendType.None;
        public string MsgName = "MsgName@ModuleName";
        public ConstDataType DataType = ConstDataType.None;

        public int intData;
        public float floatData;
        public string stringData;
        public CallEvent callEvent;

        public MsgOption()
        {

        }
    }

   
}
