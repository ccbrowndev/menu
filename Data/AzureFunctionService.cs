using Microsoft.Extensions.Configuration;

namespace menu.Data
{
    public class AzureFunctionService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public AzureFunctionService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        public async Task<string> ShareListAzure(string shareCode, string senderID, string listID, string listData)
        {
            string shareListUrl = $"https://anonylistfunctions.azurewebsites.net/api/ShareList?shareCode={shareCode}&senderID={senderID}&listID={listID}&listData={listData}";
            string functionKey = configuration["SHARE_LIST_KEY"];

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, shareListUrl);
            request.Headers.Add("x-functions-key", functionKey);

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Error sharing list");
            }
        }

        public async Task<string> ReceiveSharedListAzure(string shareCode, string senderID)
        {
            string receiveSharedListUrl = $"https://anonylistfunctions.azurewebsites.net/api/ReceiveSharedList?shareCode={shareCode}&senderID={senderID}";
            string functionKey = configuration["RECEIVE_SHARED_LIST_KEY"];

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, receiveSharedListUrl);
            request.Headers.Add("x-functions-key", functionKey);

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Error receiving shared list");
            }
        }

        public async Task<string> CheckQueueAzure(string userID)
        {
            string checkQueueUrl = $"https://anonylistfunctions.azurewebsites.net/api/CheckQueue?userID={userID}";
            string functionKey = configuration["CHECK_QUEUE_KEY"];

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, checkQueueUrl);
            request.Headers.Add("x-functions-key", functionKey);

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Error checking the queue for list updates");
            }
        }

        public async Task<string> UpdateListAzure(string senderID, string listID, string recipientIDList, string listData)
        {
            string updateListUrl = $"https://anonylistfunctions.azurewebsites.net/api/UpdateList?senderID={senderID}&listID={listID}&recipientIDList={recipientIDList}&listData={listData}";
            string functionKey = configuration["UPDATE_LIST_KEY"];

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, updateListUrl);
            request.Headers.Add("x-functions-key", functionKey);

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Error updating the list " + listID);
            }

        }
    }
}
