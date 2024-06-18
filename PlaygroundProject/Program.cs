using Microsoft.OpenApi.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PlaygroundProject.Filters;
using PlaygroundProject.Services.Interfaces;
using PlaygroundProject.Services.Implementations;
using Microsoft.IdentityModel.Tokens;
using PlaygroundProject.Controllers;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using PlaygroundProject.Middlewares;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "web-api",
        Version = "v1",
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        BearerFormat = "JWT",
        Scheme = "bearer",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "JWT Authorization header using the Bearer scheme. And enter only your token",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                },
                Scheme="oauth2",
                Name="Bearer",
                In=ParameterLocation.Header
            },
            new List<string>()
        }
    });

    c.OperationFilter<RemoveBearerPrefixFromAuthorizationHeaderFilter>();
    c.SchemaFilter<EnumSchemaFilter>();
});

builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("RolesEnum", typeof(RouteConstraint));
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {


        options.Authority = "https://localhost:7094/";




        options.RequireHttpsMetadata = false;


        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateLifetime = false,
            ValidateAudience = false
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notifications"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<MessageSwapBasedOnStatusCode>();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();





app.Run();

