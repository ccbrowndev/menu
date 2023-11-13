using menu.Domain;
using menu.Services;
using System.Collections.ObjectModel;

namespace menu;

public partial class List : ContentPage
{
    private MenuManager menuManager = new MenuManager();
    public ObservableCollection<UserList> UserLists { get; set; }

    public bool IsButtonVisible { get; set; }
    private int listId;
    public List() : this(1)
    {


    }
    public List(int listId)
    {
        InitializeComponent();
        LoadPickerData();
        IsButtonVisible = false;
        this.listId = listId;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadPickerData();
        LoadCollectionViewData(listId);

        UserList list = menuManager.GetListByid(listId);

        if (list != null && !string.IsNullOrWhiteSpace(list.name))
        {
            inputListName.Text = list.name;
        }
        else
        {
            inputListName.Text = "ListName";
        }
    }


    private void LoadPickerData()
    {
        var allLists = menuManager.GetAllLists();
        listsPicker.ItemsSource = allLists;
    }

    private void LoadCollectionViewData(int selectedList)
    {
        var listItems = menuManager.GetItemsByListid(selectedList);
        listCollectionView.ItemsSource = listItems;
    }

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedList = (UserList)picker.SelectedItem;
        LoadCollectionViewData(selectedList.id);

        if (selectedList != null && selectedList.id > 1)
        {
            Navigation.PushAsync(new List(selectedList.id));
        }
        else if(selectedList != null)
        {
            Navigation.PushAsync(new List(1));
        }
    }


    private void OnEmptyRecycleBinClicked(object sender, EventArgs e)
        {
            menuManager.EmptyRecycleBin();
        }

    /*
    private void InputListName_Focused(object sender, FocusEventArgs e)
    {
        inputListName.Text = "";
    }

    private void InputListName_Unfocused(object sender, FocusEventArgs e)
    {
        SaveData(inputListName.Text);
    }

    private void SaveData(string text)
    {
        string listName = inputListName.Text;

        UserList list = menuManager.GetListByid(listId);

        list.name = listName;

        menuManager.UpdateUserList(list);

        menuManager.SaveChanges();
    }
    */


    private void OnAddListButtonClicked(object sender, EventArgs e)
    {
        /*String listName = inputListName.Text;
        var listObject = new UserList
        {
            name = listName,
        };

        var addedList = menuManager.AddUserList(listObject);

        menuManager.SaveChanges();
        */
        Navigation.PushAsync(new List());
    }
    private void OnUpdateListNameButtonClicked(object sender, EventArgs e)
    {
        string listName = inputListName.Text;

        MenuManager menuManager = new MenuManager();
        UserList list;

        if (listId == 1)
        {
            list = new UserList()
            {
                name = listName
            };
            menuManager.AddUserList(list);
        }
        else
        {
            list = menuManager.GetListByid(listId);
            if (list != null)
            {
                list.name = listName;
                menuManager.UpdateUserList(list);
            }
            else
            {
            }
        }
        menuManager.SaveChanges();
        LoadPickerData();
    }


    private void OnAddListItemButtonClicked(object sender, EventArgs e)
    {
        var itemObject = new UserListItem();

        MenuManager menuManager = new MenuManager();
        UserListItem addedItem = menuManager.AddUserListItem(itemObject);

        menuManager.SaveChanges();

        Navigation.PushAsync(new AddNewItem(listId, addedItem.id));
    }
}