using menu.Domain;
using menu.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace menu;

public partial class List : ContentPage
{
    private MenuManager menuManager = new MenuManager();
    public ObservableCollection<UserList> UserLists { get; set; }
    public bool IsButtonVisible { get; set; }
    private int listId;
    public List(int listId=1)
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
    }


    private void LoadPickerData()
    {
        var allLists = menuManager.GetAllLists();
        listsPicker.ItemsSource = allLists;
    }

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedList = (UserList)picker.SelectedItem;

        if (selectedList != null)
        {
            Navigation.PushAsync(new ListDetails(selectedList.id));
        }
    }


private void OnEmptyRecycleBinClicked(object sender, EventArgs e)
    {
        menuManager.EmptyRecycleBin();
    }

    private void InputListName_Focused(object sender, FocusEventArgs e)
    {
        IsButtonVisible = true;
        OnPropertyChanged(nameof(IsButtonVisible));
    }

    private void InputListName_Unfocused(object sender, FocusEventArgs e)
    {
        IsButtonVisible = false;
        OnPropertyChanged(nameof(IsButtonVisible));
    }

    private void OnAddListButtonClicked(object sender, EventArgs e)
    {
        String listName = inputListName.Text;
        var listObject = new UserList
        {
            name = listName,
        };

        var addedList = menuManager.AddUserList(listObject);

        menuManager.SaveChanges();

        Navigation.PushAsync(new List(addedList.id));
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

        Navigation.PushAsync(new AddNewItem(listId, addedItem.id));
    }
}