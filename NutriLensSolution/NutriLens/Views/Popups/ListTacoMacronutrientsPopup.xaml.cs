using CommunityToolkit.Maui.Views;
using NutriLens.Entities;
using NutriLensClassLibrary.Models;
using System.Collections.ObjectModel;

namespace NutriLens.Views.Popups;

public partial class ListTacoMacronutrientsPopup : Popup
{
    public ObservableCollection<TacoItem> TacoItems { get; set; }

    public FoodItem _item { get; set; }

    public string _items { get; set; }

    public ListTacoMacronutrientsPopup(FoodItem foodItem)
	{
		InitializeComponent();

        _item = foodItem;
        _items = Items;

    }

    public string Items
    {
        get
        {
            string items = _item.TacoFoodItem.Nome + "\n" +
            "Calorias: " + _item.KiloCalories + "kcal" + "\n" +
            "Proteinas: " + _item.TacoFoodItem.Proteina.ToString() + "g" + "\n" +
            "Carboidratos: " + _item.TacoFoodItem.Carboidrato.ToString() + "g" + "\n" +
            "Lipideos: " + _item.TacoFoodItem.Lipideos.ToString() + "g" + "\n" +
            "Fibra Alimentar: " + _item.TacoFoodItem.FibraAlimentar.ToString() + "g" + "\n" +
            "Sódio: " + _item.TacoFoodItem.Sodio.ToString() + "mg" + "\n" +
            "Colesterol: " + _item.TacoFoodItem.Colesterol.ToString() + "mg" + "\n" +
            "Calcio: " + _item.TacoFoodItem.Calcio.ToString() + "mg" + "\n" +
            "ferro: " + _item.TacoFoodItem.Ferro.ToString() + "mg" + "\n";

            return items;
        }
    }

    private void btnConfirmEntry_Clicked(object sender, EventArgs e)
    {
		CloseAsync();
    }
}