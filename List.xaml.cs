using menu.Domain;
using menu.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace menu;

public partial class List : ContentPage
{
    private MenuManager menuManager = new MenuManager();
    public ObservableCollection<UserList> UserLists { get; set; }
    public List()
    {
        InitializeComponent();
        UserLists = new ObservableCollection<UserList>();
        userListsView.ItemsSource = UserLists;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        // clean ond data
        UserLists.Clear();
        // load new
        var lists = menuManager.GetAllLists();
        foreach (var list in lists)
        {
            UserLists.Add(list);
        }
    }

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is UserList selectedList)
        {
            Navigation.PushAsync(new ListDetails(selectedList.id));
        }

        ((ListView)sender).SelectedItem = null;
    }

    private void TextCell_Tapped(object sender, EventArgs e)
    {
        // If there is data inserted into the list, change this row to a clickable button
    }

    private void OnEmptyRecycleBinClicked(object sender, EventArgs e)
    {
        menuManager.EmptyRecycleBin();
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

        Navigation.PushAsync(new ListDetails(addedList.id));
    }


}