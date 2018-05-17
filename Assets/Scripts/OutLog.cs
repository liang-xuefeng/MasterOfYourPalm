using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class OutLog : MonoBehaviour
{
    static List<string> mLines = new List<string>();
    static List<string> mWriteTxt = new List<string>();
    private string outpath;

    //屏幕打印最多Log数量
    public int _screenLogMaxCount = 8;
    public bool _isInputLogOnScreen = false;
    public Color _color = Color.red;

    public string ip = "127.0.0.1";
    public int point = 60000;

    private UdpClient udpClient;
    private IPEndPoint iPEndPoint;

    void Awake() {
        iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), point);
        udpClient = new UdpClient();

        //Application.persistentDataPath Unity中只有这个路径是既可以读也可以写的。
        outpath = Application.persistentDataPath + "/outLog.txt";
        //Debug.Log("path:" + outpath);
        //每次启动客户端删除之前保存的Log
        if (System.IO.File.Exists(outpath)) {
            File.Delete(outpath);
        }
        //在这里做一个Log的监听
        Application.RegisterLogCallback(HandleLog);
        //Debug.LogError("test 1");
        //Debug.Log("test 2");
        //Debug.LogWarning("test 3");
    }

    void Update() {
        //因为写入文件的操作必须在主线程中完成，所以在Update中哦给你写入文件。
        //if (mWriteTxt.Count > 0) {
        //    string[] temp = mWriteTxt.ToArray();
        //    foreach (string t in temp) {
        //        using (StreamWriter writer = new StreamWriter(outpath, true, Encoding.UTF8)) {
        //            writer.WriteLine(t);
        //        }
        //        mWriteTxt.Remove(t);
        //    }
        //}
    }

    void HandleLog(string logString, string stackTrace, LogType type) {
        mWriteTxt.Add(logString);
        if (type == LogType.Log || type == LogType.Error || type == LogType.Exception) {
            Log(logString);
            //Log(stackTrace);
        }

        try {
            byte[] bytes;
            //bytes = Encoding.UTF8.GetBytes(type.ToString() + "\n " + logString + "\n" + stackTrace);
            bytes = Encoding.UTF8.GetBytes(logString + "\n" + stackTrace);
            udpClient.Send(bytes, bytes.Length, iPEndPoint);

        } catch (System.Exception) {

        }
    }

    //这里我把错误的信息保存起来，用来输出在手机屏幕上
    public void Log(params object[] objs) {
        string text = "";
        for (int i = 0; i < objs.Length; ++i) {
            if (i == 0) {
                text += objs[i].ToString();
            } else {
                text += ", " + objs[i].ToString();
            }
        }
        if (Application.isPlaying) {
            if (mLines.Count > _screenLogMaxCount) {
                mLines.RemoveAt(0);
            }
            mLines.Add(text);

        }
    }

    void OnGUI() {
        if (_isInputLogOnScreen) {
            GUI.color = _color;

            int count = 0;
            for (int i = mLines.Count - 1; i >= 0 && count < _screenLogMaxCount; --i) {
                count++;
                GUILayout.Label(mLines[i]);
            }
        }
    }
}