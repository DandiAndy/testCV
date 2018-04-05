using System;
using System.Collections.Generic;
using System.Text;

namespace CognitiveXamarin.Objects
{
    public class Result
    {
        public string Id { get; set; }
        public string Project { get; set; }
        public string Iteration { get; set; }
        public string Created { get; set; }
        public Prediction[] Prediction { get; set; }
    }
}
