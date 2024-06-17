using PlaygroundProject.ServicesResponse;
using System.Text;

namespace PlaygroundProject.Middlewares
{
    public class UnauthorisedMessageSwap
    {
        private readonly RequestDelegate _next;

        public UnauthorisedMessageSwap(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;




            await _next(context);

            var status = context.Response.StatusCode;

            var response = new UnauthorizedResponse();

            if (status == 401)
            {
                context.Response.Clear();
                context.Response.StatusCode = status;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(response.Message);
            }


        }
    }
}
