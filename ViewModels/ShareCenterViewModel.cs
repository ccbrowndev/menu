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
                UserList newList = ProcessAzureData(result.Result);
            }


        }

        private UserList ProcessAzureData(AzureList azureList)
        {
            try
            {
                AzureListData listData = JsonConvert.DeserializeObject<AzureListData>(azureList.ListData);
                string listName = listData.Name;
                List<AzureListItem> listItems = listData.AzureListItems;

                UserList newList = new()
                {
                    Name = listName,
                    UserUuid = azureList.OriginalSenderId,
                    IsInTrash = false,
                    ShareCode = ShareCode,
                    ListItems = new List<ListItem>()
                };

                int newListId = db.SaveUserList(newList);
                List<ListItem> outListItems = ProcessAzureListItems(newListId, listItems);

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

        private List<ListItem> ProcessAzureListItems(int dbListId, List<AzureListItem> azureListItems)
        {
            List<ListItem> listItems = new();
            foreach (AzureListItem ali in azureListItems)
            {
                ListItem li = new()
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
