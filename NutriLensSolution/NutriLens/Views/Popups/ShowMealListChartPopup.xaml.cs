using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLensClassLibrary.Models;
using System.Collections.ObjectModel;

namespace NutriLens.Views.Popups;

public partial class ShowMealListChartPopup : Popup
{
    public ObservableCollection<TacoItem> TacoItems { get; set; }

    private MealListClass _items { get; set; }
    public ObservableCollection<Brush> Chart1ColorPalette { get; set; }
    public ObservableCollection<Brush> Chart2ColorPalette { get; set; }
    public ObservableCollection<Brush> Chart3ColorPalette { get; set; }

    public ShowMealListChartPopup(MealListClass mealListItem)
	{
		InitializeComponent();

        Chart1ColorPalette = new ObservableCollection<Brush>
            {
                new SolidColorBrush(ColorHelperClass.ProteinColor),
                new SolidColorBrush(ColorHelperClass.CarbohydratesColor),
                new SolidColorBrush(ColorHelperClass.CaloriesColor)
            };

        Chart2ColorPalette = new ObservableCollection<Brush>
            {
                new SolidColorBrush(ColorHelperClass.FatColor),
                new SolidColorBrush(ColorHelperClass.FibersColor),
                new SolidColorBrush(ColorHelperClass.SodiumColor),
            };

        Chart3ColorPalette = new ObservableCollection<Brush>
            {
                new SolidColorBrush(ColorHelperClass.CholesterolColor),
                new SolidColorBrush(ColorHelperClass.CalciumColor),
                new SolidColorBrush(ColorHelperClass.IronColor),
            };

        _items = mealListItem;

        BindingContext = this;

    }

    public List<LineChartDataModel> PartialResultsMacroNutrients1
    {
        get
        {
            var anvisaLimits = AppDataHelperClass.GetAnvisaLimits();
            double limiteCalorias = AppDataHelperClass.UserInfo.KiloCaloriesDiaryObjective;
            double limiteCarboidratos = AppDataHelperClass.UserInfo.DailyCarbohydrateGoal;
            double limiteProteinas = AppDataHelperClass.UserInfo.DailyProteinGoal;

            if (AppDataHelperClass.UserInfo.DailyCarbohydrateGoal.IsZeroOrNaN())
                limiteCarboidratos = anvisaLimits.Carboidratos;

            if (AppDataHelperClass.UserInfo.DailyCarbohydrateGoal.IsZeroOrNaN())
                limiteProteinas = anvisaLimits.Proteinas;


            List<LineChartDataModel> barChart = new List<LineChartDataModel>();

            List<Meal> atual = new List<Meal>();

            foreach (Meal m in _items.MealList)
            {
                atual.Clear();
                LineChartDataModel cal = new LineChartDataModel();
                LineChartDataModel prot = new LineChartDataModel();
                LineChartDataModel carb = new LineChartDataModel();
                
                atual.Add(m);
                MealListClass refeicoesDiarias = new MealListClass(atual);

                cal.Label = m.DateTime.ToString("ddd/MMM/yy");
                prot.Label = m.DateTime.ToString("ddd/MMM/yy");
                carb.Label = m.DateTime.ToString("ddd/MMM/yy");

                cal.Value1 = (refeicoesDiarias.TotalEnergeticConsumption() / limiteCalorias) * 100;
                prot.Value2 = (refeicoesDiarias.TotalProteinsConsumption() / limiteProteinas) * 100;
                carb.Value3 = (refeicoesDiarias.TotalCarbohydratesConsumption() / limiteCarboidratos) * 100;

                barChart.Add(cal); barChart.Add(prot); barChart.Add(carb);

            }

            return barChart;
        }
    }

