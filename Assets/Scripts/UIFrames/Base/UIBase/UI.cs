using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI : Eventdispacher
{
    public delegate void HUDDMoveChange();
    public event HUDDMoveChange OnHUDDMoveIn;//HUD划入
    public event HUDDMoveChange OnHUDDMoveOut;//HUD划出

    /// <summary> 任务状态类型 </summary>
    public enum UILayerType
    {
        NONE = -1,
        TOP = 0,
        /// <summary> 此层一出 MIDDLE BOTTOM自动隐藏 </summary>
        OVERNERING = 1,
        MIDDLE = 2,
        BOTTOM = 3,
        NOTHING = 4,
        SUNDRY = 5,
        TOPPER = 6
    }

    private static UI m_Instance = null;
    private RectTransform UILayer_top;
    private RectTransform UILayer_oneyMe;
    public RectTransform UILayer_middle;
    private RectTransform UILayer_botttom;
    private RectTransform UILayer_nothing;
    public RectTransform UILayer_sundry;
    private RectTransform UILayer_topper;
    private const float UIOffsetY = 10000f;
    public static UnityEngine.Object rootPrefab;
    private GameObject Root = null;
    public static Canvas UiCanvas;
    private Camera UiCamera;
    public Camera UICamera { get { return UiCamera; } }

    private UI()
    {
        //newbieHelp = new NewbieHelp();
    }

    public static UI Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new UI();
            }
            if (m_Instance.Root == null)
            {
                m_Instance.m_triggerHUDMoveOutPanels.Clear();
                m_Instance.creatUIRoot();
                //m_Instance.newbieHelp.CreatNewbieHelpLayer(m_Instance.Root.transform.position, UiCanvas.worldCamera.depth);//创建新手引导层
                m_Instance.UILayer_sundry = m_Instance.creatUILayer("UILayer_sundry");
                //------add by jc-----该层用于称谓显示---
                m_Instance.UILayer_sundry.gameObject.layer = LayerMask.NameToLayer("Sundry");
                Canvas canvas_sundry = m_Instance.UILayer_sundry.gameObject.AddComponent<Canvas>();
                canvas_sundry.overrideSorting = true;
                canvas_sundry.sortingOrder = -10;

                //---------------------------------------
                m_Instance.UILayer_botttom = m_Instance.creatUILayer("UILayer_botttom");
                m_Instance.UILayer_middle = m_Instance.creatUILayer("UILayer_middle");
                m_Instance.UILayer_oneyMe = m_Instance.creatUILayer("UILayer_oneyMe");
                m_Instance.UILayer_top = m_Instance.creatUILayer("UILayer_top");
                m_Instance.UILayer_topper = m_Instance.creatUILayer("UILayer_topper");
                m_Instance.UILayer_nothing = m_Instance.creatUILayer("UILayer_nothing");

                m_Instance.UILayer_nothing.gameObject.SetActive(false);

                m_Instance.UILayer_top.gameObject.layer = LayerMask.NameToLayer("UI");
                Canvas canvas = m_Instance.UILayer_top.gameObject.AddComponent<Canvas>();
                canvas.overrideSorting = true;
                canvas.sortingOrder = 10;
                m_Instance.UILayer_top.gameObject.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            }
            return m_Instance;
        }
    }

    /// <summary> 获取UI数结构的字典 </summary>
    public Dictionary<string, List<string>> GetUITree()
    {
        List<Transform> layers = new List<Transform>() { UILayer_sundry, UILayer_botttom, UILayer_middle, UILayer_oneyMe, UILayer_top, UILayer_topper, UILayer_nothing };
        Dictionary<string, List<string>> treeString = new Dictionary<string, List<string>>();
        foreach (Transform layer in layers)
        {
            string layerName = layer.gameObject.name;
            treeString.Add(layerName, new List<string>());
            for (int i = 0; i < layer.childCount; i++)
            {
                treeString[layerName].Add(layer.GetChild(i).gameObject.name);
            }
        }
        return treeString;
    }

    /// <summary> 隐藏UI层 </summary>
    public void HideUI(int flag)
    {
        switch (flag)
        {
            case 1:
                UILayer_middle.gameObject.SetActive(false);
                break;
            case 2:
                UILayer_oneyMe.gameObject.SetActive(false);
                break;
            case 3:
                UILayer_top.gameObject.SetActive(false);
                break;
            case 4:
                UILayer_topper.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public RectTransform GetBottomLayer()
    {
        return UILayer_botttom;
    }

    private void creatUIRoot()
    {
        if (Root != null)
            return;
        Root = Resources.Load("FuncPrefabs/UIRoot/UIRoot", typeof(GameObject)) as GameObject;
        Root = GameObject.Instantiate(Root, new Vector3(0f, UIOffsetY, 0f), Quaternion.identity) as GameObject;
        Root.name = "UIRoot";
        UiCamera = Root.transform.GetComponentInChildren<Camera>();
        int screenW = Screen.width;
        if (screenW < 1704)
        {
            //Root.GetComponent<Canvas>().scaleFactor = screenW / 1704f;
        }
        else if (screenW > 1400)
        {
            //Root.GetComponent<Canvas>().scaleFactor = screenW / 1704f;
        }
        if (UI.UiCanvas == null)
        {
            UI.UiCanvas = Root.GetComponent<Canvas>();
        }
        GameObject eventSystem = GameObject.Find("EventSystem");
        if (eventSystem == null)
        {
            GameObject.Instantiate(Resources.Load("FuncPrefabs/UIRoot/EventSystem"));
        }

    }

    private RectTransform creatUILayer(string name)
    {
        RectTransform layerTrf = new GameObject(name).AddComponent<RectTransform>();
        layerTrf.SetParent(m_Instance.Root.transform);
        layerTrf.anchorMax = new Vector2(1, 1);
        layerTrf.anchorMin = new Vector2(0, 0);
        layerTrf.pivot = new Vector2(0.5f, 0.5f);
        layerTrf.sizeDelta = new Vector2(0, 0);
        layerTrf.localScale = Vector3.one;
        layerTrf.anchoredPosition = new Vector2(0, 0);
        return layerTrf;
    }

    private List<UIPanel> m_triggerHUDMoveOutPanels = new List<UIPanel>();

    /// <summary> HUD的划入划出 </summary>
    /// <param name="isIn">true-划入 false-划出</param>
    private void HUDPanleMove(bool isIn)
    {
        if (UILayer_middle.gameObject.activeSelf)
        {
            Animator[] animators = UILayer_middle.GetComponentsInChildren<Animator>();
            SetHUDPanleAnimatorState(animators, isIn);
        }
        if (UILayer_botttom.gameObject.activeSelf)
        {
            Animator[] animators = UILayer_botttom.GetComponentsInChildren<Animator>();
            SetHUDPanleAnimatorState(animators, isIn);
        }
    }

    private void SetHUDPanleAnimatorState(Animator[] animators, bool inStage)
    {
        for (int i = 0; i < animators.Length; i++)
        {
            if (animators[i].gameObject.activeSelf && animators[i].parameterCount > 0 && animators[i].GetParameter(0).name == "InStage")
            {
                animators[i].SetBool("InStage", inStage);
            }
        }
    }

    /// <summary> 判断鼠标是否点击上 </summary>
    public bool MouseIsHit
    {
        get
        {
            //从摄像机的原点向鼠标点击的对象身上设法一条射线
            Ray ray = UiCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 100f);
            RaycastHit hit;
            LayerMask mask = 1 << LayerMask.NameToLayer("UI");
            //当射线彭转到对象时
            bool isHitSomething = Physics.Raycast(ray, out hit, float.PositiveInfinity, mask.value);

            return isHitSomething;
        }
    }

    /// <summary> 添加面板 </summary>
    public GameObject addChild(UIPanel uiPanel, UILayerType layerType = UILayerType.MIDDLE)
    {
        if (uiPanel == null)
            return null;
        bool beforInOnelyMe = uiPanel.transform.parent == UILayer_oneyMe;
        Transform UILayer = null;
        if (layerType == UILayerType.TOP)
        {
            UILayer = UILayer_top;
        }
        else if (layerType == UILayerType.TOPPER)
        {
            UILayer = UILayer_topper;
        }
        else if (layerType == UILayerType.BOTTOM)
        {
            UILayer = UILayer_botttom;
        }
        else if (layerType == UILayerType.MIDDLE)
        {
            UILayer = UILayer_middle;
        }
        else if (layerType == UILayerType.NOTHING)
        {
            UILayer = UILayer_nothing;
        }
        else if (layerType == UILayerType.OVERNERING)
        {
            UILayer = UILayer_oneyMe;
            UILayer_sundry.gameObject.SetActive(false);
            UILayer_botttom.gameObject.SetActive(false);
            UILayer_middle.gameObject.SetActive(false);
        }
        else if (layerType == UILayerType.SUNDRY)
        {
            UILayer = UILayer_sundry;
        }

        uiPanel.transform.SetParent(UILayer);
        uiPanel.gameObject.SetActive(true);
        uiPanel.transform.SetAsLastSibling();

        if (beforInOnelyMe && UILayer_oneyMe.childCount == 0)
        {
            UILayer_sundry.gameObject.SetActive(true);
            UILayer_botttom.gameObject.SetActive(true);
            UILayer_middle.gameObject.SetActive(true);
        }

        if (uiPanel.GetComponent<RectTransform>() != null)
            uiPanel.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
        uiPanel.dispatchEvent(new DataEvent(DataEvent.ADD_TO_UI));
        return uiPanel.gameObject;
    }

    public GameObject addChildAt(UIPanel uiPanel, int index, UILayerType layerType = UILayerType.MIDDLE)
    {
        addChild(uiPanel, layerType);
        if (index < 0)
            index = 0;
        uiPanel.transform.SetSiblingIndex(index);
        return uiPanel.gameObject;
    }

    /// <summary> 友情提示：如果将destroy的默认值改为true，会引发严重的后果 </summary>
    public GameObject removeChild(UIPanel uiPanel, bool destroy = false)
    {
        if (uiPanel == null || PanelIsInUI(uiPanel) == false)
        {
            return null;
        }
        if (uiPanel.transform.parent == UILayer_oneyMe && UILayer_oneyMe.childCount == 1)
        {
            UILayer_sundry.gameObject.SetActive(true);
            UILayer_botttom.gameObject.SetActive(true);
            UILayer_middle.gameObject.SetActive(true);
            if (m_triggerHUDMoveOutPanels.Count > 0)
            {
                HUDPanleMove(false);
            }
        }
        uiPanel.transform.SetParent(UILayer_nothing);
        uiPanel.dispatchEvent(new DataEvent(DataEvent.REM_FROM_UI));
        if (destroy)
        {
            GameObject.Destroy(uiPanel.gameObject);
            return null;
        }
        else
        {
            return uiPanel.gameObject;
        }
    }

    /// <summary>移除一个图层所有的面板</summary>
    /// <param name="layerType">层级类型</param>
    /// <param name="ignoreChildNames">忽略UI名称列表</param>
    /// <param name="destroy">是否直接删除</param>
    public void remAllChild(UILayerType layerType, List<string> ignoreChildNames = null, bool destroy = false)
    {
        Transform UILayer = null;
        if (layerType == UILayerType.TOP)
            UILayer = UILayer_top;
        else if (layerType == UILayerType.TOPPER)
            UILayer = UILayer_topper;
        else if (layerType == UILayerType.BOTTOM)
            UILayer = UILayer_botttom;
        else if (layerType == UILayerType.MIDDLE)
            UILayer = UILayer_middle;
        else if (layerType == UILayerType.OVERNERING)
            UILayer = UILayer_oneyMe;
        for (int i = 0; i < UILayer.transform.childCount; )
        {
            if (ignoreChildNames == null || ignoreChildNames.Contains(UILayer.transform.GetChild(i).gameObject.name) == false)
            {
                GameObject uiGo = removeChild(UILayer.transform.GetChild(i).GetComponent<UIPanel>(), destroy);
                if (uiGo == null)
                {
                    UILayer.transform.GetChild(i).SetParent(null);
                    GameObject.Destroy(uiGo);
                }
            }
            else
            {
                i++;
            }
        }
    }

    /// <summary> 删除所有面板 </summary>
    public void remAllChild()
    {
        //List<string> ignoreChildNames = new List<string>(){"HUDBottomView","HUDRightBottomView","HUDTopView","HUDFuncOpenPro","HUDLeftMiddleView","HUDLeftTopView","Chat_SmallView",
        //"MiniMapMainCityMainView","MiniMapMainViewParent","TaskFindPathView","Debug_LefBottomView", "Tips_NumChange"};
        remAllChild(UILayerType.TOP);
        remAllChild(UILayerType.TOPPER);
        remAllChild(UILayerType.BOTTOM);
        remAllChild(UILayerType.MIDDLE);
        remAllChild(UILayerType.OVERNERING);
    }

    /// <summary> 播放特效 </summary>
    public void playEffect(string effectPath, Vector2 pos)
    {
        GameObject effect = UILoader.LoadUIEffect(effectPath);
        Transform UILayer = UILayer_middle;
        effect.transform.SetParent(UILayer);
        effect.transform.localScale = Vector3.one;
        effect.transform.localPosition = new Vector3(pos.x, pos.y, 0);
    }

    /// <summary> 设置UI的可见性 </summary>
    public void setUIVisible(UILayerType uiLayerType, bool value)
    {
        if (uiLayerType == UILayerType.TOP)
        {
            UILayer_top.gameObject.SetActive(value);
        }
        else if (uiLayerType == UILayerType.TOPPER)
        {
            UILayer_topper.gameObject.SetActive(value);
        }
        else if (uiLayerType == UILayerType.MIDDLE)
        {
            UILayer_middle.gameObject.SetActive(value);
        }
        else if (uiLayerType == UILayerType.BOTTOM)
        {
            UILayer_botttom.gameObject.SetActive(value);
        }
        else if (uiLayerType == UILayerType.SUNDRY)
        {
            UILayer_sundry.gameObject.SetActive(value);
        }
    }

    /// <summary> 画布宽度 </summary>
    public static float stageWidth
    {
        get
        {
            return Instance.Root.GetComponent<RectTransform>().sizeDelta.x;
            //return Instance.UILayer_middle.GetComponent<RectTransform>().sizeDelta.x;
        }
    }

    /// <summary> 画布高度 </summary>
    public static float stageHeight
    {
        get
        {
            return Instance.Root.GetComponent<RectTransform>().sizeDelta.y;
            //return Instance.UILayer_middle.GetComponent<RectTransform>().sizeDelta.y;
        }
    }

    public GameObject GetRoot()
    {
        return Root;
    }

    public T FindView<T>() where T : UIPanel
    {
        T inst = Root.GetComponent<T>();
        if (inst == null)
        {
            inst = Root.GetComponentInChildren<T>();
        }
        return inst;
    }

    /// <summary> 判断当前panel是否处于显示状态，即不在nothing层 </summary>
    public bool PanelIsInUI(UIPanel uiPanel)
    {
        if (uiPanel == null)
        {
            return false;
        }
        else
        {
            return uiPanel.transform.parent == this.UILayer_middle ||
                uiPanel.transform.parent == this.UILayer_botttom ||
                uiPanel.transform.parent == this.UILayer_sundry ||
                uiPanel.transform.parent == this.UILayer_top ||
                uiPanel.transform.parent == this.UILayer_topper ||
                uiPanel.transform.parent == this.UILayer_oneyMe ||
                uiPanel.transform.parent == null;
        }
    }

    /// <summary> 根据名字获取界面 </summary>
    public RectTransform GetPanel(string panelName)
    {
        Transform tf = UILayer_middle.Find(panelName);
        if (tf != null)
            return tf as RectTransform;
        tf = UILayer_botttom.Find(panelName);
        if (tf != null)
            return tf as RectTransform;
        tf = UILayer_top.Find(panelName);
        if (tf != null)
            return tf as RectTransform;
        tf = UILayer_topper.Find(panelName);
        if (tf != null)
            return tf as RectTransform;
        return null;
    }

    /// <summary> 获取UI鼠标位置 </summary>
    public Vector2 MousePosition
    {
        get
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(UILayer_middle, Input.mousePosition, UI.UiCanvas.worldCamera, out pos))
            {
                return pos;
            }
            return pos;
        }
    }

    /// <summary> 获取RectTransform鼠标位置 </summary>
    public static Vector2 GetRectMousePosition(RectTransform rectTransform)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, UI.UiCanvas.worldCamera, out pos))
        {
            return pos;
        }
        return pos;
    }

    /// <summary> 获取RectTransform鼠标位置 </summary>
    public static Vector2 GetRectInUiPosition(RectTransform rectTransform)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(UI.m_Instance.UILayer_middle, rectTransform.transform.position, UI.UiCanvas.worldCamera, out pos))
        {
            return pos;
        }
        return pos;
    }

    /// <summary>获取对象在UI下的全局坐标</summary>
    /// <param name="current">对象</param>
    /// <returns>坐标</returns>
    public static Vector2 GetTransformInUiPosition(RectTransform current)
    {
        return TransformToCanvasLocalPosition(current, UI.UiCanvas);
    }

    private static Vector2 TransformToCanvasLocalPosition(RectTransform current, Canvas canvas)
    {
        Vector3 screenPos = canvas.worldCamera.WorldToScreenPoint(current.position);
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPos, canvas.worldCamera, out localPos);
        localPos.x = localPos.x + (float)(current.rect.width * (canvas.GetComponent<RectTransform>().pivot.x - current.pivot.x));
        localPos.y = localPos.y + (float)(current.rect.height * (canvas.GetComponent<RectTransform>().pivot.y - current.pivot.y));
        return localPos;
    }

    /// <summary> 判断鼠标是否碰触到一个对象 </summary>
    public static bool MouseHit(RectTransform rectTransform)
    {
        Vector2 pos = GetRectMousePosition(rectTransform);
        pos.x += rectTransform.rect.width * rectTransform.pivot.x;
        pos.y += rectTransform.rect.height * rectTransform.pivot.y;
        return pos.x > 0 && pos.x < rectTransform.rect.width && pos.y > 0 && pos.y < rectTransform.rect.height;
    }
}
