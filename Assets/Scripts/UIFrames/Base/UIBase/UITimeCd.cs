using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UITimeCd : MonoBehaviour
{
    public Sprite[] num_array = null;

    public delegate void CdCom();
    public event CdCom m_cdComEvent;
    bool isStart = false;

    float m_cdTime = 0.0f;
    Image m_Image = null;
    void Start()
    {
        m_Image = gameObject.GetComponent<Image>();
    }
    public void startCd(float cdTime)
    {
        m_cdTime = cdTime;
        isStart = true;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (isStart)
        {
            m_cdTime -= Time.deltaTime;
            int num = (int)m_cdTime;
            if (num == -1)
            {
                isStart = false;

                if (m_cdComEvent != null)
                    m_cdComEvent();
                gameObject.SetActive(false);
            }
            else
            {
                if (num < 10 && num >= 0)
                    m_Image.sprite = num_array[num];
            }
        }
    }


}
