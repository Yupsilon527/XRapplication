

using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;


public class ARFoundationHandDetectDemo : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The ARCameraManager which will produce frame events.")]
    ARCameraManager m_CameraManager;

    public ARCameraManager cameraManager
    {
        get => m_CameraManager;
        set => m_CameraManager = value;
    }

    [SerializeField]
    Camera m_Cam;
    
    [SerializeField]
    GameObject m_TargetGo;

    public GameObject targetGo
    {
        get => m_TargetGo;
        set => m_TargetGo = value;
    }

    public GameObject m_Go;

    [SerializeField]
    RawImage m_RawImage;

    public Material m_BlitMat;

    private Texture2D m_TexFromNative = null;
    public Texture2D textureFromNative
    {
        get => m_TexFromNative;
        set => m_TexFromNative = value;
    }


    void OnEnable()
    {
        if (m_CameraManager != null)
        {
            m_CameraManager.frameReceived += OnCameraFrameReceived;
        }
    }

    void OnDisable()
    {
        if (m_CameraManager != null)
        {
            m_CameraManager.frameReceived -= OnCameraFrameReceived;
        }
    }

    unsafe void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {

#if !UNITY_EDITOR && UNITY_IOS

        var cameraParams = new XRCameraParams
        {
            zNear = m_Cam.nearClipPlane,
            zFar = m_Cam.farClipPlane,
            screenWidth = Screen.width,
            screenHeight = Screen.height,
            screenOrientation = Screen.orientation
        };

        XRCameraFrame frame;
        if (cameraManager.subsystem.TryGetLatestFrame(cameraParams, out frame))
        {
            if (m_HandDetector.IsIdle)
            {
                m_HandDetector.StartDetect(frame.nativePtr);
            }
        }

#endif
    }

    private void OnHandDetectorCompleted(object sender, Vector2 pos)
    {
        var handPos = new Vector3();
        handPos.x = pos.x;
        handPos.y = 1 - pos.y;
        handPos.z = 5;//m_Cam.nearClipPlane;
        var handWorldPos = m_Cam.ViewportToWorldPoint(handPos);
        Debug.Log(handPos);

        return;


        if (m_Go == null)
        {
            m_Go = Instantiate(m_TargetGo, handWorldPos, Quaternion.identity);
        }

        m_Go.transform.position = handWorldPos;
        m_Go.transform.LookAt(m_Cam.transform);
    }


}