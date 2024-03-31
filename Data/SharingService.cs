using menu.Models;

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
    }
}
