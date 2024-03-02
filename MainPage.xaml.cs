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
            var sharePage = new Share();

            await Shell.Current.Navigation.PushAsync(sharePage);
        }


    }

}
