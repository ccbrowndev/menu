
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
            Routing.RegisterRoute("AddNewItem", typeof(AddNewItem));

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
                    },
                    new ShellContent
                    {
                         Title = "AddNewItem",
                         Route = "AddNewItem",
                         ContentTemplate = new DataTemplate(typeof(AddNewItem)),
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