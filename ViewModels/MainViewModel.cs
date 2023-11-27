using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using menu.Models;
using System.Collections.ObjectModel;

namespace menu.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        readonly UserList defaultUserList = new()
        {
            Id = 1,
            Name = "Welcome",
            ListItems = new List<ListItem>()
        };

        public MainViewModel()
        {
            IsVisible = false;
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            ListCollectionHeight = (displayInfo.Height / displayInfo.Density) * .25;
            ItemCollectionHeight = (displayInfo.Height / displayInfo.Density) * .5;

            ListCollection = new ObservableCollection<UserList>
            {
                defaultUserList
            };
            SelectedList = defaultUserList;
            Items = new ObservableCollection<ListItem>();
        }

        [ObservableProperty]
        double listCollectionHeight;

        [ObservableProperty]
        bool isVisible;

        [ObservableProperty]
        double itemCollectionHeight;


        [ObservableProperty]
        ObservableCollection<UserList> listCollection;

        [ObservableProperty]
        ObservableCollection<ListItem> items;

        [ObservableProperty]
        UserList selectedList;

        [ObservableProperty]
        bool inputCompleted;

        [ObservableProperty]
        string text;

        [RelayCommand]
        void ToggleListCollectionVisibility()
        {
            IsVisible = !IsVisible;
        }

        [RelayCommand]
        void Add()
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;

            ListItem newListItem = new()
            {
                UserListId = SelectedList.Id,
                Text = Text,
                IsComplete = InputCompleted
            };

            Items.Add(newListItem);
            SelectedList.ListItems = Items.ToList();
            Text = string.Empty;
            InputCompleted = false;
        }

        [RelayCommand]
        void Delete(ListItem li)
        {
            if (Items.Contains(li))
            {
                Items.Remove(li);
                SelectedList.ListItems = Items.ToList();
            }
        }

        [RelayCommand]
        void UpdateTitle()
        {
            ObservableCollection<UserList> currentListCollection = ListCollection;
            ObservableCollection<UserList> newListCollection = new();

            foreach (UserList list in currentListCollection)
            {
                if (list.Id == SelectedList.Id)
                {
                    list.Name = SelectedList.Name;
                    newListCollection.Add(list);
                } else
                {
                    newListCollection.Add(list);
                }
            }

            ListCollection = newListCollection;
        }

        [RelayCommand]
        void SelectedListChanged()
        {
            if (SelectedList.ListItems == null || SelectedList.ListItems.Count == 0)
            {
                Items.Clear();
                return;
            }
            Items = new ObservableCollection<ListItem>(SelectedList.ListItems);
            ToggleListCollectionVisibility();
        }

        [RelayCommand]
        void AddList()
        {
            if (Items == null || Items.Count == 0)
                return;

            SelectedList.ListItems = Items.ToList();
            int newId = SelectedList.Id + 1;

            UserList newList = new()
            {
                Id = newId,
                Name = "Test" + newId,
                ListItems = new List<ListItem>()
            };

            ListCollection.Add(newList);
            SelectedList = newList;
            Items = new ObservableCollection<ListItem>(SelectedList.ListItems);
        }
    }
}
