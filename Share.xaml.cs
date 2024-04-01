using menu.ViewModels;

namespace menu;

public partial class Share : ContentPage
{
    public Share(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private int _shareCount = 1;


    private void DecreaseCount(object sender, EventArgs e)
    {
        if (_shareCount > 1)
        {
            _shareCount--;
            ShareCountEntry.Text = _shareCount.ToString();
        }
    }

    private void IncreaseCount(object sender, EventArgs e)
    {
        _shareCount++;
        ShareCountEntry.Text = _shareCount.ToString();
    }

    private void CanViewClicked(object sender, EventArgs e)
    {

    }

    private void CanEditClicked(object sender, EventArgs e)
    {

    }

    private void CopyClicked(object sender, EventArgs e)
    {
        var textToCopy = codeGen.Text;

        Clipboard.SetTextAsync(textToCopy);
    }

    private async void ReturnMainForShare(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
        return;

        //MenuDatabase database = new MenuDatabase();
        //MainViewModel viewModel = new MainViewModel(database);
        //await Navigation.PushAsync(new MainPage(viewModel));
    }
}