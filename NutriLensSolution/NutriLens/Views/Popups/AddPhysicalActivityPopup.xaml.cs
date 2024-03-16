using CommunityToolkit.Maui.Views;
using NutriLens.Entities;
using NutriLens.Services;
using NutriLensClassLibrary.Models;

namespace NutriLens.Views.Popups;

public partial class AddPhysicalActivityPopup : Popup
{
    public string ActivityName { get => inputActivityName.Text; }
    public int Calories { get; set; }

    public AddPhysicalActivityPopup()
    {
        InitializeComponent();
    }

    private async void btnConfirmActivity_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ActivityName))
        {
            await ViewServices.PopUpManager.PopErrorAsync("Preencha o nome da atividade f�sica");
            return;
        }

        if (!int.TryParse(inputCalories.Text, out int calories) || calories <= 0)
        {
            await ViewServices.PopUpManager.PopErrorAsync("Preencha a queima cal�rica da atividade f�sica");
            return;
        }

        try
        {
            AppDataHelperClass.AddPhysicalActivity(new PhysicalActivity
            {
                ActivityName = ActivityName,
                Calories = calories,
                DateTime = DateTime.Now
            });

            await ViewServices.PopUpManager.PopInfoAsync("Atividade f�sica inserida com sucesso!");
        }
        catch (Exception ex)
        {
            await ViewServices.PopUpManager.PopErrorAsync($"Falha ao adicionar atividade f�sica. {ex.Message}");
        }


        await CloseAsync();
    }

    private async void btnCancel_Clicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}