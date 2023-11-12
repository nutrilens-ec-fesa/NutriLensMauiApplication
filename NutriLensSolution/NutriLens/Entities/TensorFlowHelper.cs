using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TensorFlow;
using System.IO;

namespace NutriLens.Entities
{
    using TensorFlow;
    using System.IO;

    public class TensorFlowHelper
    {
        private TFGraph model;
        private TFSession session;

        public void LoadModel(string modelPath)
        {
            model = new TFGraph();
            model.Import(File.ReadAllBytes(modelPath));
            session = new TFSession(model);
        }

        public TFTensor RunObjectDetection(string modelPath)
        {

            using (var model = new TFGraph())
            {
                var tensor = new TFTensor(File.ReadAllBytes(modelPath));

                using (var session = new TFSession(model))
                {
                    // Setup the runner
                    var runner = session.GetRunner();
                    runner.AddInput(model["input"][0], tensor);
                    runner.Fetch(model["output"][0]);

                    // Run the model
                    var output = runner.Run();

                    // Fetch the results from output:
                    TFTensor result = output[0];
                    return result;
                }
            }


        }
    }

}
