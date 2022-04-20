using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRHandToImageVisualizer : MonoBehaviour
{
    public XRHandPoseVisualizer vis;
    public Texture2D startimage;

    public float UpdateInterval = 1;

    float LastUpdateTime = 3;
    private void Start()
    {
        Debug.Log("XRHandToImageVisualizer Start");
        vis.ChangeImage(startimage);
        LastUpdateTime = UpdateInterval;
    }
    private void Update()
    {
        //Debug.Log("Updatetime "+ LastUpdateTime + " " + Time.time);
        if (LastUpdateTime > Time.time)
            return;
        LastUpdateTime = Time.time + UpdateInterval;
        Debug.Log("XRHandToImageVisualizer Update");
        //if (Application.isPlaying)
            vis.ChangeImage(startimage);
}
}
