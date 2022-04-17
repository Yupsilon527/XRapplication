/* 
*   Hand Pose
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatSuite.ML.Vision {

    using System;
    using NatSuite.ML.Features;
    using NatSuite.ML.Internal;
    using NatSuite.ML.Types;

    /// <summary>
    /// Hand pose predictor.
    /// This predictor only supports predicting a single hand.
    /// </summary>
    public sealed partial class HandPosePredictor : IMLPredictor<HandPosePredictor.Hand> {

        #region --Client API--
        /// <summary>
        /// Create a hand pose predictor.
        /// </summary>
        /// <param name="model">Hand landmark ML model.</param>
        /// <param name="minScore">Minimum confidence score.</param>
        public HandPosePredictor (MLModel model, float minScore = 0.6f) {
            this.model = model as MLEdgeModel;
            this.minScore = minScore;
        }

        /// <summary>
        /// Detect hand pose in an image.
        /// </summary>
        /// <param name="inputs">Input image.</param>
        /// <returns>Detected hand or `null` if the confidence score is too low.</returns>
        public Hand Predict (params MLFeature[] inputs) {
            // Check
            if (inputs.Length != 1)
                throw new ArgumentException(@"Hand landmark predictor expects a single feature", nameof(inputs));
            // Check type
            var input = inputs[0];
            if (!MLImageType.FromType(input.type))
                throw new ArgumentException(@"Hand landmark predictor expects an an array or image feature", nameof(inputs));  
            // Predict
            var inputType = model.inputs[0] as MLImageType;
            using var inputFeature = (input as IMLEdgeFeature).Create(inputType);
            using var outputFeatures = model.Predict(inputFeature);
            // Marshal
            var scoreFeature = new MLArrayFeature<float>(outputFeatures[0]);
            var handednessFeature = new MLArrayFeature<float>(outputFeatures[1]);
            var anchorsFeature = new MLArrayFeature<float>(outputFeatures[2]);
            var score = scoreFeature[0];
            var handedness = handednessFeature[0];
            var anchors = anchorsFeature.ToArray();
            // Check
            if (score < minScore)
                return null;
            // Return
            var result = new Hand(anchors, score, handedness, inputType.height);
            return result;
        }
        #endregion


        #region --Operations--
        private readonly MLEdgeModel model;
        private readonly float minScore;

        void IDisposable.Dispose () { } // Not used
        #endregion
    }
}