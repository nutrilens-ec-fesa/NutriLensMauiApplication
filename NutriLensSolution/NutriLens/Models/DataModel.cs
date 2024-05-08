using Newtonsoft.Json;

namespace NutriLens.Models
{
    public class DataModel
    {
        public string Label { get; set; }

        public double Value { get; set; }

        public SolidColorBrush TrackFill { get; set; }

        public SolidColorBrush TrackStroke { get; set; }

        public double TrackStrokeWidth { get; set; }


    }
}
