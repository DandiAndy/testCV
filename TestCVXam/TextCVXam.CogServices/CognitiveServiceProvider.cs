using System;
using Microsoft.Cognitive.CustomVision.Training;

namespace TextCVXam.CogServices
{
    public class CognitiveServiceProvider
    {
        private string trainKey = "4284160899c74a8aab7aa433b4a8213d";
        private string predictKey = "83ad0056ce8441e681f07f222a83c237";
        private Guid projectId = Guid.Parse("8612a8af-313d-4b69-a38b-eec74e79ca79");

        private string perdictURL =
                "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.1/Prediction/8612a8af-313d-4b69-a38b-eec74e79ca79/image?iterationId=335758cc-dbc0-4845-b729-b0b82712d338"
            ;

        public void Train()
        {



            TrainingApi train = new TrainingApi()
            {
                ApiKey = trainKey
            };

            

        }
    }
}
