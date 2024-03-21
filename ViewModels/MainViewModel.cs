using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using menu.Data;
using menu.Models;
using System.Collections.ObjectModel;
using menu;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

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

            ListCollection = new ObservableCollection<UserList>(db.GetUserLists());
            SelectedList = ListCollection.FirstOrDefault();
            Items = new ObservableCollection<ListItem>(db.GetListItemsByListId(SelectedList.Id));
            TrashUserLists = new ObservableCollection<UserList>(db.GetTrashUserLists());
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

        [ObservableProperty]
        DateTime deadline = DateTime.Now.AddDays(7);

        [ObservableProperty]
        ObservableCollection<UserList> trashUserLists;

        [ObservableProperty]
        ObservableCollection<UserList> selectedTrashItems = new ObservableCollection<UserList>();



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

            if (Items == null || Items.Count == 0)
            {
                SelectedList.Id = db.SaveUserList(SelectedList);
            }

            ListItem newListItem = new()
            {
                UserListId = SelectedList.Id,
                Text = Text,
                IsComplete = InputCompleted
            };

            Items.Add(newListItem);
            db.SaveListItem(newListItem);
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
                    list.Deadline = SelectedList.Deadline;
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

                Items = new ObservableCollection<ListItem>(db.GetListItemsByListId(SelectedList.Id));

                IsVisible = false;
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

            db.SaveUserList(SelectedList);

            UserList newList = new()
            {
                Name = "New List " + (ListCollection.Count + 1),
                ListItems = new List<ListItem>(),
                Deadline = Deadline,
            };

            ListCollection.Add(newList);
            SelectedList = newList;
            Items = new ObservableCollection<ListItem>();
        }

        [RelayCommand]
        void SaveList()
        {
            if (Items == null || Items.Count == 0)
                return;

            SelectedList.ListItems = Items.ToList();
            db.SaveUserList(SelectedList);

        }

        [RelayCommand]
        void MoveToTrash(UserList list)
        {
            list.IsInTrash = true;
            db.SaveUserList(list);
            ListCollection.Remove(list);
        }

        [RelayCommand]
        void RecoverSelectedLists()
        {
            if (SelectedTrashItems != null && SelectedTrashItems.Count > 0)
            {
                foreach (var list in SelectedTrashItems.ToList())
                {
                    db.RestoreFromTrash(list);
                }
                RefreshTrashList(); 
                SelectedTrashItems.Clear(); 
            }
        }


        [RelayCommand]
        void DeleteSelectedListsForever()
        {
            if (SelectedTrashItems != null && SelectedTrashItems.Count > 0)
            {
                foreach (var list in SelectedTrashItems.ToList())
                {
                    db.DeleteUserListPermanently(list);
                }
                RefreshTrashList(); 
                SelectedTrashItems.Clear();
            }
        }


        public void RefreshTrashList()
        {
            TrashUserLists = new ObservableCollection<UserList>(db.GetTrashUserLists());
        }

        [RelayCommand]

        async Task DeleteList()
        {
            if (Items == null) return;

            bool isConfirmed = await Shell.Current.DisplayAlert(
                "Confirm Delete",
                $"Are you sure you want to delete '{SelectedList.Name}'?",
                "Yes", "No");

            if (isConfirmed)
            {
                db.MoveToTrash(SelectedList);
                ListCollection.Remove(SelectedList);

                var welcomeList = ListCollection.FirstOrDefault(l => l.Name == "Welcome") ?? ListCollection.FirstOrDefault();
                if (welcomeList != null)
                {
                    SelectedList = welcomeList;
                    Items = new ObservableCollection<ListItem>(db.GetListItemsByListId(SelectedList.Id));
                }
            }
        }



        public async Task CheckDeadlinesAsync()
        {
            var listsWithTodaysDeadline = GetListsWithTodaysDeadline();
            if (listsWithTodaysDeadline.Any())
            {
                await Shell.Current.DisplayAlert("Deadline Today", "You have list whose deadline is today.", "OK");
            }
        }

        public List<UserList> GetListsWithTodaysDeadline()
        {
            var today = DateTime.Now.Date;
            return ListCollection.Where(list => list.Deadline.Date == today).ToList();
        }

    }
}
