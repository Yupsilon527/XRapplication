using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidCameraFeed : MonoBehaviour
{
    public XRHandPoseVisualizer vis;

    public RawImage output;

    public float UpdateInterval = .1f;

    float LastUpdateTime = 0;
    private void Start()
    {
        FindAndroidCam();
        if (cameraTexture != null)
            ReadCamera();
    }
    private void Update()
    {
        if (LastUpdateTime > Time.time)
            return;
        LastUpdateTime = Time.time + UpdateInterval;
        if (cameraTexture!=null)
            ReadCamera();
    }
    WebCamTexture cameraTexture = null;
    void FindAndroidCam()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
            return;
        foreach(var device in devices)
        {
            //if (!device.isFrontFacing)
                cameraTexture = new WebCamTexture(device.name,Screen.width,Screen.height);
        }
        if (cameraTexture == null)
            return;
        cameraTexture.Play();
    }
    void ReadCamera()
    {
        /*Texture2D tex = new Texture2D(cameraTexture.width, cameraTexture.height);
        tex.SetPixels(cameraTexture.GetPixels());
        tex.Apply();*/

        output.transform.localScale = new Vector3(1, cameraTexture.videoVerticallyMirrored ? 1 : -1, 1);
        output.transform.localRotation = Quaternion.Euler(Vector3.forward * cameraTexture.videoRotationAngle);
        output.GetComponent<AspectRatioFitter>().aspectRatio = Camera.main.aspect;

        output.texture = cameraTexture;
        vis.ChangeImage(cameraTexture);        
    }
}
