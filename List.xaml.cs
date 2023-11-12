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
        LoadPickerData();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadPickerData();
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
        inputListName.Text = "";
    }

    private void InputListName_Unfocused(object sender, FocusEventArgs e)
    {
        SaveData(inputListName.Text);
    }

    private void SaveData(string text)
    {
        // 实现数据保存逻辑
        // 例如，保存到本地数据库或发送到服务器
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