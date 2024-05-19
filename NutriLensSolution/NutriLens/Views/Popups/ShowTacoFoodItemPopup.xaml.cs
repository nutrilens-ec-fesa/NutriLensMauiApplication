using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLensClassLibrary.Models;
using System.Collections.ObjectModel;

namespace NutriLens.Views.Popups;

public partial class ShowTacoFoodItemPopup : Popup
{
    public ObservableCollection<TacoItem> TacoItems { get; set; }

    private FoodItem _item { get; set; }
    public ObservableCollection<Brush> Chart1ColorPalette { get; set; }
    public ObservableCollection<Brush> Chart2ColorPalette { get; set; }
    public ObservableCollection<Brush> Chart3ColorPalette { get; set; }

    public ShowTacoFoodItemPopup(FoodItem foodItem)
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

        _item = foodItem;

        BindingContext = this;

    }

    public List<DataModel> PartialResultsMacroNutrients1
    {
        get
        {
            var anvisaLimits = AppDataHelperClass.GetAnvisaLimits();
            double limiteCalorias = AppDataHelperClass.UserInfo.DailyKiloCaloriesGoal;
            double limiteCarboidratos = AppDataHelperClass.UserInfo.DailyCarbohydrateGoal;
            double limiteProteinas = AppDataHelperClass.UserInfo.DailyProteinGoal;

            if (AppDataHelperClass.UserInfo.DailyCarbohydrateGoal.IsZeroOrNaN())
                limiteCarboidratos = anvisaLimits.Carboidratos;

            if (AppDataHelperClass.UserInfo.DailyCarbohydrateGoal.IsZeroOrNaN())
                limiteProteinas = anvisaLimits.Proteinas;

            double calorias = (double)_item.TacoFoodItem.EnergiaKcal;
            double carboidratos = (double)_item.TacoFoodItem.Carboidrato;    //300g   %100 VD
            double proteinas = (double)_item.TacoFoodItem.Proteina;          //75g    %100 VD 

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

            double gordura = (double)_item.TacoFoodItem.Lipideos;                   //55g    %100 VD Gorduras totais
            double fibra = (double)_item.TacoFoodItem.FibraAlimentar;                  //25g    %100 VD
            double sodio = (double)_item.TacoFoodItem.Sodio;                  //2,4g   %100 VD

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


            double colesterol = (double)_item.TacoFoodItem.Colesterol;        //300mg    %100 VD Gorduras totais
            double calcio = (double)_item.TacoFoodItem.Calcio;                //1000mg    %100 VD
            double ferro = (double)_item.TacoFoodItem.Ferro;                    //14mg   %100 VD

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

    //public string Items
    //{
    //    get
    //    {
    //        string items = _item.TacoFoodItem.Nome + "\n" +
    //        "Calorias: " + _item.KiloCalories + "kcal" + "\n" +
    //        "Proteinas: " + _item.TacoFoodItem.Proteina.ToString() + "g" + "\n" +
    //        "Carboidratos: " + _item.TacoFoodItem.Carboidrato.ToString() + "g" + "\n" +
    //        "Lipideos: " + _item.TacoFoodItem.Lipideos.ToString() + "g" + "\n" +
    //        "Fibra Alimentar: " + _item.TacoFoodItem.FibraAlimentar.ToString() + "g" + "\n" +
    //        "Sódio: " + _item.TacoFoodItem.Sodio.ToString() + "mg" + "\n" +
    //        "Colesterol: " + _item.TacoFoodItem.Colesterol.ToString() + "mg" + "\n" +
    //        "Calcio: " + _item.TacoFoodItem.Calcio.ToString() + "mg" + "\n" +
    //        "ferro: " + _item.TacoFoodItem.Ferro.ToString() + "mg" + "\n";

    //        return items;
    //    }
    //}

    private void btnConfirmEntry_Clicked(object sender, EventArgs e)
    {
		CloseAsync();
    }
}