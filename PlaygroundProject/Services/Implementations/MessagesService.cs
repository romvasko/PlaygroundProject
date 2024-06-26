using PlaygroundProject.Services.Interfaces;
using PlaygroundProject.ServicesResponse;

namespace PlaygroundProject.Services.Implementations
{
    public class MessagesService : IMessagesService
    {
        public async Task<MessageSentResultResponse> SendTestMessage(string testMessage)
        {
            var response = new MessageSentResultResponse();

            return response;
        }
    }
}
