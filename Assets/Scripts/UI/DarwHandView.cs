using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GeeVision.Core;
using UnityEngine;
using UnityEngine.UI;

public class DarwHandView : UIPanel
{
    public Image m_Image;
    public Texture2D m_Texture2D;
    public Image m_Error;

    public byte[] Recognized { get { return m_Recognized; } }

    private byte[] m_Recognized = new byte[224 * 171];
    private int index;

    /// <summary>
    /// 有手掌数据
    /// </summary>
    public HaveHandEvent m_HaveHandEvent;

    void Start()
    {
        m_Image.sprite = Sprite.Create(m_Texture2D, new Rect(0, 0, m_Texture2D.width, m_Texture2D.height), Vector2.one);
        m_Error.enabled = false;
        GameEvent.Instance.SetIsError = (isError) => { m_Error.enabled = isError; };
    }

    void OnEnable()
    {
        SetIamge(false);
    }


    void FixedUpdate()
    {
        m_Recognized = GeeVisionListener.Recognized;

        index = 0;
        GameEvent.Instance.Count = 0;

        for (int i = 0; i < 171; i++)
        {
            for (int j = 0; j < 224; j++)
            {
                m_Texture2D.SetPixel(i, j, m_Recognized[index] == 0 ? Color.black : Color.white);
                if (m_Recognized[index] != 0) GameEvent.Instance.Count++;
                index++;
            }
        }

        IsStart();

        m_Texture2D.Apply();
    }

    public void SetIamge(bool isActive)
    {
        m_Image.gameObject.SetActive(isActive);
    }

    public void IsStart()
    {
        //UnityEngine.Debug.Log((BaseHandPose)GeeVisionListener.Instance.HandStatus);
        //UnityEngine.Debug.Log(GameEvent.Instance.Count + "GameEvent.Instance.Count");

        if (GameEvent.Instance.Count > 800 && m_HaveHandEvent != null)
            m_HaveHandEvent();
    } 
}
