using System.Runtime.InteropServices;
using UnityEngine;

namespace GeeVision.Core
{
    public class GeeVisionBridge
    {
#if UNITY_STANDALONE_WIN

        private const string GeeVisionDLL = "GeeVisionCore";
        private const string ExamplePluginDLL = "ExamplePlugin";

        [DllImport(GeeVisionDLL)]
        public static extern int Initialize(string adapterName, string adapterPath, string analyzerName, string analyzerPath, ref DataListener listener);

        [DllImport(GeeVisionDLL)]
        public static extern void GeeVisionAddPlugin(string name,string path);

        [DllImport(GeeVisionDLL)]
        public static extern void Start();

        [DllImport(GeeVisionDLL)]
        public static extern void Destroy();

        [DllImport(ExamplePluginDLL)]
        public static extern void SetClickListener(OnClickRecognized recognized);

#elif UNITY_ANDROID

        private const string GeeVisionLibrary = "GeeVision";

        [DllImport(GeeVisionLibrary)]
        public static extern void GeeVisionAndroid_Initialize(ref DataListener listener);

        [DllImport(GeeVisionLibrary)]
        public static extern void GeeVisionAndroid_Destroy();

#endif
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FrameInfo
    {
        public HandInfo LeftHand;
        public HandInfo RightHand;

        public static FrameInfo Create()
        {
            FrameInfo frameInfo = new FrameInfo
            {
                LeftHand = new HandInfo { Joints = new JointInfo[22] },
                RightHand = new HandInfo { Joints = new JointInfo[22] }
            };
            return frameInfo;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HandInfo
    {
        public int TypeID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public JointInfo[] Joints;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JointInfo
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NodeInfo
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TrackingObject
    {
        public float Similarity;
        public float nxIndex;
        public float nyIndex;

        public Vector3 EndPosition;

        public int BasePose;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RecognizedObject
    {
        public float x2d;
        public float y2d;
        public float x3d;
        public float y3d;
        public float z3d;
        public int event_type;
        public float distance;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 224 * 171)]
        public byte[] img;
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void OnReceiveFrame(ref FrameInfo frameInfo);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void OnReceiveInstant(ref TrackingObject hand);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void OnClickRecognized(ref RecognizedObject recognized);    //手纹

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void OnReceiveRealTimeTrack(int type, float x, float y);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void OnReceiveTrack(int type);

    [StructLayout(LayoutKind.Sequential)]
    public struct DataListener
    {
        public OnReceiveFrame OnReceiveFrame;
        public OnReceiveInstant OnReceiveInstant;
        public OnReceiveRealTimeTrack OnReceiveRealTimeTrack;
        public OnReceiveTrack OnReceiveTrack;

        public DataListener(OnReceiveFrame onReceiveFrame, OnReceiveInstant onReceiveInstant,
            OnReceiveRealTimeTrack onReceiveRealTimeTrack, OnReceiveTrack onReceiveTrack)
        {
            OnReceiveFrame = onReceiveFrame;
            OnReceiveInstant = onReceiveInstant;
            OnReceiveRealTimeTrack = onReceiveRealTimeTrack;
            OnReceiveTrack = onReceiveTrack;
        }
    }
}
