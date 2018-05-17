using UnityEngine;
using System.Collections;

public class EventBase
{
    public object target;
    public string type;
    public EventBase(string type)
    {
        this.type = type;
    }
}
