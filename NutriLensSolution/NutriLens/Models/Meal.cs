namespace NutriLens.Models
{
    public class Meal
    {
        public DateTime DateTime { get; set; }
        public string Name { get; set; }
        public List<FoodItem> FoodItems { get; set; }
    }
}
