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

    public int MaxCoroutines = 3;

    void Start()
    {
        renderTex = new Texture2D(Screen.width, Screen.height);
        
        InitHand();
        if (InitialHand != null)
            RigToHand(InitialHand);
    }
    Texture2D renderTex;
    public void ChangeImage(WebCamTexture image)
    {
        if (image == null)
            return;
        Debug.Log("Change image");
        if (nCoroutines < MaxCoroutines)
        {
            renderTex = new Texture2D(image.width, image.height);
            renderTex.SetPixels(image.GetPixels());
            renderTex.Apply();
            StartCoroutine(InterpretImage(renderTex));
        }
    }
    public void ChangeImage(Texture2D image)
    {
        if (image == null)
            return;
        Debug.Log("Change image");
        if (nCoroutines < MaxCoroutines)
        {
            renderTex = new Texture2D(image.width, image.height);
            renderTex.SetPixels(image.GetPixels());
            renderTex.Apply();
            StartCoroutine(InterpretImage(renderTex));
        }
    }
    void RigToHand(Transform hand)
    {
        foreach (XRBoneController bone in hand.GetComponentsInChildren<XRBoneController>())
        {
            bone.TieToTransform(ActiveBones[(int)bone.BoneID].transform);
        }
    }
    float LastUpdateTime = 0;
    int nCoroutines = 0;
    IEnumerator InterpretImage(Texture2D image)
    {
        Debug.Log("Fetching model data from NatML...");
        // Fetch the model data from NatML
        nCoroutines++;

        float StartTime = Time.time;
        Task<MLModelData> task = MLModelData.FromHub("@natsuite/hand-pose", accessKey);
        //yield return new WaitUntil(() => task.IsCompleted);
        yield return new WaitWhile(() =>
        {
            Debug.Log("Waiting at " + Time.time + ", total time " + (Time.time - StartTime));
            return StartTime < LastUpdateTime || !task.IsCompleted;
        });
        Debug.Log("Task completed at " + Time.time + ", total time " + (Time.time - StartTime));
        if (StartTime >= LastUpdateTime)
        {
            Debug.Log("A "+ Time.deltaTime);
            LastUpdateTime = StartTime;
            // Deserialize the model
            using (MLModel model = task.Result.Deserialize())
            {
                //yield return new WaitForEndOfFrame();
                Debug.Log("B " + Time.deltaTime);
                // Create the hand pose predictor
                using (HandPosePredictor predictor = new HandPosePredictor(model))
                {
                    //yield return new WaitForEndOfFrame();
                    Debug.Log("C " + Time.deltaTime);
                    // Create input feature
                    var input = new MLImageFeature(image);
                    // Predict
                    HandPosePredictor.Hand hand = predictor.Predict(input);
                    //yield return new WaitForEndOfFrame();
                    Debug.Log("D " + Time.deltaTime);
                    // Visualize
                    RenderHandData(hand);
                }
            }
        }
        nCoroutines--;
    }
    public void StopAll()
    {
        StopAllCoroutines();
        nCoroutines = 0;
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
