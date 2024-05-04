using CommunityToolkit.Maui.Views;
using NutriLens.Entities;

namespace NutriLens.Views.Popups;

public partial class PeriodChoosePopup : Popup
{
	public bool Confirmed { get; set; }

	public DateTime StartMinDate { get; set; }
    public DateTime StartMaxDate { get; set; }
	public DateTime StartSelectedDate { get; set; }

    public DateTime EndMinDate { get; set; }
    public DateTime EndMaxDate { get; set; }
    public DateTime EndSelectedDate { get; set; }

    public PeriodChoosePopup()
	{
		InitializeComponent();

        DateTime now = DateTime.Now;
        DateTime firstMeal = AppDataHelperClass.GetFirstMealDateTime();

        StartMinDate = firstMeal;
        StartMaxDate = now;
        startDate.Date = firstMeal;

        EndMinDate = now;
        EndMaxDate = now;
        endDate.Date = now;
	}

    private async void BtnConfirmPeriod_Clicked(object sender, EventArgs e)
    {
        Confirmed = true;

        StartSelectedDate = startDate.Date;
        EndSelectedDate = endDate.Date;

        await CloseAsync();
    }

    private void start_DateSelected(object sender, DateChangedEventArgs e)
    {
        if (startDate.Date > endDate.Date)
            endDate.Date = startDate.Date;
    }

    private void end_DateSelected(object sender, DateChangedEventArgs e)
    {
        if (endDate.Date < startDate.Date)
            startDate.Date = endDate.Date;
    }
}