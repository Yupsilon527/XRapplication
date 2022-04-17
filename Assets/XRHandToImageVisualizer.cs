using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRHandToImageVisualizer : MonoBehaviour
{
    public XRHandPoseVisualizer vis;
    [Header(@"Prediction")]
    public Texture2D startimage;

    float LastUpdateTime = 3;
    private void Update()
    {
        if (LastUpdateTime > Time.time)
            return;
        LastUpdateTime = Time.time + 1f;
        if (Application.isPlaying)
            vis.ChangeImage(startimage, false);
}
}
