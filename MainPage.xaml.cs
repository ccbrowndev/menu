using menu.Models;
using menu.ViewModels;

namespace menu
{
    public partial class MainPage : ContentPage
    {

        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            UserListCollectionView.SelectionChanged += vm.OnListCollectionSelectionChanged; 
        }

        private async void OnNavigateButtonClicked(object sender, EventArgs e)
        {
            var viewModel = BindingContext as MainViewModel;
            var share = new Share(viewModel);

            await Shell.Current.Navigation.PushAsync(share);
        }

        private async void OnNavigateButtonClicked1(object sender, EventArgs e)
        {
            var viewModel = BindingContext as MainViewModel;
            var bin = new RecycleBin(viewModel);

            await Shell.Current.Navigation.PushAsync(bin);
        }


    }

}
