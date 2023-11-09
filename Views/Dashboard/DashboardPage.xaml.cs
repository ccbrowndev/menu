using menu.ViewModels.Dashboard;

namespace menu.Views.Dashboard;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardPageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}