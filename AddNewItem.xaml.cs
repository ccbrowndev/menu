using menu.Domain;
using menu.Services;

namespace menu;

public partial class AddNewItem : ContentPage
{
    private int itemId;
    private int listId;

    public AddNewItem(int _listId, int _itemId)
	{
		InitializeComponent();
        this.itemId = _itemId;
        this.listId = _listId;
	}

    private async void OnNavButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ListDetails(listId));
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        string title = Title.Text;
        string description = Description.Text;
        DateTime deadline = datePicker.Date + timePicker.Time;

        var item = new UserListItem
        {
            id = itemId,
            UserListid = itemId,
            name = title,
            details = description,
            deadline = deadline
        };

        MenuManager menuManager = new MenuManager();
        menuManager.UpdateUserListItem(item);

        menuManager.SaveChanges();

        await DisplayAlert("Success", "Item added.", "OK");

        await Navigation.PushAsync(new ListDetails(listId));
    }

}