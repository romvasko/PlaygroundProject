using System.Text.Json.Serialization;

namespace PlaygroundProject.ViewModels
{
    public class UserViewModel
    {
        [JsonPropertyName("sub")]
        public string Sub { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("role")]
        public string Role { get; set; }
    }
}
