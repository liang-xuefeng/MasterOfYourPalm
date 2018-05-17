using System.Diagnostics;
using System.IO;
using LCPrinter;
using UnityEngine;
using UnityEngine.UI;

public class FourView : UIPanel
{
    public Text m_Loading;

    public bool IsStart;

    public Accomplish m_Accomplish;

    private string[] m_LoadingText = { "打印中.", "打印中..", "打印中..." };

    private float index;
    private float timer;
    private float time;

    private string path;

    void Update()
    {
        if (!IsStart) return;

        index += Time.deltaTime;
        m_Loading.text = m_LoadingText[(int)(index * 3) % m_LoadingText.Length];

        timer += Time.deltaTime;
        if (timer >= time)
        {
            IsStart = false;
            if (m_Accomplish != null) m_Accomplish();
        }
    }

    public void Init(string path)
    {
        IsStart = true;
        index = 0;
        timer = 0;
        time = 35f;

        FileInfo mFileInfo = new FileInfo(path);
        PrintImage(mFileInfo);
    }

    /// <summary>
    /// 打印机调用
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    private void PrintImage(FileInfo fileInfo)
    {
        UnityEngine.Debug.Log(fileInfo.FullName);

        //Process.Start("mspaint.exe", "/pt " + fileInfo.FullName);
        //Process.Start(@"C:\Program Files (x86)\2345Soft\2345Pic\2345Pic.exe", "/pt " + fileInfo.FullName);
        Print.PrintTextureByPath(fileInfo.FullName, 1, "");
    }

}
