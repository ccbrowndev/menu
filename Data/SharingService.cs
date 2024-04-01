using menu.Models;
using Newtonsoft.Json;

namespace menu.Data
{
    public class SharingService
    {
        private readonly AzureFunctionService azureFunctionService;

        public SharingService(AzureFunctionService azureFunctionService)
        {
            this.azureFunctionService = azureFunctionService;
        }

        public async Task<bool> ShareList(User user, string shareCode, UserList list)
        {
            try
            {
                var result = await azureFunctionService.ShareListAzure(shareCode, user.Uuid, list.pid.ToString(), list.ToAzureString());
                if (result != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<AzureList> ReceiveSharedList(string userId, string shareCode)
        {
            try
            {
                var result = await azureFunctionService.ReceiveSharedListAzure(shareCode, userId);
                if (result != null)
                {
                    return JsonConvert.DeserializeObject<AzureList>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                if (ex is JsonSerializationException)
                {
                    Console.WriteLine("Failure to deserialize the result: " + ex.Message);
                    return null;
                }
                else
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public async Task<AzureListUpdates> CheckQueue(string userId)
        {
            try
            {
                var result = await azureFunctionService.CheckQueueAzure(userId);
                if (result != null)
                {
                    return JsonConvert.DeserializeObject<AzureListUpdates>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                if (ex is JsonSerializationException)
                {
                    Console.WriteLine("Failure to deserialize the result: " + ex.Message);
                    return null;
                }
                else
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public async Task<bool> UpdateList(string senderId, UserList list, string recipientIdList)
        {
            try
            {
                var result = await azureFunctionService.UpdateListAzure(senderId, list.pid.ToString(), recipientIdList, list.ToAzureString());
                if (result != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateList(string senderId, UserList list, List<string> recipientIdList)
        {
            try
            {
                var result = await azureFunctionService.UpdateListAzure(senderId, list.pid.ToString(), string.Join(",", recipientIdList), list.ToAzureString());
                if (result != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
