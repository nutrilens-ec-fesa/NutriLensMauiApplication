using Camera.MAUI;
using Camera.MAUI.ZXingHelper;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLens.Services;
using NutriLens.ViewInterfaces;
using Plugin.Maui.Audio;
using PopupLibrary;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ZXing;

namespace NutriLens.ViewModels
{
    internal class BarCodePageVm : INotifyPropertyChanged
    {
        private INavigation _navigation;

        private Result[] _barCodeResults;

        private DateTime? _lastBarCodeDetected;

        public string BarCodeText { get; set; }
        public ObservableCollection<BarcodeItem> BarCodesRead { get; set; }
        public bool AutoStartPreview { get; set; } = false;
        private CameraInfo _camera = null;
        public CameraInfo Camera
        {
            get => _camera;
            set
            {
                _camera = value;
                OnPropertyChanged(nameof(Camera));
                AutoStartPreview = false;
                OnPropertyChanged(nameof(AutoStartPreview));
                AutoStartPreview = true;
                OnPropertyChanged(nameof(AutoStartPreview));
            }
        }
        private ObservableCollection<CameraInfo> _cameras = new();
        public ObservableCollection<CameraInfo> Cameras
        {
            get => _cameras;
            set
            {
                _cameras = value;
                OnPropertyChanged(nameof(Cameras));
            }
        }
        public BarcodeDecodeOptions BarCodeOptions { get; set; }
        public Result[] BarCodeResults
        {
            get => _barCodeResults;
            set
            {
                if (_lastBarCodeDetected == null || (DateTime.Now - _lastBarCodeDetected) > TimeSpan.FromSeconds(3))
                {
                    _lastBarCodeDetected = DateTime.Now;

                    _barCodeResults = value;
                    if (_barCodeResults != null && _barCodeResults.Length > 0)
                    {
                        BarCodeText = _barCodeResults[0].Text;
                        Beep();
                        CheckProduct(BarCodeText);
                    }
                    else
                        BarCodeText = "No barcode detected";

                    OnPropertyChanged(nameof(BarCodeText));
                }
            }
        }

        public Command StartCamera { get; set; }
        public Command StopCamera { get; set; }
        public Command TakeSnapshotCmd { get; set; }

        private bool _takeSnapshot = false;
        public bool TakeSnapshot
        {
            get => _takeSnapshot;
            set
            {
                _takeSnapshot = value;
                OnPropertyChanged(nameof(TakeSnapshot));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task Beep()
        {
            var player = ViewServices.AudioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("beep.mp3"));
            player.Play();
        }

        private async Task CheckProduct(string barcode)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                List<BarcodeItem> barcodeItems = BarCodesRead.ToList();

                BarcodeItem foundBarCodeItem = BarCodesRead.FirstOrDefault(x => x.Barcode == barcode);

                if (foundBarCodeItem != null)
                {
                    if (await ViewServices.PopUpManager.PopYesOrNoAsync("Produto já inserido", "Produto já foi previamente inserido, deseja inserir mais uma vez?"))
                    {
                        BarCodesRead.Add(foundBarCodeItem);
                        OnPropertyChanged(nameof(BarCodesRead));
                    }

                    return;
                }

                EntitiesHelperClass.ShowLoading($"Buscando código '{BarCodeText}' na base de dados");

                BarcodeItem barcodeItem = null;

                await Task.Run(() => barcodeItem = DaoHelperClass.GetBarCodeItem(BarCodeText));

                await EntitiesHelperClass.CloseLoading();

                if (barcodeItem == null)
                {
                    if (await ViewServices.PopUpManager.PopYesOrNoAsync("Produto não encontrado", $"O produto com o código de barras '{barcode}' não foi encontrado. Deseja registrá-lo?"))
                    {
                        await _navigation.PushAsync(ViewServices.ResolvePage<IAddBarcodeProductPage>(barcode));
                    }
                }
                else
                {
                    string consumptionQuantity = await ViewServices.PopUpManager.PopFreeInputAsync(barcodeItem.ProductName, $"Quantos(as) '{barcodeItem.PortionDefinition}' você irá consumir?");

                    if (double.TryParse(consumptionQuantity, out var quantity))
                    { 
                        await ViewServices.PopUpManager.PopInfoAsync("Total de calorias: " + (quantity * barcodeItem.EnergeticValue) / barcodeItem.UnitsPerPortion);
                        BarCodesRead.Add(barcodeItem);
                        OnPropertyChanged(nameof(BarCodesRead));
                    }
                    else
                    {
                        await ViewServices.PopUpManager.PopErrorAsync("Não foi possível identificar a quantidade estipulada. Tente novamente.");
                    }
                }
            });
        }
        public BarCodePageVm(INavigation navigation)
        {
            _navigation = navigation;
            BarCodesRead = new ObservableCollection<BarcodeItem>();

            BarCodeOptions = new BarcodeDecodeOptions
            {
                AutoRotate = true,
                TryHarder = true,
                TryInverted = true,
                PossibleFormats = new[]
                {
                    BarcodeFormat.All_1D
                }
            };

            OnPropertyChanged(nameof(BarCodeOptions));

            StartCamera = new Command(() =>
            {
                AutoStartPreview = true;
                OnPropertyChanged(nameof(AutoStartPreview));
            });
            StopCamera = new Command(() =>
            {
                AutoStartPreview = false;
                OnPropertyChanged(nameof(AutoStartPreview));
            });
            TakeSnapshotCmd = new Command(() =>
            {
                TakeSnapshot = false;
                TakeSnapshot = true;
            });

            OnPropertyChanged(nameof(StartCamera));
            OnPropertyChanged(nameof(StopCamera));
            OnPropertyChanged(nameof(TakeSnapshotCmd));

            Task.Run(async () =>
            {
                await Task.Delay(500);
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Camera = Cameras[0];
                    StartCamera.Execute(this);
                });
            });
        }
    }
}
