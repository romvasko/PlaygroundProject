using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PlaygroundProject.Filters
{
    public class RemoveBearerPrefixFromAuthorizationHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authAttributes = context.MethodInfo.GetCustomAttributes(true)
                .Union(context.MethodInfo.DeclaringType.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>();

            if (authAttributes.Any())
            {
                var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            };

                operation.Security = new List<OpenApiSecurityRequirement> { securityRequirement };
            }

            var authHeaderParameter = operation.Parameters?.FirstOrDefault(p => p.Name == "Authorization");

            if (authHeaderParameter != null)
            {
                authHeaderParameter.Description = "Authorization header. Example: \"Bearer {token}\"";
                authHeaderParameter.Schema.Description = "Authorization header. Example: \"Bearer {token}\"";
                authHeaderParameter.Schema.Default = new OpenApiString("Bearer ");
            }
        }
    }
}
