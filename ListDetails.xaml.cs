using menu.Domain;
using menu.Services;

namespace menu;

public partial class ListDetails : ContentPage
{
	public ListDetails(int id)
	{
		InitializeComponent();
	}



    private void OnAddListButtonClicked(object sender, EventArgs e)
    {
        var listObject = new UserList();

        MenuManager menuManager = new MenuManager();
        UserList addedList = menuManager.AddUserList(listObject);

        menuManager.SaveChanges();

        Navigation.PushAsync(new ListDetails(addedList.id));
    }

    private void OnAddListItemButtonClicked(object sender, EventArgs e)
    {
        var itemObject = new UserListItem();

        MenuManager menuManager = new MenuManager();
        UserListItem addedItem = menuManager.AddUserListItem(itemObject);

        menuManager.SaveChanges();

        Navigation.PushAsync(new ListDetails(addedItem.id));
    }

}