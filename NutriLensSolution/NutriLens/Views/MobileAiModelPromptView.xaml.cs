using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;

namespace NutriLens.Views;

public partial class MobileAiModelPromptView : ContentPage, IAiModelPromptPage
{
	public MobileAiModelPromptView()
	{
		InitializeComponent();
		BindingContext = new AiModelPromptVm(Navigation);
	}
}