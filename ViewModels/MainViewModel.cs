using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using menu.Data;
using menu.Models;
using System.Collections.ObjectModel;
using menu;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace menu.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        MenuDatabase db;
        // 新增一个默认构造函数
        public MainViewModel()
        {
            db = new MenuDatabase(); // 或者从依赖注入容器中获取实例
            Initialize();
        }

        private void Initialize()
        {
            IsVisible = false;

            // 获取显示信息并计算列表和项的高度
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            ListCollectionHeight = (displayInfo.Height / displayInfo.Density) * .25;
            ItemCollectionHeight = (displayInfo.Height / displayInfo.Density) * .5;

            // 加载用户列表和第一个列表的项
            ListCollection = new ObservableCollection<UserList>(db.GetUserLists());
            SelectedList = ListCollection.FirstOrDefault();

            if (SelectedList != null)
            {
                Items = new ObservableCollection<ListItem>(db.GetListItemsByListId(SelectedList.Id));
            }

            // 设置其他需要在构造函数中初始化的属性
            Deadline = DateTime.Now.AddDays(7);
        }

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

        //新
        [ObservableProperty]
        private string shareCode;

        [ObservableProperty]
        string sharedCode;


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

        //新 添加两个新的命令：一个用于生成并显示分享代码，另一个用于通过分享代码检索列表。

        [RelayCommand]
        void ShareList()
        {
            if (SelectedList == null)
                return;

            var code = GenerateShareCode(SelectedList);
            Shell.Current.DisplayAlert("Share Code", $"Your share code: {code}", "OK");
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

        //新
        [RelayCommand]
        void GenerateShareCode()
        {
            ShareCode = GenerateShareCode(SelectedList);
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

        //新
        public string GenerateShareCode(UserList list)
        {
            var random = new Random();
            string code;
            bool isUnique;

            do
            {
                code = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                  .Select(s => s[random.Next(s.Length)]).ToArray());

                isUnique = !db.GetUserLists().Any(l => l.ShareCode == code);
            } while (!isUnique);

            list.ShareCode = code;
            db.SaveUserList(list); // 确保更新数据库中的列表

            return code;
        }




    }
}
