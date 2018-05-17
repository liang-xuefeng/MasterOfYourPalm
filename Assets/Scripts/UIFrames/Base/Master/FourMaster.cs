using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourMaster : IModule
{
    private FourView m_FourView;

    public override void InitModule()
    {
        
    }

    public override void StartModule(int param, object data = null)
    {
        if (m_FourView == null)
        {
            m_FourView = UILoader.LoadUI<FourView>("UI/FourView");

        }
        m_FourView.Init((string)data);
        m_FourView.m_Accomplish = Accomplish;
        UI.Instance.addChild(m_FourView);
    }

    private void Accomplish()
    {
        m_FourView.m_Accomplish = null;
        UI.Instance.removeChild(m_FourView);
        ModuleMaster.Instance.GetModule<FiveMaster>().StartModule(0);
    }
}
