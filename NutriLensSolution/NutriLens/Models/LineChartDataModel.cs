using Newtonsoft.Json;

namespace NutriLens.Models
{
    public class LineChartDataModel
    {
        public string Label { get; set; }

        public double Value1 { get; set; }
        public double Value2 { get; set; }
        public double Value3 { get; set; }

        public SolidColorBrush TrackFill { get; set; }

        public SolidColorBrush TrackStroke { get; set; }

        public double TrackStrokeWidth { get; set; }


    }
}
