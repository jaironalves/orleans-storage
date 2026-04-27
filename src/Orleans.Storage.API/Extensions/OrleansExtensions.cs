using Orleans.Configuration;
using Orleans.Dashboard;
using Orleans.Storage.Persistence.StateHandler.Abstractions;
using StackExchange.Redis;
using MySqlConnector;
using System.Data;
using Orleans.Storage.Application.Grains.Car.States;
using Orleans.Storage.Application.StateHandler.States;
using Orleans.Storage.Persistence.StateHandler.Extensions;
using Orleans.Storage.Application.StateHandler;

namespace Orleans.Storage.API.Extensions;

public static class OrleansExtensions
{
    public static IHostApplicationBuilder AddOrleans(this IHostApplicationBuilder builder)
    {
        var redisConnectionString = builder.Configuration.GetConnectionString("Silo-Redis")!;
        var redisOptions = ConfigurationOptions.Parse(redisConnectionString);

        var mysqlConnectionString = builder.Configuration.GetConnectionString("Silo-Mysql")!;

        builder.Services
            .AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var connection = ConnectionMultiplexer.Connect(redisOptions);
                return connection;
            })
            .AddSingleton<IDbConnection>(_ => new MySqlConnection(mysqlConnectionString))
            //.AddSingleton<IStateHandlerFactory, StateHandlerFactory>()
            .AddSingleton<IStateHandler<CarState>, CarStateHandler>();

        builder.UseOrleans(siloBuilder =>
        {
            var porta = 10001;
            var portaGateway = 30001;

            siloBuilder
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "storageCluster";
                    options.ServiceId = "storageService";
                })
                .ConfigureEndpoints(porta, portaGateway, listenOnAnyHostAddress: true)
                .UseRedisClustering(options =>
                {
                    options.ConfigurationOptions = redisOptions;
                })
                .AddRedisGrainStorage("redis-storage", options =>
                {
                    options.ConfigurationOptions = redisOptions;
                })
                .AddRedisGrainStorage("redis-stream", options =>
                {
                    options.ConfigurationOptions = redisOptions;
                })
                .AddRedisStreams("redis-stream", redisConfigurator =>
                {
                    redisConfigurator.ConfigureOptions((options, sp) =>
                    {
                        options.ConfigurationOptions = redisOptions;
                    });
                })
                .AddStateHandlerGrainStorage("state-handler-storage", options =>
                {
                    options
                        .AddStateHandler<CarState, CarStateHandler>();
                })
                .AddDashboard();
        });

        return builder;
    }
}
