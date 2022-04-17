/* 
*   Hand Pose
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatSuite.Examples {


    using UnityEngine;
    using NatSuite.ML;
    using NatSuite.ML.Features;
    using NatSuite.ML.Vision;
    using NatSuite.ML.Visualizers;
    using Stopwatch = System.Diagnostics.Stopwatch;

    public sealed class HandPoseSample : MonoBehaviour {
        
        [Header(@"NatML")]
        public string accessKey;

        [Header(@"Prediction")]
        public Texture2D image;
        public HandPoseVisualizer visualizer;

        async void Start () {
            Debug.Log("Fetching model data from NatML...");
            // Fetch the model data from NatML
            var modelData = await MLModelData.FromHub("@natsuite/hand-pose", accessKey);        
            // Deserialize the model
            using var model = modelData.Deserialize();
            // Create the hand pose predictor
            using var predictor = new HandPosePredictor(model);
            // Create input feature
            var input = new MLImageFeature(image);
            // Predict
            var watch = Stopwatch.StartNew();
            var hand = predictor.Predict(input);
            watch.Stop();
            // Visualize
            Debug.Log($"Detected {hand.handedness} hand with score {hand.score:0.##} after {watch.Elapsed.TotalMilliseconds}ms");
            visualizer.Render(hand);
        }
    }
}