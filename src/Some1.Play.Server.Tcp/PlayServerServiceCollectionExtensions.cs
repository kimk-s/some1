using Microsoft.Extensions.Configuration;
using Npgsql;
using Some1.Auth.Admin;
using Some1.Data.InMemory;
using Some1.Play.Data.InMemory;
using Some1.Play.Data.Postgres;
using Some1.Play.Data;
using Some1.Play.Server.Tcp;

namespace Microsoft.Extensions.DependencyInjection;

public static class PlayServerServiceCollectionExtensions
{
    public static IServiceCollection AddSome1PlayData(this IServiceCollection services, IConfiguration configuration)
    {
        var dataOptions = new DataOptions();
        configuration.GetRequiredSection(DataOptions.Data).Bind(dataOptions);

        if (dataOptions.InMemory)
        {
            services
                .Configure<InMemoryRepositoryOptions>(configuration.GetRequiredSection($"{DataOptions.Data}:{InMemoryRepositoryOptions.InMemoryRepository}"))
                .AddSingleton<IPlayRepository, InMemoryPlayRepository>()
                .AddSingleton<InMemoryRepository>();
        }
        else
        {
            services
                .AddSingleton<IPlayRepository, PostgresPlayRepository>()
                .AddSingleton(_
                    => new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Postgres")!)
                        .EnableDynamicJson()
                        .Build());
        }

        return services;
    }

    public static IServiceCollection AddSome1AuthAdmin(this IServiceCollection services, IConfiguration configuration)
    {
        var authOptions = new AuthOptions();
        configuration.GetRequiredSection(AuthOptions.Auth).Bind(authOptions);

        if (authOptions.Fake)
        {
            services.AddSingleton<IAuthAdmin, FakeAuthAdmin>();
        }
        else
        {
            services.AddSingleton<IAuthAdmin, FirebaseAuthAdmin>();
        }

        return services;
    }
}
