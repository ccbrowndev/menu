namespace menu;

public partial class Enter : ContentPage
{
	public Enter()
	{
		InitializeComponent();
	}

    private void OnEnterClicked(object sender, EventArgs e)
    {
        string randomString = GenerateRandomString(16);
        string pid = randomString;

        Application.Current.MainPage = new AppShell();
    }

    private string GenerateRandomString(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var randomString = new char[length];

        for (int i = 0; i < length; i++)
        {
            randomString[i] = chars[random.Next(chars.Length)];
        }

        return new String(randomString);
    }
}