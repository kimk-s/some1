using MagicOnion.Serialization;
using MagicOnion.Serialization.MemoryPack;
using Some1.Auth.Admin;
using Some1.Store.Admin;
using Some1.Wait.Back;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddScoped<WaitBack>()
    .AddSome1WaitData(builder.Configuration)
    .AddSome1AuthAdmin(builder.Configuration)
    .AddSome1StoreAdmin(builder.Configuration);

builder.Services.AddGrpc();

MagicOnionSerializerProvider.Default = MemoryPackMagicOnionSerializerProvider.Instance;
builder.Services.AddMagicOnion();

builder.Logging.AddSimpleConsole(c =>
{
    c.UseUtcTimestamp = true;
    c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss.fff UTC] ";
});

var app = builder.Build();

await app.Services.GetRequiredService<IAuthAdmin>().InitialzieAsync("", default);
await app.Services.GetRequiredService<IStoreAdmin>().InitializeAsync("", default);
app.MapMagicOnionService();

app.Run();
