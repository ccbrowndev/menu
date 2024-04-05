namespace menu;

public partial class ShareCenter : ContentPage
{
    public ShareCenter()
    {
        InitializeComponent();
    }

    private async void PasteCode(object sender, EventArgs e)
    {
        if (Clipboard.HasText)
        {
            var text = await Clipboard.GetTextAsync();

            paste.Text = text;
        }
        else
        {
            await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Message", "No code has been added.", "OK");
        }
    }

    private async void CancelReq(object sender, EventArgs e)
    {
        paste.Text = "";

        await Shell.Current.GoToAsync("//MainPage"); ;
    }

    private async void ConfirmReq(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(paste.Text))
        {


            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Message", "No code has been added.", "OK");
        }
    }
}