using System;
using System.Collections.Generic;
using System.Text;

namespace CognitiveXamarin.Objects
{
    public class Prediction
    {
        public string TagId { get; set; }
        public string Tag { get; set;  }
        public double Probability { get; set; }
    }
}