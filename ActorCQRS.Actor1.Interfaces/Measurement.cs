using System;
using System.Collections.Generic;
using System.Text;

namespace ActorCQRS.Actor1.Interfaces
{
    public class Measurement
    {
        public double? NumericValue { get; set; }
        public string StringValue { get; set; }
        public int Precision { get; set; }
        public DateTime UTCCapturedTimestamp { get; set; }
        public DateTime UTCSavedTimestamp { get; set; }
    }
}
