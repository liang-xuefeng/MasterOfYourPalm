using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveMaster : IModule
{
    private FiveView m_FiveView;
    private ThreeView m_ThreeView;

    public override void InitModule()
    {
        
    }

    public override void StartModule(int param, object data = null)
    {
        if (m_FiveView == null)
        {
            m_FiveView = UILoader.LoadUI<FiveView>("UI/FiveView");
        }
        m_FiveView.Init();
        m_FiveView.m_BackHome = BackHome;
        m_ThreeView = UI.Instance.FindView<ThreeView>();

        UI.Instance.addChild(m_FiveView);
    }

    private void BackHome()
    {
        m_FiveView.m_BackHome = null;
        UI.Instance.removeChild(m_FiveView);
        UI.Instance.removeChild(m_ThreeView);
        ModuleMaster.Instance.GetModule<OneMaster>().StartModule(0);
    }
}
