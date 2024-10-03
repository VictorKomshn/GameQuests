using GameQuests.Domain.AggregatesModel.PlayerAggregate.Abstract;
using GameQuests.Domain.AggregatesModel.QuestAggregate.Abstract;
using GameQuests.Infrastructure.Data;
using GameQuests.Infrastructure.Data.Repositories;
using GameQuests.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace GameQuests.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            var postgresOptions = new PostgreOptions();
            config.Bind(PostgreOptions.SectionName, postgresOptions);
            var connectionBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = postgresOptions.Host,
                Port = postgresOptions.Port,
                Username = postgresOptions.Username,
                Password = postgresOptions.Password,
                Database = postgresOptions.Database,
            };
            services.AddDbContext<GameQuestDataConext>(options => options.UseNpgsql(connectionBuilder.ToString())
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors(), contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Singleton);


            //services.AddSingleton(provider =>
            //{


            //    var builder = new DbContextOptionsBuilder<AustriaDataContext>()
            //        .UseNpgsql(connectionBuilder.ToString())
            //        .EnableSensitiveDataLogging()
            //        .EnableDetailedErrors();
            //    return builder.Options;
            //});

            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            using var dataContext = scope.ServiceProvider.GetRequiredService<GameQuestDataConext>();
            dataContext.Database.EnsureCreated();

            services.AddTransient<IQuestRepository, QuestRepository>();
            services.AddTransient<IPlayerRepository, PlayerRepository>();

            return services;
        }
    }
}
