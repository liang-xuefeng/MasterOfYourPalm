using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ModuleMaster
{

    public static ModuleMaster Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new ModuleMaster();
                m_Instance.Init();
            }
            return m_Instance;
        }
    }

    private static ModuleMaster m_Instance;
    private bool isInit = false;
    private bool isRegisteredModule = false;

    /// <summary> 模块管理(模块类型索引查找字典) </summary>
    private Dictionary<Type, IModule> m_ModuleDic_type = new Dictionary<Type, IModule>();
    /// <summary> 模块管理(模块ID索引查找字典) </summary>
    private Dictionary<int, IModule> m_ModuleDic_index = new Dictionary<int, IModule>();
    private int LoadAssetsNum = 0;

    public void Clear()
    {
        isInit = false;
        var scales = m_ModuleDic_type.GetEnumerator();
        while (scales.MoveNext())
        {
            IModule module = scales.Current.Value;
            module.isModuleInit = false;
            module.ClearData();
        }
    }

    /// <summary> 初始化所有模块 </summary>
    public void InitModule()
    {
        Debug.Log("-------------------------<初始化所有模块>--------------------------");
        if (isInit)
            return;
        isInit = true;
        if (m_ModuleDic_type.Count > 0)
        {
            var scales = m_ModuleDic_type.GetEnumerator();
            while (scales.MoveNext())
            {
                IModule module = scales.Current.Value;
                if (!module.isModuleInit)
                    module.InitModule();
            }
        }
    }

    /// <summary> 根据索引获取一个模块 </summary>
    public IModule GetModule(int index)
    {
        if (m_ModuleDic_index.ContainsKey(index))
            return m_ModuleDic_index[index];
        return null;
    }



    /// <summary> 根据类型获取一个模块 </summary>
    public T GetModule<T>() where T : IModule
    {
        Type type = typeof(T);
        if (m_ModuleDic_type.ContainsKey(type))
            return m_ModuleDic_type[type] as T;
        return null;
    }

    /// <summary> 判断一个模块是否存在 </summary>
    public bool Exist<T>()
    {
        return m_ModuleDic_type.ContainsKey(typeof(T));
    }

    private void Init()
    {
        if (isRegisteredModule)
        {
            return;
        }
        isRegisteredModule = true;
        RegisteredModule();
    }

    /// <summary> 添加一个模块 </summary>
    private void AddModule(IModule module, int moduleIndex = 0)
    {
        m_ModuleDic_type.Add(module.GetType(), module);
        m_ModuleDic_index.Add(moduleIndex, module);
    }

    //============注册各个模块写这里(自己模块写在自己名字后)==============
    private void RegisteredModule()
    {
        Debug.Log("注册模块");
        AddModule(new OneMaster(), 1);
        AddModule(new TwoMaster(), 2);
        AddModule(new ThreeMaster(), 3);
        AddModule(new FourMaster(), 4);
        AddModule(new FiveMaster(), 5);
        AddModule(new DarwHandMaster(),6);
    }
}