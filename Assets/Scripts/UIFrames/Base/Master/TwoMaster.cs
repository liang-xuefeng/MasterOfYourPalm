using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoMaster : IModule
{
    private TwoView m_TwoView;
    private DarwHandView m_DarwHandView;

    private byte[] m_Recognized = new byte[224 * 171];

    public override void InitModule()
    {
        
    }

    public override void StartModule(int param, object data = null)
    {
        if (m_TwoView == null)
        {
            m_TwoView = UILoader.LoadUI<TwoView>("UI/TwoView");
        }
        UI.Instance.addChild(m_TwoView);

        if (m_DarwHandView == null)
        {
            m_DarwHandView = UI.Instance.FindView<DarwHandView>();
        }
        m_DarwHandView.SetIamge(true);

        m_TwoView.m_HandLeaveEvent = HandLeaveEvent;
        m_TwoView.m_StartTimer = StartTimer;
    }

    private void StartTimer()
    {
        m_TwoView.m_StartTimer = null;
        m_TwoView.m_HandLeaveEvent = null;
        m_Recognized = m_DarwHandView.Recognized;

        UI.Instance.removeChild(m_TwoView);
        UI.Instance.removeChild(m_DarwHandView);

        ModuleMaster.Instance.GetModule<ThreeMaster>().StartModule(0, m_Recognized);
    }

    private void HandLeaveEvent()
    {
        m_TwoView.m_StartTimer = null;
        m_TwoView.m_HandLeaveEvent = null;
        UI.Instance.removeChild(m_TwoView);
        ModuleMaster.Instance.GetModule<OneMaster>().StartModule(0);
    }
}
