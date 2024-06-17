using System.Net;

namespace PlaygroundProject.ServicesResponse
{
    public abstract class ServiceResponseBase
    {
        public bool Success { get; set; } = true;
        public string ErrorMessage { get; set; }
        public Errors Error { get; set; }

    }
}
