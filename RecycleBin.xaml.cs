using menu.ViewModels;

namespace menu
{
    public partial class RecycleBin : ContentPage
    {
        private bool isPressing;
        private object longPressTimer;

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

        private async void OnLabelTapped(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Option", "Cancel", null, "Recover", "Completely Delete");

            switch (action)
            {
                case "Recover":
                    //
                    await DisplayAlert("Option", "You chose to recover", "OK");
                    break;
                case "Completely Delete":
                    //
                    await DisplayAlert("Option", "you chose to completely delete", "OK");
                    break;
            }
        }
    }
}
