using menu.Domain;
using menu.Services;
using System.Collections.Generic;

namespace menu;

public partial class List : ContentPage
{
    public List()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        MenuManager menuManager = new MenuManager();

        var lists = menuManager.GetAllLists();
        listsCollectionView.ItemsSource = lists; 
    }

    private void OnEmptyRecycleBinClicked(object sender, EventArgs e)
    {
        MenuManager menuManager = new MenuManager();
        menuManager.EmptyRecycleBin();
    }



    private void OnAddListButtonClicked(object sender, EventArgs e)
    {
        String listName = inputListName.Text;
        var listObject = new UserList
        {
            name = listName,
        };

        MenuManager menuManager = new MenuManager();
        var addedList = menuManager.AddUserList(listObject);

        menuManager.SaveChanges();

        Navigation.PushAsync(new ListDetails(addedList.id));
    }


}