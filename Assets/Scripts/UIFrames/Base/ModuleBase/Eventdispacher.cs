using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Eventdispacher
{
    private Dictionary<string, List<Action<DataEvent>>> listenerList = new Dictionary<string, List<Action<DataEvent>>>();

    public void addEventListener(string type, Action<DataEvent> listener)
    {
        if (listenerList.ContainsKey(type) == false)
        {
            listenerList.Add(type, new List<Action<DataEvent>>());
        }
        if (listenerList[type].IndexOf(listener) == -1)
            listenerList[type].Add(listener);
    }

    public bool dispatchEvent(DataEvent e)
    {
        string key = e.type;
        if (listenerList.ContainsKey(key) == false)
        {
            return false;
        }
        else
        {
            e.target = this;
            foreach (Action<DataEvent> listener in listenerList[key])
            {
                listener(e);
            }
            return true;
        }
    }
}
