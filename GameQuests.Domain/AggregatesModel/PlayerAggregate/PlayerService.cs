using GameQuests.Domain.AggregatesModel.EnemyAggregate;
using GameQuests.Domain.AggregatesModel.ItemAggregate;
using GameQuests.Domain.AggregatesModel.PlayerAggregate.Abstract;
using GameQuests.Domain.AggregatesModel.QuestAggregate;
using GameQuests.Domain.AggregatesModel.QuestAggregate.Exceptions;
using GameQuests.Domain.SeedWork;
using GameQuests.Domain.SeedWork.Exceptions;
using Microsoft.Extensions.Options;

namespace GameQuests.Domain.AggregatesModel.PlayerAggregate
{
    public class PlayerService : IPlayerService
    {

        private readonly IPlayerRepository _playerRepository;

        private readonly PlayerOptions _options;

        public PlayerService(IPlayerRepository playerRepository,
                             IOptions<PlayerOptions> options)
        {
            _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<PlayerQuest?> AcceptQuestAsync(Player player, Quest quest)
        {
            if (player.PlayerQuests.Count < _options.MaxActiveQuests)
            {
                if (quest.IsAvailableFor(player))
                {
                    var playerQuest = new PlayerQuest
                    {
                        Player = player,
                        Progress = 0,
                        Quest = quest,
                        Status = PlayerQuestStatus.Accepted,
                    };
                    player.PlayerQuests.Add(playerQuest);

                    await _playerRepository.UpdateAsync(player);

                    return playerQuest;
                }
                throw new UnavailableQuestException();
            }
            throw new TooManyQuestsException();
        }

        public async Task DenyQuestAsync(Player player, Quest quest)
        {
            var playerQuest = player.PlayerQuests.FirstOrDefault(x => x.Quest.Id == quest.Id);
            if (playerQuest == null)
            {
                throw new NotFoundException();
            }
            player.PlayerQuests.Remove(playerQuest);
            await _playerRepository.UpdateAsync(player);
        }

        public async Task<IEnumerable<PlayerQuest>?> CollectItemAsync(Player player, Item item)
        {
            player.Inventory.Add(item);
            try
            {
                var playerQuest = player.CheckQuests(item);
                await _playerRepository.UpdateAsync(player);

                return playerQuest;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<PlayerQuest>?> DropItemAsync(Player player, Item item)
        {
            player.Inventory.Remove(item);
            try
            {
                var playerQuest = player.CheckQuests(item);
                await _playerRepository.UpdateAsync(player);

                return playerQuest;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<PlayerQuest>?> KillEnemyAsync(Player player, Enemy enemy)
        {
            try
            {
                var playerQuest = player.CheckQuests(enemy);
                await _playerRepository.UpdateAsync(player);

                return playerQuest;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<PlayerQuest>?> MoveToAsync(Player player, Position newPosition)
        {
            player.Position = newPosition;
            try
            {
                var playerQuest = player.CheckQuests(newPosition);
                await _playerRepository.UpdateAsync(player);
                return playerQuest;
            }
            catch
            {
                throw;
            }
        }
    }
}
