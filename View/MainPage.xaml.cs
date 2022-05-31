namespace MonkeyFinder;

public partial class MainPage : ContentPage
{
	public MainPage(MonkeysViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}

