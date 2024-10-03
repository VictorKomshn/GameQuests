using GameQuests.Domain.AggregatesModel.EnemyAggregate;
using GameQuests.Domain.AggregatesModel.ItemAggregate;
using GameQuests.Domain.AggregatesModel.QuestAggregate;
using GameQuests.Domain.SeedWork;

namespace GameQuests.Domain.AggregatesModel.PlayerAggregate.Abstract
{
    public interface IPlayerService
    {
        public Task<PlayerQuest?> AcceptQuestAsync(Player player, Quest quest);

        public Task DenyQuestAsync(Player player, Quest quest);

        public Task<IEnumerable<PlayerQuest>?> MoveToAsync(Player player, Position newPosition);

        public Task<IEnumerable<PlayerQuest>?> KillEnemyAsync(Player player, Enemy enemy);

        public Task<IEnumerable<PlayerQuest>?> CollectItemAsync(Player player, Item item);

        public Task<IEnumerable<PlayerQuest>?> DropItemAsync(Player player, Item item);

    }
}
