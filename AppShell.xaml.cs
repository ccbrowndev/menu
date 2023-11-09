
using menu.Views.Dashboard;

namespace menu
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("List", typeof(List));
            Routing.RegisterRoute("Share_Center", typeof(Share_Center));

            var flyoutItem = new FlyoutItem()
            {
                FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                Items =
                {
                    new ShellContent
                    {
                        Title = "List",
                        Route = "List",
                        ContentTemplate = new DataTemplate(typeof(List)),
                    },
                    new ShellContent
                    {
                        Title = "Share Center",
                        Route = "Share_Center",
                        ContentTemplate = new DataTemplate(typeof(Share_Center)),
                    }
                }
            };
            Items.Add(flyoutItem);
        }

        //private void Button_Clicked(object sender, EventArgs e)
        //{
        // do the navigation you want
        //}
        //protected override void OnNavigated(ShellNavigatedEventArgs args)
        //{
        // base.OnNavigated(args);
        //}

        

    }
}