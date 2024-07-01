using Npgsql;
using Some1.Auth.Admin;
using Some1.Data.InMemory;
using Some1.Store.Admin;
using Some1.Wait.Data;
using Some1.Wait.Data.InMemory;
using Some1.Wait.Data.Postgres;

namespace Microsoft.Extensions.DependencyInjection;

public static class WaitServerServiceCollectionExtensions
{
    public static IServiceCollection AddSome1WaitData(this IServiceCollection services, IConfiguration configuration)
    {
        var dataOptions = new DataOptions();
        configuration.GetRequiredSection(DataOptions.Data).Bind(dataOptions);

        if (dataOptions.InMemory)
        {
            services
                .Configure<InMemoryRepositoryOptions>(configuration.GetRequiredSection($"{DataOptions.Data}:{InMemoryRepositoryOptions.InMemoryRepository}"))
                .AddSingleton<IWaitRepository, InMemoryWaitRepository>()
                .AddSingleton<InMemoryRepository>();
        }
        else
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services
                .AddScoped<IWaitRepository, PostgresWaitRepository>()
                .AddSingleton(_ => NpgsqlDataSource.Create(configuration.GetConnectionString("Postgres")!));
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

    public static IServiceCollection AddSome1StoreAdmin(this IServiceCollection services, IConfiguration configuration)
    {
        var storeOptions = new StoreOptions();
        configuration.GetRequiredSection(StoreOptions.Store).Bind(storeOptions);

        if (storeOptions.Fake)
        {
            services.AddSingleton<IStoreAdmin, FakeStoreAdmin>();
        }
        else
        {
            services.AddSingleton<IStoreAdmin, StoreAdmin>();
        }

        return services;
    }
}
