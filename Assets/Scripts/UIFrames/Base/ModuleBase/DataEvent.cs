using UnityEngine;
using System.Collections;
public class DataEvent : EventBase
{
    public static readonly string INIT = "init";
    public static readonly string COMPLETE = "complete";
    public static readonly string ERROR = "error";
    public static readonly string CLOSE = "close";
    public static readonly string OPEN = "open";
    public static readonly string EXIT = "exit";
    public static readonly string OK = "OK";
    public static readonly string YES = "yes";
    public static readonly string NO = "no";
    public static readonly string CANCEL = "cancel";
    public static readonly string ADD_TO_UI = "addToUI";
    public static readonly string REM_FROM_UI = "remFromUI";
    public static readonly string CLICK = "click";
    public static readonly string REFRESHUI = "refreshUI";

    public object data;
    public object data2;
    public DataEvent(string type, object data = null, object data2 = null)
        : base(type)
    {
        this.data = data;
        this.data2 = data2;
    }
}

