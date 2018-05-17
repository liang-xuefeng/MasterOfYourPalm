using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GeeVision.Core;

public class Gestures : MonoBehaviour
{
    public Text staticGestureType;
    public Text realTimeGestureType;
    public Text handStatus;
    public Text textOpenType;

    private GeeVisionListener listener;

    private void Start()
    {
        listener = GeeVisionListener.Instance;
    }
    private void Update()
    {
        staticGestureType.text = listener.StaticGestureType.ToString();
        realTimeGestureType.text = listener.RealTimeGestureType.ToString();
        handStatus.text = listener.HandStatus.ToString();

        ReturnHome();
        EnterFun();
        TimerFun((int)listener.RealTimeGestureType);
    }

    #region 手掌保持2秒后握拳确认
    public enum TestOpenType
    {
        Open_Hold,
        Open_Open,
        Open_None
    }
    TestOpenType openType;
    float realTime = 0;
    int i = 0;
    bool isHold = false;
    private void TestHoldTypeFun()
    {
        if (listener.HandStatus == 5)
        {
            realTime += Time.deltaTime;
        }
        else realTime = 0;
        if (realTime > 0.5f)
        {
            openType = TestOpenType.Open_Open;
            realTime = 0;
        }        
    }
    /// <summary>
    /// 确认方法
    /// </summary>
    /// <returns></returns>
    private bool EnterFun()
    {
        TestHoldTypeFun();
        if (listener.HandStatus == 0 && openType == TestOpenType.Open_Open)
        {
            textOpenType.text = " 确认  " + (++i).ToString() + "  次 ";
            openType = TestOpenType.Open_Hold;
            realTime = 0;
            return true;
        }
        else return false;
    }
#endregion

    #region 手掌持续4秒返回
    float time = 0; float times = 0;
    /// <summary>
    /// 返回主界面
    /// </summary>
    /// <returns></returns>
    public bool ReturnHome()
    {
        if (listener.HandStatus == 5)
        {
            times += Time.deltaTime;
            time = 0;
            if (times > 4)
            {
                GoHome();
                return true;
            }
        }
        else if ((time += Time.deltaTime) > 0.5f)
        {
            times = 0;
        }
        return false;
    }
    public void GoHome()
    {
        print("GoHome");
        times = 0;
    }
    #endregion

    #region 顺逆时针旋转
    private float rotationTime = 0;
    /// <summary>
    /// 顺逆时针延时检测方法
    /// </summary>
    /// <param name="type"></param>
    public void TimerFun(int type)
    {

        if (type == 503 || type == 603)
        {

            rotationTime = 0;
            //避免误触发返回HOME
            times = 0;
        }
        //开启计时
        rotationTime += Time.deltaTime;
        if (rotationTime < 0.5f)
        {
            //程序运行
            RealizeRotationResult(type);
            
        }        
    }
    /// <summary>
    /// 实现旋转效果
    /// </summary>
    /// <param name="type"></param>
    private void RealizeRotationResult(float type)
    {
        if (type == 503 || type == 603)
        {
            
            //audio.volume += (type == 503 ? 0.02f : -0.02f);

        }
    }
#endregion
}
