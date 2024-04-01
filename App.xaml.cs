using menu.Data;
using menu.ViewModels;

namespace menu
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            bool isFirstTime = Preferences.Get("IsFirstTime", true);

            if (isFirstTime)
            {
                MainPage = new Enter();

                Preferences.Set("IsFirstTime", false);
            }
            else
            {
                MainPage = new AppShell();
            }
        }

        protected override void OnStart()
        {
            CheckDeadlinesAsync();
        }

        private async void CheckDeadlinesAsync()
        {
            var database = new MenuDatabase();

            var viewModel = new MainViewModel(database);

            await viewModel.CheckDeadlinesAsync();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
