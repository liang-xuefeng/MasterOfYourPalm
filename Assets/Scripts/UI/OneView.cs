using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneView : UIPanel
{
    public Image m_Hand;
    public Image m_Car;

    /// <summary> 车辆名称 与 车图片 </summary>
    private Dictionary<string, string> m_IamgePath;

    /// <summary> 表信息 </summary>
    private List<CarElement> m_CarElement;

    private void Start()
    {
        m_CarElement = GameTable.GetTable<CarTable>().GetAllElement();

        HandMove();
        GetIamge();
    }

    /// <summary>
    /// 手移动
    /// </summary>
    private void HandMove()
    {
        iTween.ValueTo(m_Hand.gameObject,
            iTween.Hash(
            "from", new Vector3(0,-395,0),
            "to", new Vector3(0,-336,0),
            "time", 0.7f,
            "easeType", iTween.EaseType.linear,
            "loopType", iTween.LoopType.pingPong,
            "onupdate", (System.Action<object>)delegate(object nowValue)
              {
                  m_Hand.gameObject.transform.localPosition = (Vector3)nowValue;
              }
            ));
    }

    /// <summary>
    /// 设置车的图片
    /// </summary>
    void GetIamge()
    {
        float time = 2f;
        int index = 0;

        iTween.ValueTo(m_Car.gameObject,
            iTween.Hash(
            "from", 1,
            "to", 0.2f,
            "time", time,
            "easeType", iTween.EaseType.linear,
            "loopType", iTween.LoopType.loop,
            "onupdate", (System.Action<object>)delegate (object nowValue)
            {
                m_Car.color = new Color(1,1,1, (float)nowValue);
            },
            "oncomplete",(System.Action) delegate
            {
                m_Car.overrideSprite = Resources.Load("Cars/" + m_CarElement[index].SpriteName, typeof(Sprite)) as Sprite;
                index++;
                if (index >= m_CarElement.Count) index = 0;
            } 
            ));
    }
}
