using AlphaApi.Implementations;
using AlphaApi.Interfaces;
using AlphaApi.MicroServices;

namespace AlphaApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddHttpClient<BetaApiClient>();
        
        builder.Services.AddSingleton<IDataService, DataService>();
        
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();
        builder.Services.AddEndpointsApiExplorer();
        
        builder.WebHost.UseUrls("http://0.0.0.0:8080");
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BetaApi V1");
                c.RoutePrefix = string.Empty;
            });
        }
        app.MapControllers();

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.Run();
    }
}