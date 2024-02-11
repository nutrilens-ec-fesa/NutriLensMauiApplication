using NutriLensClassLibrary.Models;

namespace NutriLensWebApp.Interfaces
{
    public interface IBarcodeItem
    {
        public List<BarcodeItem> GetBarcodeItemsList();
        public BarcodeItem GetBarcodeItem(string barcode);
        public void InsertBarcodeItem(BarcodeItem barcodeItem);
        public void UpdateBarcodeItem(BarcodeItem barcodeItem);

    }
}
