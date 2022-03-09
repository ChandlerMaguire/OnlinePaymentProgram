using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using TenmoClient.Models;

namespace TenmoClient.Services
{
    public class TenmoApiService : AuthenticatedApiService
    {
        public TenmoApiService(string apiUrl) : base(apiUrl) { }

        // Add methods to call api here...
        public ApiAccount GetAccount()
        {
            RestRequest request = new RestRequest($"account");
            IRestResponse<ApiAccount> response = client.Get<ApiAccount>(request);
            CheckForError(response);
            return response.Data;
        }

        protected void CheckForError(IRestResponse response)
        {
            string message;
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                message = $"Error occurred - unable to reach server. Response status was '{response.ResponseStatus}'.";
                throw new HttpRequestException(message, response.ErrorException);
            }
            else if (!response.IsSuccessful)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    message = $"Authorization is required and the user has not logged in.";
                }
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    message = $"The user does not have permission.";
                }
                else
                {
                    message = $"An http error occurred. Status code {(int)response.StatusCode} {response.StatusDescription}";
                }
                throw new HttpRequestException(message, response.ErrorException);
            }
        }

    }
}
