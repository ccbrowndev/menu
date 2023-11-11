using menu.Domain;
using menu.Services;

namespace menu;

public partial class List : ContentPage
{
    public List()
    {
        InitializeComponent();
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