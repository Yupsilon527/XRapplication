using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRHandToImageVisualizer : MonoBehaviour
{
    public XRHandPoseVisualizer vis;
    public Texture2D startimage;

    public float UpdateInterval = 1;

    float LastUpdateTime = 3;
    private void Update()
    {
        if (LastUpdateTime > Time.time)
            return;
        LastUpdateTime = Time.time + UpdateInterval;
        if (Application.isPlaying)
            vis.ChangeImage(startimage, false);
}
}
