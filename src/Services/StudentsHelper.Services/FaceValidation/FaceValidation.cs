namespace StudentsHelper.Services.FaceValidation
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.Azure.CognitiveServices.Vision.Face;
    using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

    public sealed class FaceValidation : IFaceValidation, System.IDisposable
    {
        private readonly IFaceClient faceClient;

        public FaceValidation(string endpoint, string subscriptionKey)
        {
            var credentials = new ApiKeyServiceClientCredentials(subscriptionKey);
            this.faceClient = new FaceClient(credentials)
            {
                Endpoint = endpoint,
            };
        }

        public async Task<bool> IsFaceValidAsync(Stream imageStream)
        {
            IList<DetectedFace> detectedFaces;

            detectedFaces = await this.faceClient.Face.DetectWithStreamAsync(
                imageStream,
                detectionModel: DetectionModel.Detection03,
                recognitionModel: "recognition_04");

            return detectedFaces.Count > 0;
        }

        public void Dispose()
        {
            this.faceClient.Dispose();
        }
    }
}
