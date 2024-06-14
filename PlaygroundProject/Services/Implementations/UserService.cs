using Newtonsoft.Json;
using PlaygroundProject.Services.Interfaces;
using PlaygroundProject.ServicesResponce;
using PlaygroundProject.ViewModels;

namespace PlaygroundProject.Services.Implementations
{
    public class UserService : IUserService
    {
        private const string IdentityUrl = "https://localhost:7094";
        public UserService() { }

        public async Task<GetTokenResponce> GetToken(Roles role)
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
            GetTokenResponce tokenResponce = new();
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
