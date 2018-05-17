using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveView : UIPanel
{
    private float timer;
    private float time;
    private  bool IsStart;

    public BackHome m_BackHome;

    void Update()
    {
        if (!IsStart) return;

        timer += Time.deltaTime;
        if (timer >= time)
        {
            IsStart = false;
            if (m_BackHome != null) m_BackHome();
        }
    }

    public void Init()
    {
        timer = 0;
        time = 3f;
        IsStart = true;
    }
	
}
