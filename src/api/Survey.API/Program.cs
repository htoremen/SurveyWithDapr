using Core.Infrastructure;
using Google.Api;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Survey.API.Service;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


var appSettings = builder.Configuration.Get<AppSettings>();
builder.Services.Configure<AppSettings>(options => builder.Configuration.GetSection(nameof(AppSettings)).Bind(options));

builder.Services.AddWebUIServices(builder, appSettings);
builder.Services.AddSwaggerGen(appSettings);
builder.Services.AddHealthChecksServices(appSettings);
builder.Services.AddCustomJwtAuthentication(appSettings);

builder.Services.AddDaprClient(client => {
    client.UseJsonSerializationOptions(new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CorsPolicy");

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseCloudEvents();
app.MapControllers();
app.MapSubscribeHandler();

app.Run();
