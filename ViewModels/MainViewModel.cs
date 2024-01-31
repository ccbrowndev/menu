using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using menu.Models;
using System.Collections.ObjectModel;
using Microsoft.Maui.ApplicationModel;

namespace menu.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        readonly UserList defaultUserList = new()
        {
            Id = 1,
            Name = "Welcome",
            ListItems = new()
            {
                new() { Id=1, UserListId=1, Text="Tap \"Welcome\" to change title.", IsComplete=false},
                new() { Id=2, UserListId=1, Text="Type into placeholder to add an item.", IsComplete=false},
                new() { Id=3, UserListId=1, Text="Tap the Add List button to add a list.", IsComplete = false},
                new() { Id=4, UserListId=1, Text="Swipe to the left to access the delete button.", IsComplete=false}
            } 
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
            Items = new ObservableCollection<ListItem>(defaultUserList.ListItems);
            Lists = new ObservableCollection<UserList>();
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
        ObservableCollection<UserList> lists;

        [ObservableProperty]
        UserList selectedList;

        [ObservableProperty]
        bool inputCompleted;

        [ObservableProperty]
        string text;

        [ObservableProperty]
        DateTime deadline = DateTime.Now.AddDays(7);

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
                ListItems = new List<ListItem>(),
                Deadline = Deadline
            };

            ListCollection.Add(newList);
            SelectedList = newList;
            Items = new ObservableCollection<ListItem>(SelectedList.ListItems);

            //ScheduleNotificationForItem(newList);
        }

<<<<<<< Updated upstream
        //[RelayCommand]
        //public void ScheduleNotificationForItem(UserList list)
        //{
        //    if (list.Deadline > DateTime.Now)
        //    {
        //        var notification = new NotificationRequest
        //        {
        //            Title = "Deadline Reminder",
        //            Description = $"Your item \"{list.Name}\" is due!",
        //            Schedule = new NotificationSchedule(list.Deadline.Value)
        //        };

        //        NotificationCenter.Current.Schedule(notification);
        //    }
        //}

=======
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
            return Lists.Where(list => list.Deadline.Date == today).ToList();
        }
>>>>>>> Stashed changes
    }
}
