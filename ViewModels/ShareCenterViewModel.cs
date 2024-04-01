using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using menu.Data;
using menu.Models;
using Newtonsoft.Json;

namespace menu.ViewModels
{
    public partial class ShareCenterViewModel : ObservableObject
    {
        private readonly MenuDatabase db;
        private readonly SharingService sharingService;

        public ShareCenterViewModel(MenuDatabase db, SharingService sharingService)
        {
            this.db = db;
            this.sharingService = sharingService;
        }

        [ObservableProperty]
        string shareCode;

        [RelayCommand]
        void ReceiveSharedList()
        {
            User user = db.GetDefaultUser();

            var result = sharingService.ReceiveSharedList(user.Uuid, ShareCode);

            if (result.IsCompletedSuccessfully)
            {
                UserList newList = processAzureData(result.Result);
            }


        }

        private UserList processAzureData(AzureList azureList)
        {
            try
            {
                AzureListData listData = JsonConvert.DeserializeObject<AzureListData>(azureList.ListData);
                string listName = listData.Name;
                List<AzureListItem> listItems = listData.AzureListItems;

                UserList newList = new UserList()
                {
                    Name = listName,
                    UserUuid = azureList.OriginalSenderId,
                    IsInTrash = false,
                    ShareCode = ShareCode,
                    ListItems = new List<ListItem>()
                };

                int newListId = db.SaveUserList(newList);
                List<ListItem> outListItems = processAzureListItems(newListId, listItems);

                newList.ListItems = outListItems;
                return newList;
            }
            catch (Exception ex)
            {
                if (ex is JsonSerializationException)
                {
                    Console.WriteLine("Failure to deserialize the Azure List Data: " + ex.Message);
                }
                else
                {
                    Console.WriteLine(ex.Message);
                }
                return null;
            }
        }

        private List<ListItem> processAzureListItems(int dbListId, List<AzureListItem> azureListItems)
        {
            List<ListItem> listItems = new List<ListItem>();
            foreach (AzureListItem ali in azureListItems)
            {
                ListItem li = new ListItem()
                {
                    Text = ali.Text,
                    IsComplete = ali.IsComplete,
                    UserListId = dbListId
                };

                int liId = db.SaveListItem(li);
                listItems.Add(db.GetListItemById(liId));
            }

            return listItems;
        }
    }
}
