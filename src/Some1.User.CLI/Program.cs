using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Some1.Net;
using Some1.Play.Client;
using Some1.Play.Client.Tcp;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.Play.Info.Alpha;
using Some1.User.CLI;

var builder = ConsoleApp.CreateBuilder(args);

builder.ConfigureAppConfiguration(x =>
{
    x.AddJsonFile("appsettings.json");
});

builder.ConfigureServices((ctx, services) =>
{
    services
        .AddScoped<IPlayFront, PlayFront>()
        .AddScoped<IPlayClient, TcpPlayClient>()
        .AddScoped<IPlayStreamer, RemotePlayStreamer>()
        .AddSingleton<RemotePlayStreamerJitterBuffer>()
        .AddSingleton<IPlayInfoRepository, AlphaPlayInfoRepository>(_ => new(ctx.HostingEnvironment.EnvironmentName))
        .AddSingleton<DuplexPipeProcessor>()
        .AddScoped<IPlayClientAuth, GuidPlayClientAuth>()
        .AddSingleton<IPlayAddressGettable, PlayAddress>();

    services.AddLogging(x =>
    {
        x.ClearProviders();
        x.AddSimpleConsole(c =>
        {
            c.UseUtcTimestamp = true;
            c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss.fff UTC] ";
        });
    });
});

var app = builder.Build();
app.AddCommands<Command>();
await app.Services.GetRequiredService<IPlayInfoRepository>().LoadAsync(default);
app.Run();
