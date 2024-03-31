namespace menu;

public partial class Share : ContentPage
{
    public Share()
    {
        InitializeComponent();
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

    private void GenerateClicked(object sender, EventArgs e)
    {

    }

    private async void CopyClicked(object sender, EventArgs e)
    {
        string textToCopy = GeneratedCodeEntry.Text;

        if (!string.IsNullOrEmpty(textToCopy))
        {
            await Clipboard.SetTextAsync(textToCopy);

            await DisplayAlert("Copied", "The code has been copied to clipboard.", "OK");
        }
    }

}