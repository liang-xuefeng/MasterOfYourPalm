using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeMaster : IModule
{
    private ThreeView m_ThreeView;

    

    public override void InitModule()
    {
        
    }

    public override void StartModule(int param, object data = null)
    {
        if (m_ThreeView == null)
        {
            m_ThreeView = UILoader.LoadUI<ThreeView>("UI/ThreeView");
        }
        m_ThreeView.Init();
        m_ThreeView.m_Recognized = (byte[])data;

        m_ThreeView.m_StartPrint = StartPrint;
        UI.Instance.addChild(m_ThreeView);
    }

    private void StartPrint(string path)
    {
        m_ThreeView.m_StartPrint = null;

        ModuleMaster.Instance.GetModule<FourMaster>().StartModule(0, path);
    }
}
