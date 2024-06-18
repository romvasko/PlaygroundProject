using PlaygroundProject.ServicesResponse;
using PlaygroundProject.ServicesResponse.StatusResonse;
using System.Text;

namespace PlaygroundProject.Middlewares
{
    public class MessageSwapBasedOnStatusCode
    {
        private readonly RequestDelegate _next;

        public MessageSwapBasedOnStatusCode(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;




            await _next(context);

            var status = context.Response.StatusCode;


            if (status == 401)
            {
                var response = new UnauthorizedResponse();
                context.Response.Clear();
                context.Response.StatusCode = status;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(response.Message);
            }

            if (status == 403)
            {
                var response = new AccessDeniedResponse();
                context.Response.Clear();
                context.Response.StatusCode = status;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(response.Message);
            }

        }
    }
}
