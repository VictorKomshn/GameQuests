using GameQuests.Domain.AggregatesModel.PlayerAggregate.Abstract;
using GameQuests.Domain.AggregatesModel.PlayerAggregate;
using GameQuests.Domain.AggregatesModel.QuestAggregate.Abstract;
using GameQuests.Infrastructure.Data.Repositories;
using GameQuests.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using GameQuests.Domain.AggregatesModel.QuestAggregate;

namespace GameQuests.Domain.Test.QuestAggreagateTests
{
    internal class QuestServiceTest
    {

        private IQuestService _questSerivce;
        private IQuestRepository _questRepository;

        private ServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();

            // Using In-Memory database for testing
            services.AddDbContext<GameQuestDataConext>(options =>
            options.UseInMemoryDatabase("TestDb"));
            services.AddTransient<IPlayerRepository, PlayerRepository>();
            services.AddTransient<IQuestRepository, QuestRepository>();

            _serviceProvider = services.BuildServiceProvider();
            _questRepository = _serviceProvider.GetRequiredService<IQuestRepository>();
            IOptions<PlayerOptions> playerOptions = Options.Create(new PlayerOptions() { MaxActiveQuests = 10 });

            _questSerivce = new QuestService(_questRepository);
        }

        [Test]
        public async Task ShoundReturn_CollectionOfAbailableQuests()
        {
            var testAvailableQuest = new Quest()
            {
                Id = Guid.NewGuid(),
                Description = "test",
                Name = "test",
                MinLvlRequired = 1
            };

            var testUnavailableQuest = new Quest()
            {
                Id = Guid.NewGuid(),
                Description = "test",
                Name = "test",
                MinLvlRequired = 2
            };

            var testPlayer = new Player();

            await _questRepository.AddAsync(testUnavailableQuest);
            await _questRepository.AddAsync(testAvailableQuest);

            var actualAvailableQuest = await _questSerivce.GetAvailableAsync(testPlayer);

            var expectedAvailableQuest = testAvailableQuest;
            Assert.NotNull(actualAvailableQuest.First());
            Assert.That(actualAvailableQuest.First().Id, Is.EqualTo(expectedAvailableQuest.Id));
        }

        [TearDown]
        public void Dispose()
        {
            _serviceProvider.Dispose();
        }
    }
}
