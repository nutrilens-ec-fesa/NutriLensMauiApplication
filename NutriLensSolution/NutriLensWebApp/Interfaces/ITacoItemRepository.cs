using NutriLensClassLibrary.Models;

namespace NutriLensWebApp.Interfaces
{
    public interface ITacoItemRepository
    {
        public List<TacoItem> GetList();
        public void UpdateTacoItems(List<TacoItem> tacoItems);
        public void InsertCustomTacoItem(TacoItem tacoItem);
        public void UpdateTacoItem(TacoItem tacoItem);
    }
}
