using CommunityToolkit.Maui.Views;
using NutriLens.Entities;
using NutriLensClassLibrary.Models;
using System.Collections.ObjectModel;

namespace NutriLens.Views.Popups;

public partial class AddTbcaFoodItemPopup : Popup
{
    public bool Confirmed { get; set; }

    public ObservableCollection<TbcaItem> TbcaItems { get; set; }
    public TbcaItem SelectedItem { get; set; }
    public string InputPortion { get => inputPortion.Text; }
    public double InputCalories { get => string.IsNullOrEmpty(inputCalories.Text) ? -1 : double.Parse(inputCalories.Text); }

    public AddTbcaFoodItemPopup()
    {
        InitializeComponent();

        TbcaItems = new ObservableCollection<TbcaItem>();

        if (AppDataHelperClass.TbcaFoodItems == null || AppDataHelperClass.TbcaFoodItems.Count == 0)
        {
            List<TbcaItem> tbcaItems = DaoHelperClass.GetTbcaItemsList();
            AppDataHelperClass.SetTbcaItems(tbcaItems.OrderBy(x => x.Alimento).ToList());
        }

        tbcaPicker.ItemsSource = AppDataHelperClass.TbcaFoodItems;
    }

    private async void BtnConfirmItem_Clicked(object sender, EventArgs e)
    {
        Confirmed = true;

        if (tbcaPicker.SelectedItem != null && tbcaPicker.SelectedItem is TbcaItem selecteditem)
        {
            SelectedItem = selecteditem;
        }

        await CloseAsync();
    }

    private void tbcaTextSearch_Unfocused(object sender, FocusEventArgs e)
    {
        FilterTbcaItemsBySearchTextWords();
    }

    private void FilterTbcaItemsBySearchTextWords()
    {
        tbcaPicker.ItemsSource = null;

        if (string.IsNullOrEmpty(tbcaTextSearch.Text))
            return;

        string[] words = tbcaTextSearch.Text.Trim().Split(" ");

        List<TbcaItem> tbcaItems = new List<TbcaItem>();

        foreach (string word in words)
        {
            if (tbcaItems.Count == 0)
            {
                tbcaItems.AddRange(AppDataHelperClass.TbcaFoodItems.Where(x => x.Alimento.Contains(word)).ToList());

                if (tbcaItems.Count == 0)
                    return;
            }
            else
            {
                tbcaItems = tbcaItems
                    .Where(x => x.Alimento.Contains(word))
                    .ToList();
            }
        }

        tbcaPicker.ItemsSource = tbcaItems;

        if (tbcaItems.Count > 0)
            tbcaPicker.SelectedIndex = 0;
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
        if (tbcaPicker.SelectedItem != null && tbcaPicker.SelectedItem is TbcaItem selecteditem)
        {
            if (double.TryParse(inputPortion.Text, out double quantity))
            {
                inputCalories.Text = (quantity * selecteditem.EnergiaKcal / 100).ToString();
            }
            else
                inputCalories.Text = string.Empty;
        }
    }

    private void tbcaPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (tbcaPicker.SelectedItem != null && tbcaPicker.SelectedItem is TbcaItem selecteditem)
        {
            if (double.TryParse(inputPortion.Text, out double quantity))
            {
                inputCalories.Text = (quantity * selecteditem.EnergiaKcal / 100).ToString();
            }
            else
                inputCalories.Text = string.Empty;
        }
    }

    private void tbcaTextSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        FilterTbcaItemsBySearchTextWords();
    }
}