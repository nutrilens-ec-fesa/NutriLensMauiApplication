namespace NutriLens.Models
{
    /// <summary>
    /// Representa um modelo de refeição
    /// </summary>
    public class Meal
    {
        /// <summary>
        /// Data e hora da refeição
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Nome da refeição
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Itens alimentícios presentes na refeição
        /// </summary>
        public List<FoodItem> FoodItems { get; set; }

        public override string ToString()
        {
            return $"{DateTime} - {Name} - {FoodItems.Count} itens";
        }
    }
}
