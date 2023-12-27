using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Survey.API.Service;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services, WebApplicationBuilder builder, AppSettings appSettings)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();
        services.AddScoped<HttpContextAccessor>();
        services.AddHttpClient();
        services.AddScoped<HttpClient>();
        services.AddSingleton<IConfiguration>(builder.Configuration);

        StaticValues.Secret = appSettings.Authenticate.Secret;

        return services;
    }

    public static IServiceCollection AddHealthChecksServices(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddHealthChecks();
        return services;
    }


    public static void AddCustomJwtAuthentication(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.RequireHttpsMetadata = false;
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, // Oluşturulacak token değerini kimlerin/hangi originlerin/sitelerin kullanacağını belirlediğimiz alandır.
                ValidateIssuer = false, // Oluşturulacak token değerini kimin dağıttığını ifade edeceğimiz alandır.
                ValidateLifetime = true, // Oluşturulan token değerinin süresini kontrol edecek olan doğrulamadır.
                ValidateIssuerSigningKey = true, // Üretilecek token değerinin uygulamamıza ait bir değer olduğunu ifade eden security key verisinin doğrulamasıdır.
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.Authenticate.Secret)),
                ClockSkew = TimeSpan.Zero // Üretilecek token değerinin expire süresinin belirtildiği değer kadar uzatılmasını sağlayan özelliktir. 
            };
        });
    }

    public static void AddSwaggerGen(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Survey API",
                Version = "v1"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
           {
             new OpenApiSecurityScheme
             {
               Reference = new OpenApiReference
               {
                 Type = ReferenceType.SecurityScheme,
                 Id = "Bearer"
               }
              },
              new string[] { }
            }
          });
        });
    }
}