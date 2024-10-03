using GameQuests.Domain.AggregatesModel.PlayerAggregate;
using GameQuests.Domain.AggregatesModel.QuestAggregate.Abstract;

namespace GameQuests.Domain.AggregatesModel.QuestAggregate
{
    public class QuestService : IQuestService
    {
        private readonly IQuestRepository _questRepository;

        public QuestService(IQuestRepository questRepository)
        {
            _questRepository = questRepository ?? throw new ArgumentNullException(nameof(questRepository));
        }

        public async Task<ICollection<Quest>> GetAvailableAsync(Player player)
        {
            return await _questRepository.ListAsync(x => x.IsAvailableFor(player));
        }
    }
}
