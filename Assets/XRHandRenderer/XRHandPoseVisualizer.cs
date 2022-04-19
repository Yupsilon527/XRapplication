using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NatSuite.ML;
using NatSuite.ML.Features;
using NatSuite.ML.Vision;
using NatSuite.ML.Visualizers;

using System.Threading.Tasks;

public class XRHandPoseVisualizer : MonoBehaviour
{

    [Header(@"NatML")]
    public string accessKey;

    [Header(@"Rig To Mesh")]
    public Transform InitialHand;

    void Start()
    {
        InitHand();
        if (InitialHand != null)
            RigToHand(InitialHand);
    }
    public void ChangeImage(Texture2D image,bool stop_current)
    {
        Debug.Log("Change image to "+image.name);
        if (BusyCoroutine != null)
        {
            if (stop_current)
                StopCoroutine(BusyCoroutine);
            else
                return;
        }
        BusyCoroutine = StartCoroutine(InterpretImage(image));
    }
    void RigToHand(Transform hand)
    {
        foreach (XRBoneController bone in InitialHand.GetComponentsInChildren<XRBoneController>())
        {
            bone.TieToTransform(ActiveBones[(int)bone.BoneID].transform);
        }
    }
    Coroutine BusyCoroutine=null;
    IEnumerator InterpretImage(Texture2D image)
    {
        Debug.Log("Fetching model data from NatML...");
        // Fetch the model data from NatML

        float StartTime = Time.time;
        Task<MLModelData> task = MLModelData.FromHub("@natsuite/hand-pose", accessKey);
        //yield return new WaitUntil(() => task.IsCompleted);
        yield return new WaitWhile(() =>
        {
            Debug.Log("Waiting at " + Time.time + ", total time " + (Time.time - StartTime));
            return !task.IsCompleted;
        });
        Debug.Log("Task completed at " + Time.time + ", total time " + (Time.time - StartTime));
        // Deserialize the model
        MLModel model = task.Result.Deserialize();
        // Create the hand pose predictor
        HandPosePredictor predictor = new HandPosePredictor(model);
        // Create input feature
        var input = new MLImageFeature(image);
        // Predict
        HandPosePredictor.Hand hand = predictor.Predict(input);
        // Visualize
        RenderHandData(hand);
        BusyCoroutine = null;
    }

    public GameObject BonePrefab;
    List<GameObject> ActiveBones = new List<GameObject>();
    void InitHand()
    {
        Debug.Log("Initialize hand...");
        for (int iB = 0; iB<=20; iB++)
        {
            PoolBone();
        }
    }
    GameObject PoolBone()
    {
        GameObject boneP = GameObject.Instantiate(BonePrefab);
        boneP.name = "Bone " + (XRBoneController.BoneIndexID)ActiveBones.Count;
        boneP.transform.SetParent(transform);
        ActiveBones.Add(boneP);
        return boneP;
    }

    void RenderHandData(HandPosePredictor.Hand hand)
    {
        if (hand == null)
            return;
        Debug.Log("Try Draw Hand...");
        int iB = 0;
        foreach (Vector3 p in hand)
        {
            GameObject gOb = (iB >= ActiveBones.Count) ? PoolBone() : ActiveBones[iB];
            gOb.transform.localPosition = p;
            iB++;
        }
        Debug.Log("Hand Drawing Completed!");
    }
}
