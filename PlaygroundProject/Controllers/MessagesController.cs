using Microsoft.AspNetCore.Mvc;
using PlaygroundProject.ServicesResponse;

namespace PlaygroundProject.Controllers
{
    public class MessagesController : ControllerBase
    {
        private readonly IMessagesService _messagesService;
        public MessagesController(IMessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        public async Task<IActionResult> SendTestMessage()
        {
            var response = await _messagesService.SendTestMessage();
            return Ok();
        }
    }
}
