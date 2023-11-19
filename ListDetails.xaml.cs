using menu.Domain;
using menu.Services;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;


namespace menu;

public partial class ListDetails : ContentPage
{
    private MenuManager menuManager = new MenuManager();
    public ObservableCollection<UserList> UserLists { get; set; }

    private readonly List<UserListItem> items;


    public bool IsButtonVisible { get; set; }
    private int listId;
    public ListDetails() : this(1)
    {

    }
    public ListDetails(int listId)
	{
        InitializeComponent();
        IsButtonVisible = false;
        this.listId = listId;


        this.items = menuManager.GetItemsByListid(listId);

        FillCalendar(listId);
    }


    private void FillCalendar(int _listId)
    {
        // ???????
        calendarGrid.Children.Clear();

        for (int i = 0; i < 12; i++)
        {
            List<UserListItem> _items = items.Where(s => s.UserListid == _listId).ToList();

            foreach (var item in _items)
            {
                Button _itemButton = new Button
                {
                    Text = item.name,
                };

                Grid.SetRow(_itemButton, i + 1);

                int itemId = item.id;
                _itemButton.Clicked += OnScheduleButtonClicked;

                calendarGrid.Children.Add(_itemButton);
            }
        }
    }

    private void OnScheduleButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            int itemId = (int)button.CommandParameter;
            // ?? itemId
        }
    }


}