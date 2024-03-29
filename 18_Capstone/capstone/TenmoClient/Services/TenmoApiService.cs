﻿using RestSharp;
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
        public List<ApiUser> GetUsers()
        {
            RestRequest request = new RestRequest("user");
            IRestResponse<List<ApiUser>> response = client.Get<List<ApiUser>>(request);
            CheckForError(response);
            return response.Data;
        }
        public ApiTransfer GetTransfer()
        {
            RestRequest request = new RestRequest("transfer");
            IRestResponse<ApiTransfer> response = client.Get<ApiTransfer>(request);
            CheckForError(response);
            return response.Data;
        }
        public ApiTransfer GetTransferById(int id)
        {
            RestRequest request = new RestRequest($"transfer/{id}");
            IRestResponse<ApiTransfer> response = client.Get<ApiTransfer>(request);
            CheckForError(response);
            return response.Data;
        }
        public List<ApiTransfer> GetTransfers()
        {
            RestRequest request = new RestRequest("transfer");
            IRestResponse<List<ApiTransfer>> response = client.Get<List<ApiTransfer>>(request);
            CheckForError(response);
            return response.Data;
        }
        public bool AddTransfer(ApiTransfer newTransfer)
        {
            RestRequest request = new RestRequest($"transfer");
            request.AddJsonBody(newTransfer);
            IRestResponse<bool> response = client.Post<bool>(request);
            CheckForError(response);
            return response.Data;
        }
        public ApiUser GetUserById(int id)
        {
            RestRequest request = new RestRequest($"user/{id}");
            IRestResponse<ApiUser> response = client.Get<ApiUser>(request);
            CheckForError(response);
            return response.Data;
        }
        public ApiUser GetCurrentUser()
        {
            RestRequest request = new RestRequest($"user/currentuser");
            IRestResponse<ApiUser> response = client.Get<ApiUser>(request);
            CheckForError(response);
            return response.Data;
        }
        public ApiAccount GetAccountById(int id)
        {
            RestRequest request = new RestRequest($"account/{id}");
            IRestResponse<ApiAccount> response = client.Get<ApiAccount>(request);
            CheckForError(response);
            return response.Data;
        }

        protected void CheckForError(IRestResponse<ApiTransfer> response)
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
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    message = $"An http error occurred. Status code {(int)response.StatusCode} {response.StatusDescription}";
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
