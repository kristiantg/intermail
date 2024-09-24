using intermail.Authentication;
using intermail.Clients;
using intermail.Endpoints;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSerilog();
});

builder.Services.AddHttpClient<IIntermailHttpClient, IntermailHttpClient>(client =>
{
    var baseUrlString = builder.Configuration["IntermailClient:BaseUrl"];
    var accessToken = builder.Configuration["IntermailClient:AccessToken"];

    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
    client.BaseAddress = new Uri(baseUrlString);
});

Log.Logger = new LoggerConfiguration()
    .Enrich.WithThreadId()
    .Enrich.WithThreadName()
    .WriteTo.File("./Logs/Entry.log", outputTemplate:
  "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties}{NewLine}{Exception}")  
    .CreateLogger();

var app = builder.Build();


app.UseHttpsRedirection();

app.UseMiddleware<ApiKeyAuthMiddleware>();

app.UseAuthorization();

app.MapControllers();
app.MapCustomersEndpoints();


app.Run();
