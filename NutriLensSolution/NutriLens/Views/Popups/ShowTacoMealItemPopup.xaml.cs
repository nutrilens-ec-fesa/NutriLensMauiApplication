using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLensClassLibrary.Models;
using System.Collections.ObjectModel;

namespace NutriLens.Views.Popups;

public partial class ShowTacoMealItemPopup : Popup
{
    public ObservableCollection<TacoItem> TacoItems { get; set; }

    private MealListClass _items { get; set; }
    public ObservableCollection<Brush> Chart1ColorPalette { get; set; }
    public ObservableCollection<Brush> Chart2ColorPalette { get; set; }
    public ObservableCollection<Brush> Chart3ColorPalette { get; set; }

    public ShowTacoMealItemPopup(Meal mealItem)
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

        List<Meal> meals = new List<Meal>();
        MealListClass it = new MealListClass(meals);
        _items = it;
        _items.MealList.Add(mealItem);

        BindingContext = this;

    }

    public List<DataModel> PartialResultsMacroNutrients1
    {
        get
        {
            var anvisaLimits = AnvisaLimits.GetAnvisaLimits();
            double limiteCalorias = AppDataHelperClass.UserInfo.KiloCaloriesDiaryObjective;
            double limiteCarboidratos = AppDataHelperClass.UserInfo.DailyCarbohydrateGoal;
            double limiteProteinas = AppDataHelperClass.UserInfo.DailyProteinGoal;

            if (AppDataHelperClass.UserInfo.DailyCarbohydrateGoal.IsZeroOrNaN())
                limiteCarboidratos = anvisaLimits.Carboidratos;

            if (AppDataHelperClass.UserInfo.DailyCarbohydrateGoal.IsZeroOrNaN())
                limiteProteinas = anvisaLimits.Proteinas;

            double calorias = _items.TotalEnergeticConsumption();
            double carboidratos = _items.TotalCarbohydratesConsumption();    //300g   %100 VD
            double proteinas = _items.TotalProteinsConsumption();          //75g    %100 VD 

            List<DataModel> barChart = new List<DataModel>();

            DataModel prot = new DataModel();
            prot.Label = "Proteínas:\n " + proteinas.ToString("0.0") + "g\n" + " de " + limiteProteinas.ToString("0.0") + "g";
            prot.Value = (proteinas / limiteProteinas) * 100;
            barChart.Add(prot);

            DataModel carb = new DataModel();
            carb.Label = "Carboidratos:\n " + carboidratos.ToString("0.0") + "g\n" + " de " + limiteCarboidratos.ToString("0.0") + "g";
            carb.Value = (carboidratos / limiteCarboidratos) * 100;
            barChart.Add(carb);

            DataModel cal = new DataModel();
            cal.Label = "Calorias:\n " + calorias.ToString("0.0") + "kcal\n" + " de " + limiteCalorias.ToString("0.0") + "kcal";
            cal.Value = (calorias / limiteCalorias) * 100;
            barChart.Add(cal);

            return barChart;
        }
    }

    public List<DataModel> PartialResultsMacroNutrients2
    {
        get
        {
            var anvisaLimits = AnvisaLimits.GetAnvisaLimits();
            double limiteGorduras = AppDataHelperClass.UserInfo.DailyFatGoal;
            double limiteFibras = AppDataHelperClass.UserInfo.DailyFiberGoal;
            double limiteSodio = AppDataHelperClass.UserInfo.DailySodiumGoal;

            if (AppDataHelperClass.UserInfo.DailyFatGoal.IsZeroOrNaN())
                limiteGorduras = anvisaLimits.GordurasTotais;

            if (AppDataHelperClass.UserInfo.DailyFiberGoal.IsZeroOrNaN())
                limiteFibras = anvisaLimits.FibraAlimentar;

            if (AppDataHelperClass.UserInfo.DailySodiumGoal.IsZeroOrNaN())
                limiteSodio = anvisaLimits.Sodio;

            double gordura = _items.TotalFatConsumption();                   //55g    %100 VD Gorduras totais
            double fibra = _items.TotalFibersConsumption();                  //25g    %100 VD
            double sodio = _items.TotalSodiumConsumption();                  //2,4g   %100 VD

            List<DataModel> barChart = new List<DataModel>();

            DataModel gord = new DataModel();
            gord.Label = "Gorduras:\n " + gordura.ToString("0.0") + "g\n" + " de " + limiteGorduras.ToString("0.0") + "g";
            gord.Value = (gordura / limiteGorduras) * 100;
            gord.TrackStroke = SolidColorBrush.Red;
            barChart.Add(gord);


            DataModel fib = new DataModel();
            fib.Label = "Fibras:\n " + fibra.ToString("0.0") + "g\n" + " de " + limiteFibras.ToString("0.0") + "g";
            fib.Value = (fibra / limiteFibras) * 100;
            fib.TrackFill = SolidColorBrush.Magenta;
            barChart.Add(fib);

            DataModel sod = new DataModel();
            sod.Label = "Sódio:\n " + sodio.ToString("0.0") + "mg\n" + " de " + limiteSodio.ToString("0.0") + "mg";
            sod.Value = (sodio / limiteSodio) * 100;
            barChart.Add(sod);

            return barChart;
        }
    }

    public List<DataModel> PartialResultsMacroNutrients3
    {
        get
        {
            var anvisaLimits = AnvisaLimits.GetAnvisaLimits();
            double limiteColesterol = AppDataHelperClass.UserInfo.DailyCholesterolGoal;
            double limiteCalcio = AppDataHelperClass.UserInfo.DailyCalciumGoal;
            double limiteFerro = AppDataHelperClass.UserInfo.DailyIronGoal;

            if (AppDataHelperClass.UserInfo.DailyCholesterolGoal.IsZeroOrNaN())
                limiteColesterol = anvisaLimits.Colesterol;

            if (AppDataHelperClass.UserInfo.DailyCalciumGoal.IsZeroOrNaN())
                limiteCalcio = anvisaLimits.Calcio;

            if (AppDataHelperClass.UserInfo.DailyIronGoal.IsZeroOrNaN())
                limiteFerro = anvisaLimits.Ferro;


            double colesterol = _items.TotalCholesterolConsumption();        //300mg    %100 VD Gorduras totais
            double calcio = _items.TotalCalciumConsumption();                //1000mg    %100 VD
            double ferro = _items.TotalIronConsumption();                    //14mg   %100 VD

            List<DataModel> barChart = new List<DataModel>();

            DataModel col = new DataModel();
            col.Label = "Colesterol:\n " + colesterol.ToString("0.0") + "mg\n" + " de " + limiteColesterol.ToString("0.0") + "mg";
            col.Value = (colesterol / limiteColesterol) * 100;
            barChart.Add(col);


            DataModel cal = new DataModel();
            cal.Label = "Cálcio:\n " + calcio.ToString("0.0") + "mg\n" + " de " + limiteCalcio.ToString("0.0") + "mg";
            cal.Value = (calcio / limiteCalcio) * 100;
            barChart.Add(cal);

            DataModel fer = new DataModel();
            fer.Label = "Ferro:\n " + ferro.ToString("0.0") + "mg\n" + " de " + limiteFerro.ToString("0.0") + "mg";
            fer.Value = (ferro / limiteFerro) * 100;
            barChart.Add(fer);

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