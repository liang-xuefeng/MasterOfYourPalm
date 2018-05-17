using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneMaster : IModule
{
    private OneView m_OneView;
    private DarwHandView m_DarwHandView;

    public override void InitModule()
    {
        
    }

    public override void StartModule(int param, object data = null)
    {
        if (m_OneView == null)
        {
            m_OneView = UILoader.LoadUI<OneView>("UI/OneView");
        }
        UI.Instance.addChild(m_OneView);

        if (m_DarwHandView == null)
        {
            m_DarwHandView = UILoader.LoadUI<DarwHandView>("UI/DarwHandView");
        }

        m_DarwHandView.m_HaveHandEvent = HaveHandEvent;
        m_DarwHandView.SetIamge(false);
        UI.Instance.addChild(m_DarwHandView, UI.UILayerType.TOP);
    }

    private void HaveHandEvent()
    {
        ModuleMaster.Instance.GetModule<TwoMaster>().StartModule(0);
        UI.Instance.removeChild(m_OneView);
        m_DarwHandView.m_HaveHandEvent = null;
    }
}
