using Camera.MAUI;
using Camera.MAUI.ZXingHelper;
using NutriLens.Services;
using Plugin.Maui.Audio;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ZXing;

namespace NutriLens.ViewModels
{
    internal class BarCodePageVm : INotifyPropertyChanged
    {
        private INavigation _navigation;

        private Result[] _barCodeResults;

        public string BarCodeText { get; set; }
        public ObservableCollection<string> BarCodesRead { get; set; }
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
                _barCodeResults = value;
                if (_barCodeResults != null && _barCodeResults.Length > 0)
                {
                    BarCodeText = _barCodeResults[0].Text;

                    if (BarCodesRead.FirstOrDefault(x => x == BarCodeText) == null)
                    {
                        BarCodesRead.Add(BarCodeText);
                        OnPropertyChanged(nameof(BarCodesRead));
                        Beep();
                    }
                }
                else
                    BarCodeText = "No barcode detected";

                OnPropertyChanged(nameof(BarCodeText));
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

        public BarCodePageVm(INavigation navigation)
        {
            _navigation = navigation;
            BarCodesRead = new ObservableCollection<string>();

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
