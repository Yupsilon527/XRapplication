using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRBoneController : MonoBehaviour
{
    public enum BoneIndexID
    {
        wrist = 0,
        thumbCMC = 1,
        thumbMCP = 2,
        thumbIP = 3,
        thumbTip = 4,
        indexMCP = 5,
        indexPIP = 6,
        indexDIP = 7,
        indexTip = 8,
        middleMCP = 9,
        middlePIP = 10,
        middleDIP = 11,
        middleTip = 12,
        ringMCP = 13,
        ringPIP = 14,
        ringDIP = 15,
        ringTip = 16,
        pinkyMCP = 17,
        pinkyPIP = 18,
        pinkyDIP = 19,
        pinkyTip = 20,
    }
    public BoneIndexID BoneID = 0;

    Transform LinkedTransform;
    public void TieToTransform(Transform other)
    {
        LinkedTransform = other;
    }
    private void Update()
    {
        UpdatePose();
    }
    
    protected void UpdatePose()
    {
        if (LinkedTransform != null)
        {
            transform.position = LinkedTransform.position;
            
        }
    }
}
