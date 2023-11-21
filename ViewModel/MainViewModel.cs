using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using menu.Model;
using System.Collections.ObjectModel;

namespace menu.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel() 
        {
            ListsVisible = false;
            Lists = new ObservableCollection<UserList>();
            Items = new ObservableCollection<UserListItem>();
            ActiveList = new UserList()
            {
                Id = 1
            };
            Lists.Add(ActiveList);
        }

        [ObservableProperty]
        bool listsVisible;

        [ObservableProperty]
        ObservableCollection<UserList> lists;

        [ObservableProperty]
        UserList activeList;

        [ObservableProperty]
        ObservableCollection<UserListItem> items;

        [ObservableProperty]
        string text;

        [ObservableProperty]
        bool completed;

        [RelayCommand]
        void AddItem()
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;

            UserListItem newItem = new()
            {
                Text = Text,
                Completed = Completed
            };

            Items.Add(newItem);
            Text = String.Empty;
            ActiveList.ListItems = Items.ToList<UserListItem>();
        }

        [RelayCommand]
        void DeleteItem(UserListItem item)
        {
            if (Items.Contains(item))
            {
                Items.Remove(item);
                ActiveList.ListItems = Items.ToList<UserListItem>();
            }
        }

        [RelayCommand]
        void AddList()
        {
            //Save items on existing list
            ActiveList.ListItems = Items.ToList<UserListItem>();
            int activeListID = ActiveList.Id;
            ActiveList = new UserList()
            {
                Id = activeListID + 1
            };
            Items = new ObservableCollection<UserListItem>();
            Lists.Add(ActiveList);
        }

        [RelayCommand]
        void ToggleListView()
        {
            ListsVisible = !ListsVisible;
        }

        [RelayCommand]
        void LoadList()
        {
            if (ActiveList.ListItems == null)
            {
                Items = new ObservableCollection<UserListItem>();
                return;
            }

            Items = new ObservableCollection<UserListItem>(ActiveList.ListItems);
        }
    }
}
