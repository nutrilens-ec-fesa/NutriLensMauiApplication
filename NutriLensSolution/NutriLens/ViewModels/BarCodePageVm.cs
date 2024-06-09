using Camera.MAUI;
using Camera.MAUI.ZXingHelper;
using CommunityToolkit.Mvvm.Input;
using ExceptionLibrary;
using Newtonsoft.Json;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLens.Services;
using NutriLens.ViewInterfaces;
using NutriLensClassLibrary.Models;
using Plugin.Maui.Audio;
using PopupLibrary;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ZXing;

namespace NutriLens.ViewModels
{
    public partial class BarCodePageVm : INotifyPropertyChanged
    {
        private INavigation _navigation;

        private Result[] _barCodeResults;

        private DateTime? _lastBarCodeDetected;

        private bool _barcodeCheckingOnGoing = false;

        public string BarCodeText { get; set; }

        public double TotalCalories
        {
            get
            {
                return BarCodesRead.Select(x => x.TotalCaloriesConsumption).Sum();
            }
        }

        public string TotalCaloriesInfo
        {
            get
            {
                return $"{TotalCalories} kcal";
            }
        }

        public ObservableCollection<BarcodeItemEntry> BarCodesRead { get; set; }
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
                    if (!_barcodeCheckingOnGoing && _barCodeResults != null && _barCodeResults.Length > 0 && _barCodeResults[0].Text.Length >= 12 && _barCodeResults[0].Text.StartsWith('7'))
                    {
                        _barcodeCheckingOnGoing = true;
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
                List<BarcodeItemEntry> barcodeItems = BarCodesRead.ToList();

                BarcodeItemEntry foundBarCodeItem = BarCodesRead.FirstOrDefault(x => x.Barcode == barcode);

                if (foundBarCodeItem != null)
                {
                    if (await ViewServices.PopUpManager.PopYesOrNoAsync("Produto já inserido", "Produto já foi previamente inserido, deseja atualizar a quantidade?"))
                        await BarcodeProductPrompt(foundBarCodeItem, true);
                    else
                        _barcodeCheckingOnGoing = false;


                    return;
                }

                EntitiesHelperClass.ShowLoading($"Buscando código '{BarCodeText}' na base de dados");

                BarcodeItemEntry barcodeItem = null;

                await Task.Run(async () =>
                {
                    try
                    {
                        barcodeItem = DaoHelperClass.GetBarCodeItem(BarCodeText);
                    }
                    catch (Exception ex)
                    {
                        await EntitiesHelperClass.CloseLoading();
                        await ViewServices.PopUpManager.PopErrorAsync(ExceptionManager.ExceptionMessage(ex));
                        barcodeItem = null;
                        _barcodeCheckingOnGoing = false;
                    }
                });

                await EntitiesHelperClass.CloseLoading();

                if (barcodeItem == null)
                {
                    if (await ViewServices.PopUpManager.PopYesOrNoAsync("Produto não encontrado", $"O produto com o código de barras '{barcode}' não foi encontrado. Deseja registrá-lo?"))
                    {
                        await _navigation.PushAsync(ViewServices.ResolvePage<IAddBarcodeProductPage>(barcode));

                        // await CheckProduct(barcode);
                    }
                }
                else
                {
                    await BarcodeProductPrompt(barcodeItem);
                }
            });
        }

        public async Task BarcodeProductPrompt(BarcodeItemEntry barcodeItem, bool editing = false)
        {
            string consumptionQuantity;
            
            if(!editing)
                consumptionQuantity = await ViewServices.PopUpManager.PopFreeInputAsync(barcodeItem.ProductName, $"Quantos(as) '{barcodeItem.PortionDefinition}' você irá consumir?", 1000, "Preencha a quantidade...", string.Empty);
            else
                consumptionQuantity = await ViewServices.PopUpManager.PopFreeInputAsync(barcodeItem.ProductName, $"Quantos(as) '{barcodeItem.PortionDefinition}' você irá consumir?", 1000, "Preencha a quantidade...", barcodeItem.QuantityConsumption.ToString());

            if (double.TryParse(consumptionQuantity, out var quantity))
            {
                barcodeItem.QuantityConsumption = quantity;
                
                if(!editing)
                    BarCodesRead.Add(barcodeItem);

                UpdateBarCodesReadList(); 
            }
            else
                await ViewServices.PopUpManager.PopErrorAsync("Não foi possível identificar a quantidade estipulada. Tente novamente.");

            _barcodeCheckingOnGoing = false;
        }

        public BarCodePageVm(INavigation navigation)
        {
            _navigation = navigation;
            BarCodesRead = new ObservableCollection<BarcodeItemEntry>();

            BarCodeOptions = new BarcodeDecodeOptions
            {
                AutoRotate = true,
                TryHarder = true,
                TryInverted = true,
                PossibleFormats = new[]
                {
                    BarcodeFormat.EAN_13
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

        [RelayCommand]
        public void Appearing()
        {
            StartCamera.Execute(this);

            if(AppDataHelperClass.MealToEdit != null)
            {
                foreach (FoodItem food in AppDataHelperClass.MealToEdit.FoodItems)
                {
                    BarCodesRead.Add(food.BarcodeItemEntry);
                }

                UpdateBarCodesReadList();
            }
        }

        [RelayCommand]
        public async void RegisterBarCodeItems()
        {
            List<FoodItem> barcodeFoodItems = new List<FoodItem>();

            foreach(BarcodeItemEntry barcode in BarCodesRead)
            {
                FoodItem barcodeFoodItem = new FoodItem
                {
                    KiloCalories = barcode.TotalCaloriesConsumption,
                    BarcodeItemEntry = barcode,
                    Name = barcode.ProductName,
                    Portion = Math.Round(barcode.BasePortion * barcode.QuantityConsumption, 2).ToString(),
                };

                barcodeFoodItems.Add(barcodeFoodItem);
            }

            // Se for uma nova refeição
            if (AppDataHelperClass.MealToEdit == null)
            {
                Meal newMeal = new Meal()
                {
                    DateTime = DateTime.Now,
                    Name = "Refeição via código de barras",
                    FoodItems = barcodeFoodItems
                };

                AppDataHelperClass.AddMeal(newMeal);

                await ViewServices.PopUpManager.PopInfoAsync("Refeição registrada com sucesso!");
            }
            // Edição de refeição
            else
            {
                AppDataHelperClass.MealToEdit.FoodItems = barcodeFoodItems;
                await AppDataHelperClass.UpdateMeal(AppDataHelperClass.MealToEdit);
                AppDataHelperClass.MealToEdit = null;

                await ViewServices.PopUpManager.PopInfoAsync("Refeição editada com sucesso!");
            }

            await _navigation.PopAsync();
        }

        [RelayCommand]
        public async void DeleteItem(BarcodeItemEntry barcodeItem)
        {
            if (await ViewServices.PopUpManager.PopYesOrNoAsync("Deletar item", $"Deseja realmente deletar \"{barcodeItem.ProductName}\" da refeição?"))
            {
                BarCodesRead.Remove(barcodeItem);
                UpdateBarCodesReadList();
            }
        }

        [RelayCommand]
        public async Task EditItem(BarcodeItemEntry barcodeItem)
        {
            await BarcodeProductPrompt(barcodeItem, true);
        }

        /// <summary>
        /// Método criado como forma de correção de atualização nativa da ObservableCollection,
        /// pois a atualização removendo, editando e adicionando itens, gera alguns problemas
        /// de renderização (falsos duplicados, itens não renderizados, etc). Esse método
        /// basicamente remove todos os itens e os adiciona novamente, dessa forma a interface
        /// funciona da forma como deveria.
        /// </summary>
        private void UpdateBarCodesReadList()
        {
            List<BarcodeItemEntry> updatedItems = BarCodesRead.ToList();

            BarCodesRead.Clear();

            foreach (BarcodeItemEntry item in updatedItems)
            {
                BarCodesRead.Add(item);
            }

            OnPropertyChanged(nameof(TotalCaloriesInfo));
        }
    }
}
