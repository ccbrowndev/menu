using menu.ViewModels;

namespace menu
{
    public partial class RecycleBin : ContentPage
    {
        
        public RecycleBin(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private void RecoveredClicked(object sender, EventArgs e)
        {


        }

        private async void DeletedClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Confirm Delete", "Are you sure you want to completely delete these items?", "Yes", "No");
            if (answer)
            {

            }
            else
            {

            }
        }

        
    }
}
