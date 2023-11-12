using menu.Domain;
using menu.Services;
using Microsoft.Maui.Controls;

namespace menu;

public partial class ListDetails : ContentPage
{
    private int listId;

    public ListDetails(int id)
	{
		InitializeComponent();
        this.listId = id;
    }

    
    

    protected override void OnAppearing()
    {
        base.OnAppearing();

        MenuManager menuManager = new MenuManager();
        UserList list = menuManager.GetListByid(listId);

        if (list != null && !string.IsNullOrWhiteSpace(list.name))
        {
            inputListName.Text = list.name;
        }
        else
        {
            inputListName.Text = "ListName"; 
        }

        var items = menuManager.GetItemsByListid(listId);
        itemsCollectionView.ItemsSource = items;
    }

    private void OnAddListButtonClicked(object sender, EventArgs e)
    {
        var listObject = new UserList();

        MenuManager menuManager = new MenuManager();
        UserList addedList = menuManager.AddUserList(listObject);

        menuManager.SaveChanges();

        Navigation.PushAsync(new ListDetails(addedList.id));
    }

    private void OnUpdateListNameButtonClicked(object sender, EventArgs e) 
    {
        string listName = inputListName.Text;

        MenuManager menuManager = new MenuManager();
        UserList list = menuManager.GetListByid(listId);

        list.name = listName;

        menuManager.UpdateUserList(list);

        menuManager.SaveChanges();
    }

    private void OnAddListItemButtonClicked(object sender, EventArgs e)
    {
        var itemObject = new UserListItem();

        MenuManager menuManager = new MenuManager();
        UserListItem addedItem = menuManager.AddUserListItem(itemObject);

        menuManager.SaveChanges();

        Navigation.PushAsync(new AddNewItem(listId,addedItem.id));
    }

}