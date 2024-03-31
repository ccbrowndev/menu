using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace menu.Data
{
    public class SharingService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public SharingService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        public async Task<string> ShareList(string shareCode, string senderID, string listID, string listData)
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

        public async Task<string> ReceiveSharedList(string shareCode, string senderID)
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

        public async Task<string> CheckQueue(string userID)
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

        public async Task<string> UpdateList(string senderID, string listID, string recipientIDList, string listData)
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
