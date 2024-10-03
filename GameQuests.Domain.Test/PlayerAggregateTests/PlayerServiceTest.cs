using GameQuests.Domain.AggregatesModel.EnemyAggregate;
using GameQuests.Domain.AggregatesModel.ItemAggregate;
using GameQuests.Domain.AggregatesModel.LocaitonAggregate;
using GameQuests.Domain.AggregatesModel.PlayerAggregate;
using GameQuests.Domain.AggregatesModel.PlayerAggregate.Abstract;
using GameQuests.Domain.AggregatesModel.QuestAggregate;
using GameQuests.Domain.AggregatesModel.QuestAggregate.Abstract;
using GameQuests.Domain.AggregatesModel.QuestAggregate.Exceptions;
using GameQuests.Domain.SeedWork;
using GameQuests.Domain.SeedWork.Exceptions;
using GameQuests.Infrastructure.Data;
using GameQuests.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GameQuests.Domain.Test.PlayerAggregateTests
{
    internal class PlayerServiceTest
    {
        private IPlayerService _playerSerivce;
        private IPlayerRepository _playerRepository;

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
            _playerRepository = _serviceProvider.GetRequiredService<IPlayerRepository>();
            IOptions<PlayerOptions> playerOptions = Options.Create(new PlayerOptions() { MaxActiveQuests = 10 });

            _playerSerivce = new PlayerService(_playerRepository, playerOptions);
        }

        #region AcceptQuest

        [Test]
        public void Should_ThrowException_On_Unavailable_AcceptQuest()
        {
            var testPlayer = new Player();

            _playerRepository.AddAsync(testPlayer);
            var testQuestWithBiggerMinLvlRequired = new Quest()
            {
                Id = Guid.NewGuid(),
                MinLvlRequired = 2
            };

            Assert.ThrowsAsync<UnavailableQuestException>(async () => await _playerSerivce.AcceptQuestAsync(testPlayer, testQuestWithBiggerMinLvlRequired));
        }

        [Test]
        public void Should_ThrowException_On_TooMany_AcceptQuest()
        {
            var testPlayer = new Player();
            var testQuest = new Quest()
            {
                Id = Guid.NewGuid(),
                MinLvlRequired = 1
            };
            for (int i = 0; i < 10; i++)
            {
                var playerQuest = new PlayerQuest
                {
                    Player = testPlayer,
                    Quest = testQuest,
                    Progress = 0,
                    Status = PlayerQuestStatus.InProgress
                };

                testPlayer.PlayerQuests.Add(playerQuest);

            }
            _playerRepository.AddAsync(testPlayer);


            Assert.ThrowsAsync<TooManyQuestsException>(async () => await _playerSerivce.AcceptQuestAsync(testPlayer, testQuest));
        }

        [Test]
        public async Task Should_Success_On_Correct_AcceptQuest()
        {
            var testPlayer = new Player();
            await _playerRepository.AddAsync(testPlayer);

            var testQuest = new Quest()
            {
                Id = Guid.NewGuid(),
                Name = "Test task",
                Description = "This is your first quest -- dont ruin it",
                MinLvlRequired = 1
            };

            var testQuestObjectives = new[] {
                    new QuestObjective
                    {
                        ObjectiveGameObject = new Item()
                            {
                                Id = Guid.NewGuid(),
                                Price = 1,
                                Name = "Gold",
                                Desciption = "Some shiny ore to buy things with"
                            },
                        Amount = 1,
                        Quest  =testQuest
                    }};

            testQuest.Objectives = testQuestObjectives;

            var questRepository = _serviceProvider.GetRequiredService<IQuestRepository>();

            await questRepository.AddAsync(testQuest);

            var expected = new PlayerQuest
            {
                Player = testPlayer,
                Status = PlayerQuestStatus.Accepted,
                Quest = testQuest,
                Progress = 0
            };

            var actual = await _playerSerivce.AcceptQuestAsync(testPlayer, testQuest);

            Assert.IsNotNull(actual);
            Assert.That(actual.Quest.Id, Is.EqualTo(expected.Quest.Id));
            Assert.That(actual.Player.Id, Is.EqualTo(expected.Player.Id));
            Assert.That(actual.Progress, Is.EqualTo(expected.Progress));
            Assert.That(actual.Status, Is.EqualTo(expected.Status));
        }

        #endregion

        #region DenyQuest

        [Test]
        public async Task Should_ThrowException_On_NonExistent_DenyQuest()
        {
            var testPlayer = new Player();
            await _playerRepository.AddAsync(testPlayer);

            var testQuest = new Quest()
            {
                Id = Guid.NewGuid(),
                Name = "Test task",
                Description = "This is your first quest -- dont ruin it",
                MinLvlRequired = 1
            };

            var testQuestObjectives = new[] {
                    new QuestObjective
                    {
                        ObjectiveGameObject = new Item()
                            {
                                Id = Guid.NewGuid(),
                                Price = 1,
                                Name = "Gold",
                                Desciption = "Some shiny ore to buy things with"
                            },
                        Amount = 1,
                        Quest  =testQuest
                    }};

            testQuest.Objectives = testQuestObjectives;
            Assert.ThrowsAsync<NotFoundException>(async () => await _playerSerivce.DenyQuestAsync(testPlayer, testQuest));
        }

        [Test]
        public async Task Should_Success_On_Correct_DenyQuest()
        {
            var testPlayer = new Player();
            await _playerRepository.AddAsync(testPlayer);

            var testQuest = new Quest()
            {
                Id = Guid.NewGuid(),
                Name = "Test task",
                Description = "This is your first quest -- dont ruin it",
                MinLvlRequired = 1
            };

            var testQuestObjectives = new[] {
                    new QuestObjective
                    {
                        ObjectiveGameObject = new Item()
                            {
                                Id = Guid.NewGuid(),
                                Price = 1,
                                Name = "Gold",
                                Desciption = "Some shiny ore to buy things with"
                            },
                        Amount = 1,
                        Quest  =testQuest
                    }};

            testQuest.Objectives = testQuestObjectives;

            var questRepository = _serviceProvider.GetRequiredService<IQuestRepository>();

            await questRepository.AddAsync(testQuest);

            await _playerSerivce.AcceptQuestAsync(testPlayer, testQuest);

            Assert.DoesNotThrowAsync(async () => await _playerSerivce.DenyQuestAsync(testPlayer, testQuest));
        }

        #endregion

        #region CollectItem

        [Test]
        public async Task Should_ReturnQuest_Increment_On_CollectItem_CollectQuestExists()
        {
            var testPlayer = new Player();
            await _playerRepository.AddAsync(testPlayer);
            int questItemsAmount = 3;
            var testQuestItem = new Item()
            {
                Id = Guid.NewGuid(),
                Price = 1,
                Name = "Gold",
                Desciption = "Some shiny ore to buy things with"
            };

            var testCollectQuest = new Quest()
            {
                Id = Guid.NewGuid(),
                Name = "Test task",
                Description = "This is your first quest -- dont ruin it",
                QuestType = QuestType.Collect,
                Reward = new[] {new Item()
                {
                    Id = Guid.NewGuid(),
                    Price = 100,
                    Name = "GreatGold",
                    Desciption = "Big and expensive gold piece"
                } },
                MinLvlRequired = 1
            };

            var testQuestObjectives = new[] {
                    new QuestObjective
                    {
                        ObjectiveGameObject = testQuestItem,
                        Amount = questItemsAmount,
                        Quest  = testCollectQuest
                    }};

            testCollectQuest.Objectives = testQuestObjectives;

            var questRepository = _serviceProvider.GetRequiredService<IQuestRepository>();

            await questRepository.AddAsync(testCollectQuest);


            await _playerSerivce.AcceptQuestAsync(testPlayer, testCollectQuest);

            var quest = await _playerSerivce.CollectItemAsync(testPlayer, testQuestItem);

            Assert.NotNull(quest);
            Assert.IsTrue(quest.FirstOrDefault()?.Progress == 1);

        }

        [Test]
        public async Task Should_ReturnNull_On_CollectItem_CollectQuestNotExist()
        {
            var testPlayer = new Player();
            await _playerRepository.AddAsync(testPlayer);

            var testQuestItem = new Item()
            {
                Id = Guid.NewGuid(),
                Price = 1,
                Name = "Gold",
                Desciption = "Some shiny ore to buy things with"
            };

            var testQuest = new Quest()
            {
                Id = Guid.NewGuid(),
                Name = "Test task",
                Description = "This is your first quest -- dont ruin it",
                MinLvlRequired = 1
            };

            var testQuestObjectives = new[] {
                    new QuestObjective
                    {
                        ObjectiveGameObject = testQuestItem,
                        Amount = 1,
                        Quest  =testQuest
                    }};

            testQuest.Objectives = testQuestObjectives;

            var questRepository = _serviceProvider.GetRequiredService<IQuestRepository>();

            await questRepository.AddAsync(testQuest);

            var quest = await _playerSerivce.CollectItemAsync(testPlayer, testQuestItem);

            Assert.IsNull(quest);
        }

        #endregion

        #region DropItem

        [Test]
        public async Task Should_ReturnQuest_Decrement_On_DropItem_CollectQuestExist()
        {
            var testPlayer = new Player();
            await _playerRepository.AddAsync(testPlayer);
            int questItemsAmount = 3;
            var testQuestItem = new Item()
            {
                Id = Guid.NewGuid(),
                Price = 1,
                Name = "Gold",
                Desciption = "Some shiny ore to buy things with"
            };

            var testCollectQuest = new Quest()
            {
                Id = Guid.NewGuid(),
                Name = "Test task",
                Description = "This is your first quest -- dont ruin it",
                QuestType = QuestType.Collect,
                Reward = new[] {new Item()
                {
                    Id = Guid.NewGuid(),
                    Price = 100,
                    Name = "GreatGold",
                    Desciption = "Big and expensive gold piece"
                } },
                MinLvlRequired = 1
            };

            var testQuestObjectives = new[] {
                    new QuestObjective
                    {
                        ObjectiveGameObject =testQuestItem,
                        Amount = questItemsAmount,
                        Quest  = testCollectQuest
                    }};

            testCollectQuest.Objectives = testQuestObjectives;

            var questRepository = _serviceProvider.GetRequiredService<IQuestRepository>();

            await questRepository.AddAsync(testCollectQuest);


            await _playerSerivce.AcceptQuestAsync(testPlayer, testCollectQuest);

            int expectedProgress = 2;

            await _playerSerivce.CollectItemAsync(testPlayer, testQuestItem);
            var quest = await _playerSerivce.CollectItemAsync(testPlayer, testQuestItem);

            Assert.NotNull(quest);
            Assert.IsTrue(quest.FirstOrDefault()?.Progress == expectedProgress);


            expectedProgress--;
            quest = await _playerSerivce.DropItemAsync(testPlayer, testQuestItem);

            Assert.NotNull(quest);
            Assert.IsTrue(quest.FirstOrDefault()?.Progress == expectedProgress);
        }

        [Test]
        public async Task Should_ReturnNull_On_DropItem_CollectQuestNotExist()
        {
            var testPlayer = new Player();
            await _playerRepository.AddAsync(testPlayer);

            var testQuestItem = new Item()
            {
                Id = Guid.NewGuid(),
                Price = 1,
                Name = "Gold",
                Desciption = "Some shiny ore to buy things with"
            };

            var testQuest = new Quest()
            {
                Id = Guid.NewGuid(),
                Name = "Test task",
                Description = "This is your first quest -- dont ruin it",
                MinLvlRequired = 1
            };

            var testQuestObjectives = new[] {
                    new QuestObjective
                    {
                        ObjectiveGameObject = testQuestItem,
                        Amount = 1,
                        Quest  =testQuest
                    }};

            testQuest.Objectives = testQuestObjectives;

            var questRepository = _serviceProvider.GetRequiredService<IQuestRepository>();

            await questRepository.AddAsync(testQuest);

            var quests = await _playerSerivce.CollectItemAsync(testPlayer, testQuestItem);

            Assert.IsNull       (quests);

            quests = await _playerSerivce.DropItemAsync(testPlayer, testQuestItem);

            Assert.IsNull(quests);
        }

        #endregion

        #region KillEnemy

        [Test]
        public async Task Should_ReturnQuest_On_KillEnemy_CollectQuestExist()
        {
            var testPlayer = new Player();
            await _playerRepository.AddAsync(testPlayer);
            int questItemsAmount = 3;
            var testQuestItem = new Enemy()
            {
                Id = Guid.NewGuid(),
                Name = "Gold",
                Desciption = "Some shiny ore to buy things with",
                Power = 10,
            };

            var testCollectQuest = new Quest()
            {
                Id = Guid.NewGuid(),
                Name = "Test task",
                Description = "This is your first quest -- dont ruin it",
                QuestType = QuestType.Eliminate,
                Reward = new[] {new Item()
                {
                    Id = Guid.NewGuid(),
                    Price = 100,
                    Name = "GreatGold",
                    Desciption = "Big and expensive gold piece"
                } },
                MinLvlRequired = 1
            };

            var testQuestObjectives = new[] {
                    new QuestObjective
                    {
                        ObjectiveGameObject = testQuestItem,
                        Amount = questItemsAmount,
                        Quest = testCollectQuest
                    }};

            testCollectQuest.Objectives = testQuestObjectives;

            var questRepository = _serviceProvider.GetRequiredService<IQuestRepository>();

            await questRepository.AddAsync(testCollectQuest);

            await _playerSerivce.AcceptQuestAsync(testPlayer, testCollectQuest);


            int expectedProgress = 1;

            var quest = await _playerSerivce.KillEnemyAsync(testPlayer, testQuestItem);

            Assert.NotNull(quest);
            Assert.IsTrue(quest.FirstOrDefault()?.Progress == expectedProgress);
        }

        #endregion

        #region MoveTo

        [Test]
        public async Task Should_ReturnFinishedQuest_On_MoveTo_CollectQuestExist()
        {
            var testPlayer = new Player();
            await _playerRepository.AddAsync(testPlayer);
            int questItemsAmount = 1;

            var testStartLocationPosition = new Position(1, 1);
            var testEndLocationPosition = new Position(3, 3);
            var testQuestItem = new Location(testStartLocationPosition, testEndLocationPosition, "Realm", "The first location");

            var testCollectQuest = new Quest()
            {
                Id = Guid.NewGuid(),
                Name = "Test task",
                Description = "This is your first quest -- dont ruin it",
                QuestType = QuestType.Visit,
                Reward = new[] {new Item()
                {
                    Id = Guid.NewGuid(),
                    Price = 100,
                    Name = "GreatGold",
                    Desciption = "Big and expensive gold piece"
                } },
                MinLvlRequired = 1
            };

            var testQuestObjectives = new[] {
                    new QuestObjective
                    {
                        ObjectiveGameObject =testQuestItem,
                        Amount = questItemsAmount,
                        Quest  =testCollectQuest
                    }};

            testCollectQuest.Objectives = testQuestObjectives;

            var questRepository = _serviceProvider.GetRequiredService<IQuestRepository>();

            await questRepository.AddAsync(testCollectQuest);

            await _playerSerivce.AcceptQuestAsync(testPlayer, testCollectQuest);

            int expectedProgress = 1;

            var newPosition = new Position(2, 2);
            var quest = await _playerSerivce.MoveToAsync(testPlayer, newPosition);

            Assert.NotNull(quest);
            Assert.IsTrue(quest.FirstOrDefault()?.Progress == expectedProgress);
            Assert.IsTrue(quest.FirstOrDefault()?.Status == PlayerQuestStatus.Finished);
        }

        #endregion

        [TearDown]
        public void Dispose()
        {
            _serviceProvider.Dispose();
        }
    }
}
