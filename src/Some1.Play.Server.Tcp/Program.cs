using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Some1.Auth.Admin;
using Some1.Net;
using Some1.Play.Core;
using Some1.Play.Core.Options;
using Some1.Play.Info;
using Some1.Play.Info.Alpha;
using Some1.Play.Server.Tcp;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services
    .Configure<ListenerOptions>(builder.Configuration.GetSection(ListenerOptions.Listener))
    .Configure<PlayLoopOptions>(builder.Configuration.GetSection(PlayLoopOptions.PlayLoop))
    .Configure<PlayOptions>(builder.Configuration.GetSection(PlayOptions.Play));

builder.Services
    .AddHostedService<ListenerService>()
    .AddHostedService<PlayLoopService>()
    .AddSingleton<PlayLoop>()
    .AddSingleton<IPlayCore, PlayCore>()
    .AddSingleton<IClock, DateTimeClock>()
    .AddSingleton<IPlayInfoRepository, AlphaPlayInfoRepository>(_ => new(builder.Environment.EnvironmentName))
    .AddSingleton<DuplexPipeProcessor>()
    .AddSome1PlayData(builder.Configuration)
    .AddSome1AuthAdmin(builder.Configuration);

builder.Logging.AddSimpleConsole(c =>
{
    c.UseUtcTimestamp = true;
    c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss.fff UTC] ";
});

IHost host = builder.Build();

await host.Services.GetRequiredService<IPlayInfoRepository>().LoadAsync(default);
await host.Services.GetRequiredService<IAuthAdmin>().InitialzieAsync("", default);

host.Run();
