using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using menu.Data;
using menu.Models;
using System.Collections.ObjectModel;

namespace menu.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        MenuDatabase db;

        public MainViewModel(MenuDatabase database)
        {
            db = database;
            IsVisible = false;
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            ListCollectionHeight = (displayInfo.Height / displayInfo.Density) * .25;
            ItemCollectionHeight = (displayInfo.Height / displayInfo.Density) * .5;

            LoadListData();
            SelectedList = ListCollection.FirstOrDefault();
            LoadItemData(SelectedList.Id);
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

        void LoadListData()
        {
            List<UserList> dbUserLists = db.GetUserLists();
            ListCollection = new ObservableCollection<UserList>(dbUserLists);
        }

        void LoadItemData(int listId)
        {
            List<ListItem> dbListItems = db.GetListItemsByListId(listId);
            Items = new ObservableCollection<ListItem>(dbListItems);
        }
        
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
            db.SaveListItem(newListItem);
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
                db.DeleteListItem(li);
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
                    db.SaveUserList(list);
                } else
                {
                    newListCollection.Add(list);
                }
            }

            ListCollection = newListCollection;
        }

        public void OnListCollectionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                UserList newSelectedList = e.CurrentSelection[0] as UserList;

                if (newSelectedList.ListItems == null || newSelectedList.ListItems.Count == 0)
                {
                    newSelectedList.ListItems = db.GetListItemsByListId(newSelectedList.Id);
                }
                Items = new ObservableCollection<ListItem>(newSelectedList.ListItems);
                ToggleListCollectionVisibility();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                SelectedList = ListCollection.FirstOrDefault();
            }
        }

        [RelayCommand]
        void AddList()
        {
            if (Items == null || Items.Count == 0)
                return;

            SelectedList.ListItems = Items.ToList();
            db.SaveUserList(SelectedList);

            UserList newList = new()
            {
                Name = "Test" + (ListCollection.Count + 1),
                ListItems = new List<ListItem>()
            };

            ListCollection.Add(newList);
            SelectedList = newList;
            Items = new ObservableCollection<ListItem>(SelectedList.ListItems);
        }
    }
}
