using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using menu.Data;
using menu.Models;
using System.Collections.ObjectModel;

namespace menu.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MenuDatabase db;
        private readonly SharingService sharingService;

        public MainViewModel(MenuDatabase database, SharingService sharingService)
        {
            db = database;
            this.sharingService = sharingService;
            IsVisible = false;
            var displayInfo = DeviceDisplay.MainDisplayInfo;

            ListCollectionHeight = (displayInfo.Height / displayInfo.Density) * .25;
            ItemCollectionHeight = (displayInfo.Height / displayInfo.Density) * .5;

            ListCollection = new ObservableCollection<UserList>(db.GetUserLists());
            SelectedList = ListCollection.FirstOrDefault();
            Items = new ObservableCollection<ListItem>(db.GetListItemsByListId(SelectedList.Id));

            TrashListCollection = new ObservableCollection<UserList>(db.GetTrashUserLists());
            SelectedTrashList = TrashListCollection.FirstOrDefault();
            TrashUserLists = new ObservableCollection<UserList>(db.GetTrashLists());
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
        ObservableCollection<UserList> trashListCollection;

        [ObservableProperty]
        ObservableCollection<ListItem> items;

        [ObservableProperty]
        ObservableCollection<UserList> trashUserLists;

        [ObservableProperty]
        UserList selectedTrashList;

        [ObservableProperty]
        UserList selectedList;

        [ObservableProperty]
        bool inputCompleted;

        [ObservableProperty]
        string text;

        [ObservableProperty]
        DateTime deadline = DateTime.Now.AddDays(7);

        [ObservableProperty]
        private string shareCode;

        [ObservableProperty]
        string sharedCode;

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
                    list.IsInTrash = SelectedList.IsInTrash;
                    newListCollection.Add(list);
                    db.SaveUserList(list);
                }
                else
                {
                    newListCollection.Add(list);
                }
            }

            ListCollection = newListCollection;
        }

        [RelayCommand]
        void GetShareCode()
        {
            ObservableCollection<UserList> currentListCollection = ListCollection;
            ObservableCollection<UserList> newListCollection = new();

            foreach (UserList list in currentListCollection)
            {
                list.Name = SelectedList.Name;
                list.Deadline = SelectedList.Deadline;
                list.IsInTrash = SelectedList.IsInTrash;
                list.ShareCode = SelectedList.ShareCode;
                newListCollection.Add(list);
                db.SaveUserList(list);
                
            }

            ListCollection = newListCollection;
        }


        
        [RelayCommand]
        async Task RetrieveListByShareCode(string code)
        {
            var list = db.GetUserLists().FirstOrDefault(l => l.ShareCode == code);
            if (list == null)
            {
                await Shell.Current.DisplayAlert("Error", "Invalid share code.", "OK");
                return;
            }

            SelectedList = list;
            Items = new ObservableCollection<ListItem>(db.GetListItemsByListId(SelectedList.Id));
        }


        public void OnListCollectionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                UserList newSelectedList = e.CurrentSelection[0] as UserList;

                Items = new ObservableCollection<ListItem>(db.GetListItemsByListId(SelectedList.Id));

                IsVisible = false;
            }
            catch (Exception ex)
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
                IsInTrash = false,
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
        void RecoverSelectedLists(UserList li)
        {
            if (li != null)
            {
                db.RestoreFromTrash(li);
                RefreshTrashList();
            }
        }

        [RelayCommand]
        void SaveShareCode(UserList li)
        {
            li = SelectedList;
            if(li != null)
            {
                var random = new Random();
                string code;
                bool isUnique;

                do
                {
                    code = new string(Enumerable.Repeat("abcdefghijklmnopqrstuvwxyz0123456789", 10)
                      .Select(s => s[random.Next(s.Length)]).ToArray());

                    isUnique = !db.GetUserLists().Any(l => l.ShareCode == code);
                } while (!isUnique);

                li.ShareCode = code;
                db.SaveUserList(li);
            }
        }


        [RelayCommand]
        void DeleteSelectedListsForever()
        {
            db.DeleteUserListPermanently();
            RefreshTrashList();
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
            db.SaveUserList(SelectedList);
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

        [RelayCommand]
        public async Task GenerateShareCode()
        {
            var random = new Random();
            string code;
            bool isUnique;

            do
            {
                code = new string(Enumerable.Repeat("abcdefghijklmopqrstuvwxyz0123456789", 10)
                  .Select(s => s[random.Next(s.Length)]).ToArray());

                isUnique = !db.GetUserLists().Any(l => l.ShareCode == code);
            } while (!isUnique);

            SelectedList.ShareCode = code;
            db.SaveUserList(SelectedList); // 确保更新数据库中的列表

            User self = db.GetDefaultUser();
            await sharingService.ShareList(self, code, SelectedList);
        }


    }
}
