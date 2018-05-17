#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Titan.UI;

public class UIPanel : Panel
{
    [HideInInspector]
    public string name = "noName";

    private Dictionary<string, List<Action<DataEvent>>> listenerList = new Dictionary<string, List<Action<DataEvent>>>();

    public void addEventListener(string type, Action<DataEvent> listener)
    {
        if (listenerList.ContainsKey(type) == false)
        {
            listenerList.Add(type, new List<Action<DataEvent>>());
        }
        if (listenerList[type].Contains(listener) == false)
        {
            listenerList[type].Add(listener);
        }
    }

    public void removeEventListener(string type, Action<DataEvent> listener)
    {
        if (listenerList.ContainsKey(type) == false)
        {
            return;
        }
        if (onDispatchingKey == type)
        {//正在派发这个事件
            remListeners.Add(listener);
        }
        else
        {
            listenerList[type].Remove(listener);
            if (listenerList[type].Count == 0)
            {
                listenerList.Remove(type);
            }
        }
    }

    public bool hasEventListener(string type)
    {
        return listenerList.ContainsKey(type);
    }

    private string onDispatchingKey = null;//当前正在派发的事件类型
    private List<Action<DataEvent>> remListeners = new List<Action<DataEvent>>();

    public bool dispatchEvent(DataEvent e)
    {
        string key = e.type;
        if (listenerList.ContainsKey(key) == false)
        {
            return false;
        }
        else
        {
            onDispatchingKey = key;
            e.target = this;
            foreach (Action<DataEvent> listener in listenerList[key])
            {
                listener(e);
            }
            onDispatchingKey = null;
            for (int i = 0; i < remListeners.Count; i++)
            {//移除派发过程中移除的事件
                removeEventListener(key, remListeners[i]);
            }
            remListeners.Clear();
            return true;
        }
    }

    public Vector2 Position
    {
        get
        {
            return this.GetComponent<RectTransform>().anchoredPosition;
        }
        set
        {
            this.GetComponent<RectTransform>().anchoredPosition = value;
        }
    }

    public Vector2 Size
    {
        get
        {
            return this.GetComponent<RectTransform>().sizeDelta;
        }
    }

    /// <summary>
    /// 是否可见 <see cref="UIPanel"/> is visible.
    /// </summary>
    /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
    public bool visible
    {
        get
        {
            return this.gameObject.activeSelf;
        }
        set
        {
            this.gameObject.SetActive(value);
        }
    }
    public bool isInUI
    {
        get
        {
            return UI.Instance.PanelIsInUI(this);
        }
    }


    public void OnDestroy()
    {
        //if(this.gameObject != null)
        //BundleHelper.Instance.RemoveAtlasReource(this.gameObject);
    }

}
