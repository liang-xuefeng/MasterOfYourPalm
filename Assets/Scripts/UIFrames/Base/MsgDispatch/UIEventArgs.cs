using UnityEngine;
using System.Collections;
using System;

namespace Titan.UI
{
    public interface IMsgClick
    {
        void onClick(object sender, System.EventArgs e);
    }

    public interface IReciver : IMsgClick
    {

    }

    public interface IMsgRegister
    {
        void RegisterMsg(string msgType, object sender, EventHandler handler);
        void UnRegisterMsg(string msgType, object sender, EventHandler handler);
        void SendMsg(string msgType,object sender,EventArgs eventArgs);
    }

    public class UIEventArgs : EventArgs
    {
        public readonly string MsgType;

        public UIEventArgs(string msgType)
        {
            MsgType = msgType;
        }
        
        #region Static Arg1 
        private static T ArgToType<T>(UIEventArgs e, T defaultArg)
        {
            if (e is UIEventArgs<T>)
            {
                return ((UIEventArgs<T>)e).Arg1;
            }
            else
            {
                Debug.Log("<color=red> UIEvent try to conver " + e.GetType() + " to " + typeof(UIEventArgs<T>)+". Maybe the EventArgs Func is Wrong</color>");
                return defaultArg;
            }
        }

        public static int ArgToInt(UIEventArgs e,int defaultNum = -1)
        {
            return ArgToType(e, defaultNum);
        }

        public static float ArgToFloat(UIEventArgs e, float defaultNum = -1.0f)
        {
            return ArgToType(e, defaultNum);
        }

        public static string ArgToString(UIEventArgs e, string defaultString = "Default Str")
        {
            return ArgToType(e, defaultString);
        }

        #endregion


        #region Static Arg2
        public static T Arg1ToType<T, K>(UIEventArgs e, T defaultArg)
        {
            if (e is UIEventArgs<T, K>)
            {
                return ((UIEventArgs<T, K>)e).Arg1;
            }
            else
            {
                Debug.Log("<color=red> UIEvent try to conver " + e.GetType() + " to " + typeof(UIEventArgs<T,K>)+". Maybe the EventArgs Func is Wrong</color>");
                return defaultArg;
            }
        }

        public static K Arg2ToType<T,K>(UIEventArgs e, K defaultArg)
        {
            if (e is UIEventArgs<T,K>)
            {
                return ((UIEventArgs<T,K>)e).Arg2;
            }
            else
            {
                return defaultArg;
            }
        }
        #endregion

    }

    public class UIEventArgs<T> : UIEventArgs
    {
        public readonly T Arg1;
        public UIEventArgs(string msgType, T arg)
            : base(msgType)
        {
            Arg1 = arg;
        }
    }

    public class UIEventArgs<T,K> : UIEventArgs
    {
        public readonly T Arg1;
        public readonly K Arg2;
        public UIEventArgs(string msgType, T arg1, K arg2)
            : base(msgType)
        {
            Arg1 = arg1;
            Arg2 = arg2;
        }
    }
}
