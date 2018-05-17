using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用手掌进入
/// </summary>
public delegate void HaveHandEvent();

/// <summary>
/// 手掌离开
/// </summary>
public delegate void HandLeaveEvent();

/// <summary>
/// 开始记时
/// </summary>
public delegate void StartTimer();

/// <summary>
/// 开始打印
/// </summary>
public delegate void StartPrint(string path);

/// <summary>
/// 打印完成
/// </summary>
public delegate void Accomplish();

/// <summary>
/// 返回主界面
/// </summary>
public delegate void BackHome();

/// <summary>
/// 是否显视提示
/// </summary>
/// <param name="isActive"></param>
public delegate void SetLoading(bool isActive);

/// <summary>
/// 是否错误
/// </summary>
/// <param name="isError"></param>
public delegate void IsError(bool isError);


public class GameEvent
{
    private static GameEvent _instance;

    public static GameEvent Instance
    {
        get { return _instance ?? (_instance = new GameEvent()); }
    }

    private GameEvent() { }

    /// <summary> 是否后门 </summary>
    public bool IsBackDoor = false;

    /// <summary> 手掌数据 </summary>
    public int Count;

    public IsError SetIsError;
}
