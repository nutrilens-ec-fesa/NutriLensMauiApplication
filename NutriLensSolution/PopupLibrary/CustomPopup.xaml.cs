namespace PopupLibrary;

public partial class CustomPopup
{
    public CustomPopup(IDeviceDisplay deviceDisplay, int buttonsQuantity)
    {
        InitializeComponent();

        btn1.IsVisible = buttonsQuantity >= 1;
        btn2.IsVisible = buttonsQuantity >= 2;
        btn3.IsVisible = buttonsQuantity >= 3;

        switch (buttonsQuantity)
        {
            case 1:
                if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                {
                    if (DeviceDisplay.Current.MainDisplayInfo.Orientation == DisplayOrientation.Portrait)
                        Size = new(0.85 * (deviceDisplay.MainDisplayInfo.Width / deviceDisplay.MainDisplayInfo.Density), 0.25 * (deviceDisplay.MainDisplayInfo.Height / deviceDisplay.MainDisplayInfo.Density));
                    else
                        Size = new(0.85 * (deviceDisplay.MainDisplayInfo.Width / deviceDisplay.MainDisplayInfo.Density), 0.50 * (deviceDisplay.MainDisplayInfo.Height / deviceDisplay.MainDisplayInfo.Density));
                }
                else
                    Size = new(0.35 * (deviceDisplay.MainDisplayInfo.Width / deviceDisplay.MainDisplayInfo.Density), 0.25 * (deviceDisplay.MainDisplayInfo.Height / deviceDisplay.MainDisplayInfo.Density));
                btn1.Margin = new Thickness(btn1.Margin.Left, btn1.Margin.Top, btn1.Margin.Right * 2, btn1.Margin.Bottom);
                break;
            case 2:
                if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                {
                    if (DeviceDisplay.Current.MainDisplayInfo.Orientation == DisplayOrientation.Portrait)
                        Size = new(0.85 * (deviceDisplay.MainDisplayInfo.Width / deviceDisplay.MainDisplayInfo.Density), 0.25 * (deviceDisplay.MainDisplayInfo.Height / deviceDisplay.MainDisplayInfo.Density));
                    else
                        Size = new(0.85 * (deviceDisplay.MainDisplayInfo.Width / deviceDisplay.MainDisplayInfo.Density), 0.50 * (deviceDisplay.MainDisplayInfo.Height / deviceDisplay.MainDisplayInfo.Density));
                }
                else
                    Size = new(0.35 * (deviceDisplay.MainDisplayInfo.Width / deviceDisplay.MainDisplayInfo.Density), 0.25 * (deviceDisplay.MainDisplayInfo.Height / deviceDisplay.MainDisplayInfo.Density));
                btn2.Margin = new Thickness(btn2.Margin.Left, btn2.Margin.Top, btn2.Margin.Right * 2, btn2.Margin.Bottom);
                break;
            case 3:
                if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                {
                    if (DeviceDisplay.Current.MainDisplayInfo.Orientation == DisplayOrientation.Portrait)
                    {
                        stkButtons.Orientation = StackOrientation.Vertical;
                        stkButtons.HeightRequest *= 2.3;
                        btn1.Margin = new Thickness(20, 20, 20, 10);
                        btn2.Margin = new Thickness(20, 10, 20, 10);
                        btn3.Margin = new Thickness(20, 10, 20, 20);
                        Size = new(0.85 * (deviceDisplay.MainDisplayInfo.Width / deviceDisplay.MainDisplayInfo.Density), 0.38 * (deviceDisplay.MainDisplayInfo.Height / deviceDisplay.MainDisplayInfo.Density));
                    }
                    else
                    {
                        Size = new(0.85 * (deviceDisplay.MainDisplayInfo.Width / deviceDisplay.MainDisplayInfo.Density), 0.50 * (deviceDisplay.MainDisplayInfo.Height / deviceDisplay.MainDisplayInfo.Density));
                        btn3.Margin = new Thickness(btn3.Margin.Left, btn3.Margin.Top, btn3.Margin.Right * 2, btn3.Margin.Bottom);
                    }
                }
                else
                {
                    Size = new(0.45 * (deviceDisplay.MainDisplayInfo.Width / deviceDisplay.MainDisplayInfo.Density), 0.25 * (deviceDisplay.MainDisplayInfo.Height / deviceDisplay.MainDisplayInfo.Density));
                    btn3.Margin = new Thickness(btn3.Margin.Left, btn3.Margin.Top, btn3.Margin.Right * 2, btn3.Margin.Bottom);
                }
                break;
        }


    }

    private async void OnBtn1Clicked(object sender, EventArgs e)
    {
        Close(1);
    }

    private async void OnBtn2Clicked(object sender, EventArgs e)
    {
        Close(2);
    }

    private async void OnBtn3Clicked(object sender, EventArgs e)
    {
        Close(3);
    }
}