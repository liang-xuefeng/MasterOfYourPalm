using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ThreeView : UIPanel
{
    public Text m_Loading;
    public Text m_Instruction;
    public Text m_HandText;

    public Image m_Car;

    public Image m_Image;
    public Texture2D m_Texture2D;

    /// <summary> 开始打印 </summary>
    public StartPrint m_StartPrint;

    /// <summary> 后门 </summary>
    private int m_Backdoor;

    /// <summary> 表信息 </summary>
    private List<CarElement> m_CarElement;

    /// <summary> 手掌信息 </summary>
    public byte[] m_Recognized = new byte[224 * 171];

    private string[] m_LoadingText = {"分析中.\n请将手拿出来", "分析中..\n请将手拿出来", "分析中...\n请将手拿出来"};

    public bool IsStart;

    private float index;
    private float timer;
    private float time;

    /// <summary> 图片存储的位置 </summary>
    private string m_CaptureScreenPath;

    /// <summary> 当前需要打印的图片 </summary>
    private string m_CurrentCapture;

    void Start()
    {
        m_Image.sprite = Sprite.Create(m_Texture2D, new Rect(0, 0, m_Texture2D.width, m_Texture2D.height), Vector2.one);

        SetPixel(m_Recognized);

        m_CarElement = GameTable.GetTable<CarTable>().GetAllElement();

        m_CaptureScreenPath = Application.persistentDataPath + "/Capture";
    }

    void Update()
    {       
        if (!IsStart) return;

        index += Time.deltaTime;
        m_Loading.text = m_LoadingText[(int)(index * 3) % m_LoadingText.Length];

        SetInstruction();

        timer += Time.deltaTime;
        if (timer >= time)
        {
            IsStart = false;
            SetInstruction(GameEvent.Instance.IsBackDoor);
            GameEvent.Instance.IsBackDoor = false;

            m_Loading.gameObject.SetActive(false);
            m_HandText.gameObject.SetActive(true);
            StartCoroutine(CaptureScreen());
        }
    }

    public void Init()
    {
        index = 0;
        timer = 0;
        time = 2f;
        IsStart = true;
        m_Loading.gameObject.SetActive(true);
        m_HandText.gameObject.SetActive(false);
    }

    private string carName;
    private string outText;

    public void SetInstruction(bool isBackDoor = false)
    {
        OutText(isBackDoor, out carName, out outText);

        GetIamge(carName);
        m_Instruction.text = outText;
    }

    public void SetPixel(byte[] Recognized)
    {
        int index = 0;
        for (int i = 0; i < 171; i++)
        {
            for (int j = 0; j < 224; j++)
            {
                m_Texture2D.SetPixel(i, j, Recognized[index] == 0 ? Color.black : Color.white);
                index++;
            }
        }
        m_Texture2D.Apply();
    }

    /// <summary>
    /// 设置车的图片
    /// </summary>
    /// <param name="name"></param>
    private void GetIamge(string name)
    {
        m_Car.overrideSprite = Resources.Load("Cars/" + name, typeof(Sprite)) as Sprite;
    }

    /// <summary>
    /// 输出文字
    /// </summary>
    /// <param name="isBackDoor">是否使用后门</param>
    /// <returns></returns>
    private void OutText(bool isBackDoor, out string spriteName, out string outText)
    {
        int index = UnityEngine.Random.Range(0, m_CarElement.Count);

        spriteName = m_CarElement[index].SpriteName;

        string[] m_Appearance = m_CarElement[index].Appearance;
        string[] m_InternalPerformance = m_CarElement[index].InternalPerformance;
        if (!isBackDoor)
        {
            outText = m_Appearance[UnityEngine.Random.Range(0, m_Appearance.Length)] + " 又 " 
                      + m_InternalPerformance[UnityEngine.Random.Range(0, m_InternalPerformance.Length)]
                      + " 的你\n" + "最适合\n" + m_CarElement[index].Name + " ";
        }
        else
        {
            while (m_CarElement[index].Postern == 0)
            {
                index = UnityEngine.Random.Range(0, m_CarElement.Count);
            }
            spriteName = m_CarElement[index].SpriteName;
            m_Appearance = m_CarElement[index].Appearance;
            m_InternalPerformance = m_CarElement[index].InternalPerformance;

            outText = m_Appearance[UnityEngine.Random.Range(0, m_Appearance.Length)] + " 又 "
                     + m_InternalPerformance[UnityEngine.Random.Range(0, m_InternalPerformance.Length)]
                     + " 的你\n" + "最适合\n" + m_CarElement[index].Name + " ";
        }
    }

    /// <summary>
    /// 截图
    /// </summary>
    IEnumerator CaptureScreen()
    {
        if (!Directory.Exists(m_CaptureScreenPath))
        {
            Directory.CreateDirectory(m_CaptureScreenPath);
        }

        m_CurrentCapture = m_CaptureScreenPath + "/Capture" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".png";
        
        //截图
        Application.CaptureScreenshot(m_CurrentCapture, 0);
        yield return new WaitForSeconds(1.5f);
        if (m_StartPrint != null)
        {
            m_StartPrint(m_CurrentCapture);
        }
    }
}
