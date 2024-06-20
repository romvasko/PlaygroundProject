using Microsoft.AspNetCore.Mvc;
using PlaygroundProject.Services.Interfaces;
using PlaygroundProject.ServicesResponse;
using PlaygroundProject.ViewModels;
using System.Net;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace PlaygroundProject.Services.Implementations
{
    public class UserService : IUserService
    {
        private const string IdentityUrl = "https://localhost:7094";
        public UserService() { }

        private async Task<JsonResult> GetUserInfoJson(string token)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = IdentityUrl + "/",
                Policy =
                {
                    RequireHttps = false
                }
            });

            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return null;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(token);

            var info = await apiClient.GetUserInfoAsync(new UserInfoRequest()
            {
                Address = disco.UserInfoEndpoint,
                Token = token,
            });

            if (string.IsNullOrEmpty(info.Json.ToString()))
            {
                return null;
            }
            return new JsonResult(info.Json);
        }

        public async Task<GetUserInfoResponse> GetUserInfo(string token)
        {
            UserViewModel user;
            var jsonUser = await GetUserInfoJson(token);

            if (jsonUser == null)
            {
                return new GetUserInfoResponse()
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessage = "identity_server_error"
                };
            }

            try
            {
                user = System.Text.Json.JsonSerializer.Deserialize<UserViewModel>(jsonUser.Value.ToString());
            }
            catch (Exception e)
            {
                return new GetUserInfoResponse()
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadGateway,
                    ErrorMessage = "user_mapping_error"
                };
            }

            if (user == null)
            {
                return new GetUserInfoResponse()
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadGateway,
                    ErrorMessage = "user_mapping_error"
                };
            }

            var customer = new CustomerViewModel()
            {
                Id = user.Sub,
                Email = user.Email,
                Role = user.Role
            };

            return new GetUserInfoResponse()
            {
                Success = true,
                Customer = customer
            };
        }

        public async Task<GetTokenResponse> GetToken(Roles role)
        {
            var client_id = "web-api";
            var grant_type = "password";
            var scope = "openid IdentityServerApi web-api profile offline_access";
            string username;
            string password;
            if(role == Roles.Admin)
            {
                username = "testAdmin@gmail.com";
                password = "Admin123*";
            } else
            {
                username = "testUser@gmail.com";
                password = "testUser123*";
            }

            var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("client_id", client_id),
        new KeyValuePair<string, string>("grant_type", grant_type),
        new KeyValuePair<string, string>("scope", scope),
        new KeyValuePair<string, string>("username", username),
        new KeyValuePair<string, string>("password", password)
    });

            var response = await client.PostAsync(IdentityUrl + "/connect/token", content);
            GetTokenResponse tokenResponce = new();
            if (!response.IsSuccessStatusCode)
            {
                tokenResponce.ErrorMessage = await response.Content.ReadAsStringAsync();
                tokenResponce.Success = false;
                return tokenResponce;
            }
            
            tokenResponce.TokenViewModel = JsonConvert.DeserializeObject<TokenViewModel>(await response.Content.ReadAsStringAsync());
            return tokenResponce;

        }
    }
}
