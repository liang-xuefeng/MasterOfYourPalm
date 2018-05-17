using System.Collections;
using System.Collections.Generic;
using GeeVision.Core;
using UnityEngine;
using UnityEngine.UI;

public class TwoView : UIPanel
{
    public Image m_Round2;
    public Image m_Finger;

    public AudioSource m_AudioSource;
    public AudioClip[] m_Clips;

    private float m_Timer;
    private float m_Time;

    public StartTimer m_StartTimer;

    /// <summary>
    /// 手掌离开
    /// </summary>
    public HandLeaveEvent m_HandLeaveEvent;

    void OnEnable()
    {
        m_Timer = 0;
        m_Time = 5;
        m_Round2.fillAmount = 0;
    }

    void Update()
    {
        if (GameEvent.Instance.Count > 500 && (BaseHandPose)GeeVisionListener.Instance.HandStatus == BaseHandPose.Finger5
            && (BaseHandPose)GeeVisionListener.Instance.HandStatus != BaseHandPose.Finger4)
        {
            if (GameEvent.Instance.SetIsError != null) GameEvent.Instance.SetIsError(false);

            SetFill();

            PlaySound(0);

            m_Timer += Time.deltaTime;

            if (!(m_Timer > m_Time)) return;
            m_Timer = 0;
            if (m_StartTimer != null)
            {
                m_StartTimer();
            }
        }
        else if (GameEvent.Instance.Count > 500 && (BaseHandPose) GeeVisionListener.Instance.HandStatus != BaseHandPose.Finger5)
        {
            if (GameEvent.Instance.SetIsError != null) GameEvent.Instance.SetIsError(true);

            SetFill();

            PlaySound(1);

            m_Timer -= Time.deltaTime;

            if (m_Timer <= 0)
            {
                m_Timer = 0;
            }
        }
        else
        {
            if (GameEvent.Instance.SetIsError != null) GameEvent.Instance.SetIsError(true);

            SetFill();

            PlaySound(1);

            m_Timer -= Time.deltaTime;

            if (m_Timer <= 0)
            {
                m_Timer = 0;
                if (m_HandLeaveEvent != null && GameEvent.Instance.Count < 500)
                {
                    if (GameEvent.Instance.SetIsError != null) GameEvent.Instance.SetIsError(false);
                    m_HandLeaveEvent();
                }
            }
        }
    }   

    private void SetFill()
    {
        m_Round2.fillAmount = m_Timer/m_Time;
    }

    private void PlaySound(int index)
    {
        if (m_AudioSource.clip != m_Clips[index])
        {
            m_AudioSource.Stop();

            m_AudioSource.clip = m_Clips[index];

            //if(index == 0)
            //    m_AudioSource.time = m_Timer;
            //else
            //    m_AudioSource.time = m_Time - m_Timer;
            
            m_AudioSource.Play();
        }
    }
}
