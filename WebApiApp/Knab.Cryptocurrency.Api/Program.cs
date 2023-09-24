using AspNetCoreRateLimit;
using Knab.Cryptocurrency.Api.Security.Authentication;
using Knab.Cryptocurrency.Api.Security.CORS;
using Knab.Cryptocurrency.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddCors(options =>
{
    options.AddPolicy(name: CorsPolicyConstant.PolicyName, policyBuilder =>
    {
        policyBuilder.WithOrigins(CorsPolicyConstant.AllowOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

if(configuration.GetSection("Authentication:NeedAuthentication").Value?.ToLower() is "true")
{
    services.AddControllers(options =>
        options.Filters.Add<ApiKeyAuthenticationFilter>()); 
}
else
{
    services.AddControllers();
}

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey",
        new OpenApiSecurityScheme
        {
            Description = "The Api Key to access The API",
            In = ParameterLocation.Header,
            Name = "x-api-key",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "ApiKeyScheme"
        });
    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            scheme,
            new List<string>()
        }
    });
});
services.AddSettings(configuration);
services.AddInfrastructure(configuration);
services.AddRateLimiter(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseIpRateLimiting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors(CorsPolicyConstant.PolicyName);
app.MapControllers();

app.Run();