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
            var selectedList = viewModel?.SelectedList;
            var db = viewModel?.db;

            var sharePage = new Share(db, selectedList);

            await Shell.Current.Navigation.PushAsync(sharePage);
        }


    }

}
