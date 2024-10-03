using GameQuests.Domain.AggregatesModel.PlayerAggregate;
using GameQuests.Domain.AggregatesModel.PlayerAggregate.Abstract;
using GameQuests.Domain.AggregatesModel.QuestAggregate;
using GameQuests.Domain.AggregatesModel.QuestAggregate.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameQuests.Domain
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<PlayerOptions>(config.GetSection(PlayerOptions.SectionName));

            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IQuestService, QuestService>();

            return services;
        }
    }
}
