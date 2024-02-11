using NutriLensClassLibrary.Models;

namespace NutriLensWebApp.Interfaces
{
    public interface ITbcaItemRepository
    {
        public List<TbcaItem> GetList();
    }
}
