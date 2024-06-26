using PlaygroundProject.ServicesResponse;

namespace PlaygroundProject.Services.Interfaces
{
    public interface IMessagesService
    {
        public Task<MessageSentResultResponse> SendTestMessage(string testMessage);

    }
}
