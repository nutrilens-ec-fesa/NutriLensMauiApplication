using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExceptionLibrary;
using NutriLens.Entities;
using NutriLens.Services;
using NutriLens.ViewInterfaces;
using NutriLensClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriLens.ViewModels
{
    public partial class AiModelPromptVm : ObservableObject
    {
        private INavigation _navigation;

        public OpenAiPrompt OpenAiPrompt { get; set; }

        public AiModelPromptVm(INavigation navigation)
        {
            _navigation = navigation;
            OpenAiPrompt = new OpenAiPrompt();
        }

        [RelayCommand]
        private async Task Appearing()
        {
            try
            {
                EntitiesHelperClass.ShowLoading("Resgatando último prompt");

                await Task.Run(() => OpenAiPrompt = DaoHelperClass.GetOpenAiPrompt());

                await EntitiesHelperClass.CloseLoading();

                OnPropertyChanged(nameof(OpenAiPrompt));
            }
            catch(Exception ex)
            {
                await ViewServices.PopUpManager.PopErrorAsync(ExceptionManager.ExceptionMessage(ex));
                await EntitiesHelperClass.CloseLoading();
            }
        }

        [RelayCommand]
        private async Task UpdatePrompt()
        {
            try
            {
                EntitiesHelperClass.ShowLoading("Atualizando prompt");

                OpenAiPrompt.Id = null;

                await Task.Run(() => DaoHelperClass.PostNewOpenAiPrompt(OpenAiPrompt));

                await EntitiesHelperClass.CloseLoading();

                await ViewServices.PopUpManager.PopInfoAsync("Novo prompt atualizado com sucesso!");

                await _navigation.PopAsync();
            }
            catch(Exception ex)
            {
                await ViewServices.PopUpManager.PopErrorAsync(ExceptionManager.ExceptionMessage(ex));
                await EntitiesHelperClass.CloseLoading();
            }
        }
    }
}
