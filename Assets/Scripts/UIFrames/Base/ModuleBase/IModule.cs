using UnityEngine;
using System;
using Titan.UI;


public abstract class IModule : IReciver
{
    public event Action<IModule> OnLoadAssetsComplete;
    public bool isModuleInit = false;
    public abstract void InitModule();//初始化模块
    public abstract void StartModule(int param, object data = null);//启动模块

    public void Trace(string str)
    {
        Debug.Log("<color=#00ff00ff>" + str + "</color>");
    }

    /// <summary> 预加载资源 </summary>
    public virtual void LoadAssets()
    {
        LoadAssetsComplete();
    }

    /// <summary> 活动次数进度 </summary>
    public virtual string GetProgress()
    {
        return "";
    }

    public void LoadAssetsComplete()
    {
        if (OnLoadAssetsComplete != null)
            OnLoadAssetsComplete(this);
    }

    /// <summary> 模块是否可以提升，返回模块内是否可以提升 </summary>
    public virtual bool ModuleAscensionValue()
    {
        return false;
    }

    /// <summary> 模拟点击水平标签(当一个系统有标签时,需要直接切换到指定标签 add by jc) </summary>
    public virtual void SimulateHTabClick(int tabIndex) { }

    /// <summary> 模拟点击纵向标签(当一个系统有标签时,需要直接切换到指定标签 add by jc) </summary>
    public virtual void SimulateVTabClick(int tabIndex) { }

    /// <summary> 点击 </summary>
    public virtual void onClick(object sender, EventArgs e) { }

    /// <summary> 清除数据 </summary>
    public virtual void ClearData()
    {
        //此函数会在进入游戏(包括重新进入)的时候调用
    }
}
