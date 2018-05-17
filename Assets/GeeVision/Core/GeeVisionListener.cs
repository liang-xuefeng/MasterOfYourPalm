using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace GeeVision.Core
{
    public enum RealTimeGestureType
    {
        Off,
        Stay,
        Start,
        Move
    }

    public enum StaticGestureType : int
    {
        None = 0,
        ArrowLeft = 5,
        ArrowRight = 6,
    }

    /// <summary>
    /// 手势
    /// </summary>
    public enum BaseHandPose
    {
        Open = 1 << 0,
        Hold = 1 << 1,
        Finger1 = 1 << 2,
        Finger2 = 1 << 3,
        Finger3 = 1 << 4,
        Finger4 = 1 << 5,
        Finger5 = 1 << 6,
    }

    //    public interface IGeeVisionListener
    //    {
    //        void OnReceiveFrame(ref FrameInfo frameInfo);
    //        void OnReceiveInstant(int whichHand, int typeID, float cofidence);
    //        void OnReceiveRealTimeTrack(int type, float x, float y);
    //        void OnReceiveTrack(int type);
    //    }

    public interface IGeeVisionObserver
    {
        
    }

    public class GeeVisionListener
    {
        public static GeeVisionListener Instance = new GeeVisionListener();
        
        public DataListener DataListener;
        public FrameInfo FrameInfo = FrameInfo.Create();
        public TrackingObject TrackingObject = new TrackingObject();
        public static byte[] Recognized = new byte[224 * 171];      //手掌信息
        
        public RealTimeGestureType RealTimeGestureType;

        public StaticGestureType StaticGestureType;

        public Vector2 LastPoint;

        public int HandStatus;

        protected GeeVisionListener()
        {
            DataListener = new DataListener(OnReceiveFrame, OnReceiveInstant, OnReceiveRealTimeTrack, OnReceiveTrack);
            GeeVisionBridge.SetClickListener(OnClickRecognized);
            Debug.Log("GeeVisionListener");
        }

        public void OnReceiveFrame(ref FrameInfo frameInfo)
        {
            
        }

        public void OnReceiveInstant(ref TrackingObject hand)
        {
            TrackingObject = hand;
            //foreach (BaseHandPose value in Enum.GetValues(typeof(BaseHandPose)))
            //{
            //    if ((hand.BasePose & (int)value) <= 0) continue;
            //    HandStatus = (int)value;
            //    return;
            //}

            if ((hand.BasePose & (1 << 0)) > 0)
            {
                if ((hand.BasePose & (1 << 6)) > 0)
                {
                    HandStatus = (int)BaseHandPose.Finger5;
                }
                else if ((hand.BasePose & (1 << 5)) > 0)
                {
                    HandStatus = (int)BaseHandPose.Finger4;
                }
                else if ((hand.BasePose & (1 << 4)) > 0)
                {
                    HandStatus = (int)BaseHandPose.Finger3;
                }
                else if ((hand.BasePose & (1 << 3)) > 0)
                {
                    HandStatus = (int)BaseHandPose.Finger2;
                }
                else if ((hand.BasePose & (1 << 2)) > 0)
                {
                    HandStatus = (int)BaseHandPose.Finger1;
                }
            }
            else if ((hand.BasePose & (1 << 1)) > 0)
            {
                HandStatus = (int)BaseHandPose.Hold;
            }
            
        }

        public void OnInstant(int whichHand, int basePose, float nx, float ny)
        {
            
        }

        public void OnReceiveRealTimeTrack(int type, float x, float y)
        {
            RealTimeGestureType = (RealTimeGestureType) type;
            LastPoint = new Vector2(x, y);
        }
        
        public void OnReceiveTrack(int type)
        {
            StaticGestureType = (StaticGestureType) type;
//            Debug.Log("OnReceiveTrack : " + StaticGestureType);
        }


        public void OnClickRecognized(ref RecognizedObject recognized)
        {
            Array.Copy(recognized.img, Recognized, recognized.img.Length);
        }
    }

    public interface IGeeVisionPathTrackListener
    {
        
    }

    public class GeeVisionPathTrackObserver : MonoBehaviour, IGeeVisionPathTrackListener
    {
        public static GeeVisionPathTrackObserver Instance;
        protected List<IGeeVisionPathTrackListener> Listeners;
        public void Awake()
        {
            Instance = this;
            Listeners = new List<IGeeVisionPathTrackListener>();
        }

        public void Update()
        {

        }

        public void AddListener(IGeeVisionPathTrackListener listener)
        {
            if (!Listeners.Contains(listener)) Listeners.Add(listener);
        }
    }
}
