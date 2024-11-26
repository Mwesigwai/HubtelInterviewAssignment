using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Hubtel.Interview.Assignment.UserWallet.Extensions;
public static class SwaggerExtensions
{
    public static void AddSwaggerDocumentation(this IServiceCollection services)
    {
        var info = new OpenApiInfo{
        Title = "Hubtel Interview Assignment For User Wallet",
        Version = "v1",
        Description = "This api simulates managing a user's wallet on the Hubtel app",
        Contact = new OpenApiContact{
            Name = "Isaac Mwesigwa",
            Email = "mwesigwai433@gmail.com"
        }};

        services.AddSwaggerGen(setupAction => {
            setupAction.SwaggerDoc("v1", info);
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            setupAction.IncludeXmlComments(xmlPath);
        });
    }   
}