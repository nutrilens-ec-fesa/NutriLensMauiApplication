namespace NutriLens.Models
{
    public class Meal
    {
        public DateTime DateTime { get; set; }
        public string Name { get; set; }
        public List<FoodItem> FoodItems { get; set; }

        public override string ToString()
        {
            return $"{DateTime} - {Name} - {FoodItems.Count} itens";
        }
    }
}
