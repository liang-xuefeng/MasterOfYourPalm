using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace GeeVision.Core
{
    public class GeeVisionManager : MonoBehaviour
    {
        public static GeeVisionManager Instance { private set; get; }
        private GeeVisionListener _listener;

#if UNITY_ANDROID
        
        public AndroidJavaObject GeeVision;

#endif

        //public Text StatusText;
        protected void Awake()
        {
            Instance = this;
            _listener = GeeVisionListener.Instance;

            int status = 0;
#if UNITY_STANDALONE_WIN

            Debug.Log("Windows Platform");
            try
            {
                string path = Application.dataPath + Path.AltDirectorySeparatorChar + "Plugins";
                string adapter = "AdapterRoyale.dll";
                string analyzer = "Analyzer.dll";
                GeeVisionBridge.Initialize(adapter, path, analyzer, path, ref _listener.DataListener);
                GeeVisionBridge.GeeVisionAddPlugin("ExamplePlugin.dll", path);
                GeeVisionBridge.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }

#elif UNITY_ANDROID
            
            GeeVision = new AndroidJavaObject("com.geevision.deepfish.geevisionandroid.GeeVisionBridge",
                new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"));
            status = 1;
            GeeVisionBridge.GeeVisionAndroid_Initialize(ref _listener.DataListener);
            status = 2;
#endif
            //。/StatusText.text = status.ToString();
        }

        protected void OnDestroy()
        {
#if UNITY_STANDALONE_WIN

            GeeVisionBridge.Destroy();

#elif UNITY_ANDROID

            GeeVision.Call("Destroy");
            GeeVisionBridge.GeeVisionAndroid_Destroy();

#endif
        }
    }

//    public class GeeVisionDataObserver : MonoBehaviour, IGeeVisionListener
//    {
//        public DataListener Listener;
//
//        public void Update()
//        {
//            
//        }
//
//        public void OnReceiveFrame(ref FrameInfo frameInfo)
//        {
//            throw new System.NotImplementedException();
//        }
//
//        public void OnReceiveInstant(int whichHand, int typeID, float cofidence)
//        {
//            throw new System.NotImplementedException();
//        }
//
//        public void OnReceiveRealTimeTrack(int type, float x, float y)
//        {
//            throw new System.NotImplementedException();
//        }
//
//        public void OnReceiveTrack(int type)
//        {
//            throw new System.NotImplementedException();
//        }
//    }
}