    public List<LineChartDataModel> PartialResultsMacroNutrients2
    {
        get
        {
            var anvisaLimits = AppDataHelperClass.GetAnvisaLimits();
            double limiteGorduras = AppDataHelperClass.UserInfo.DailyFatGoal;
            double limiteFibras = AppDataHelperClass.UserInfo.DailyFiberGoal;
            double limiteSodio = AppDataHelperClass.UserInfo.DailySodiumGoal;

            if (AppDataHelperClass.UserInfo.DailyFatGoal.IsZeroOrNaN())
                limiteGorduras = anvisaLimits.GordurasTotais;

            if (AppDataHelperClass.UserInfo.DailyFiberGoal.IsZeroOrNaN())
                limiteFibras = anvisaLimits.FibraAlimentar;

            if (AppDataHelperClass.UserInfo.DailySodiumGoal.IsZeroOrNaN())
                limiteSodio = anvisaLimits.Sodio;

            List<LineChartDataModel> barChart = new List<LineChartDataModel>();

            List<Meal> atual = new List<Meal>();

            foreach (Meal m in _items.MealList)
            {
                atual.Clear();
                LineChartDataModel gord = new LineChartDataModel();
                LineChartDataModel fib = new LineChartDataModel();
                LineChartDataModel sod = new LineChartDataModel();

                atual.Add(m);
                MealListClass refeicoesDiarias = new MealListClass(atual);

                gord.Label = m.DateTime.ToString("ddd/MMM/yy");
                fib.Label = m.DateTime.ToString("ddd/MMM/yy");
                sod.Label = m.DateTime.ToString("ddd/MMM/yy");

                gord.Value1 = (refeicoesDiarias.TotalFatConsumption() / limiteGorduras) * 100;
                fib.Value2 = (refeicoesDiarias.TotalFibersConsumption() / limiteFibras) * 100;
                sod.Value3 = (refeicoesDiarias.TotalSodiumConsumption() / limiteSodio) * 100;

                barChart.Add(gord); barChart.Add(fib); barChart.Add(sod);

            }

            return barChart;

        }
    }

    public List<LineChartDataModel> PartialResultsMacroNutrients3
    {
        get
        {
            var anvisaLimits = AppDataHelperClass.GetAnvisaLimits();
            double limiteColesterol = AppDataHelperClass.UserInfo.DailyCholesterolGoal;
            double limiteCalcio = AppDataHelperClass.UserInfo.DailyCalciumGoal;
            double limiteFerro = AppDataHelperClass.UserInfo.DailyIronGoal;

            if (AppDataHelperClass.UserInfo.DailyCholesterolGoal.IsZeroOrNaN())
                limiteColesterol = anvisaLimits.Colesterol;

            if (AppDataHelperClass.UserInfo.DailyCalciumGoal.IsZeroOrNaN())
                limiteCalcio = anvisaLimits.Calcio;

            if (AppDataHelperClass.UserInfo.DailyIronGoal.IsZeroOrNaN())
                limiteFerro = anvisaLimits.Ferro;


            List<LineChartDataModel> barChart = new List<LineChartDataModel>();

            List<Meal> atual = new List<Meal>();

            foreach (Meal m in _items.MealList)
            {
                atual.Clear();
                LineChartDataModel col = new LineChartDataModel();
                LineChartDataModel cal = new LineChartDataModel();
                LineChartDataModel fer = new LineChartDataModel();

                atual.Add(m);
                MealListClass refeicoesDiarias = new MealListClass(atual);

                col.Label = m.DateTime.ToString("ddd/MMM/yy");
                cal.Label = m.DateTime.ToString("ddd/MMM/yy");
                fer.Label = m.DateTime.ToString("ddd/MMM/yy");

                col.Value1 = (refeicoesDiarias.TotalEnergeticConsumption() / limiteColesterol) * 100;
                cal.Value2 = (refeicoesDiarias.TotalProteinsConsumption() / limiteCalcio) * 100;
                fer.Value3 = (refeicoesDiarias.TotalCarbohydratesConsumption() / limiteFerro) * 100;

                barChart.Add(col); barChart.Add(cal); barChart.Add(fer);

            }

            return barChart;
        }
    }

    [RelayCommand]
    private void Appearing()
    {
        OnPropertyChanged(nameof(PartialResultsMacroNutrients1));
        OnPropertyChanged(nameof(PartialResultsMacroNutrients2));
        OnPropertyChanged(nameof(PartialResultsMacroNutrients3));
    }

    private void btnConfirmEntry_Clicked(object sender, EventArgs e)
    {
		CloseAsync();
    }
}