using CommunityToolkit.Maui.Views;
using NutriLens.Entities;
using NutriLensClassLibrary.Models;
using System.Collections.ObjectModel;

namespace NutriLens.Views.Popups;

public partial class AddTacoFoodItemPopup : Popup
{
    public bool Confirmed { get; set; }

    public ObservableCollection<TacoItem> TacoItems { get; set; }
    public TacoItem SelectedItem { get; set; }
    public string InputPortion { get => inputPortion.Text; }
    public double InputCalories { get => string.IsNullOrEmpty(inputCalories.Text) ? -1 : double.Parse(inputCalories.Text); }

    public AddTacoFoodItemPopup()
    {
        InitializeComponent();

        TacoItems = new ObservableCollection<TacoItem>();

        if (AppDataHelperClass.TacoFoodItems == null || AppDataHelperClass.TacoFoodItems.Count == 0)
        {
            List<TacoItem> tacoItems = DaoHelperClass.GetTacoItemsList();
            AppDataHelperClass.SetTacoItems(tacoItems.OrderBy(x => x.Nome).ToList());
        }

        tacoPicker.ItemsSource = AppDataHelperClass.TacoFoodItems;
    }

    public AddTacoFoodItemPopup(FoodItem foodItem)
    {
        InitializeComponent();

        TacoItems = new ObservableCollection<TacoItem>();

        if (AppDataHelperClass.TacoFoodItems == null || AppDataHelperClass.TacoFoodItems.Count == 0)
        {
            List<TacoItem> tacoItems = DaoHelperClass.GetTacoItemsList();
            AppDataHelperClass.SetTacoItems(tacoItems.OrderBy(x => x.Nome).ToList());
        }

        tacoPicker.ItemsSource = AppDataHelperClass.TacoFoodItems;
        tacoPicker.SelectedItem = AppDataHelperClass.TacoFoodItems.Find(x => x.Nome == foodItem.Name);
        SelectedItem = foodItem.TacoFoodItem;
        inputPortion.Text = foodItem.Portion;
    }

    private async void BtnConfirmItem_Clicked(object sender, EventArgs e)
    {
        Confirmed = true;

        if (tacoPicker.SelectedItem != null && tacoPicker.SelectedItem is TacoItem selecteditem)
        {
            SelectedItem = selecteditem;
        }

        await CloseAsync();
    }

    private void tacoTextSearch_Unfocused(object sender, FocusEventArgs e)
    {
        FilterTacoItemsBySearchTextWords();
    }

    private void FilterTacoItemsBySearchTextWords()
    {
        tacoPicker.ItemsSource = null;

        if (string.IsNullOrEmpty(tacoTextSearch.Text))
            return;

        string[] words = tacoTextSearch.Text.Trim().Split(" ");

        List<TacoItem> tacoItems = new List<TacoItem>();

        foreach (string word in words)
        {
            if (tacoItems.Count == 0)
            {
                tacoItems.AddRange(AppDataHelperClass.TacoFoodItems.Where(x => x.Nome.ToUpper().Contains(word.ToUpper())).ToList());

                if (tacoItems.Count == 0)
                    return;
            }
            else
            {
                tacoItems = tacoItems
                    .Where(x => x.Nome.Contains(word))
                    .ToList();
            }
        }

        tacoPicker.ItemsSource = tacoItems;

        if (tacoItems.Count > 0)
            tacoPicker.SelectedIndex = 0;
    }

    private void inputPortion_Unfocused(object sender, FocusEventArgs e)
    {
        //if (tbcaPicker.SelectedItem != null && tbcaPicker.SelectedItem is TbcaItem selecteditem)
        //{
        //    if (double.TryParse(inputPortion.Text, out double quantity))
        //    {
        //        inputCalories.Text = (quantity * selecteditem.EnergiaKcal / 100).ToString();
        //    }
        //}
    }

    private void inputPortion_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (tacoPicker.SelectedItem != null && tacoPicker.SelectedItem is TacoItem selecteditem)
        {
            if (double.TryParse(inputPortion.Text, out double quantity))
            {
                inputCalories.Text = (quantity * selecteditem.GetValue(nameof(selecteditem.EnergiaKcal)) / 100).ToString();
            }
            else
                inputCalories.Text = string.Empty;
        }
    }

    private void tacoPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (tacoPicker.SelectedItem != null && tacoPicker.SelectedItem is TacoItem selecteditem)
        {
            if (selecteditem.Liquid != null && (bool)selecteditem.Liquid)
                lblPortionLabel.Text = "Porção (ml): ";
            else
                lblPortionLabel.Text = "Porção (g): ";

            if (double.TryParse(inputPortion.Text, out double quantity))
            {
                inputCalories.Text = (quantity * selecteditem.GetValue(nameof(selecteditem.EnergiaKcal)) / 100).ToString();
            }
            else
                inputCalories.Text = string.Empty;
        }
    }

    private void tacoTextSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        FilterTacoItemsBySearchTextWords();
    }
}