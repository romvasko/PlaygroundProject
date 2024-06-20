using PlaygroundProject.ViewModels;
using System.Net;

namespace PlaygroundProject.ServicesResponse
{
    public class GetUserInfoResponse : ServiceResponseBase
    {
        public HttpStatusCode StatusCode { get; set; }
        public CustomerViewModel Customer { get; set; }
    }
}
