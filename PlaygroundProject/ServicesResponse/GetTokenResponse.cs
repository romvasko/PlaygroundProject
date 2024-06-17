using PlaygroundProject.ViewModels;

namespace PlaygroundProject.ServicesResponse
{
    public class GetTokenResponse : ServiceResponseBase
    {
        public TokenViewModel TokenViewModel { get; set; }
    }
}
