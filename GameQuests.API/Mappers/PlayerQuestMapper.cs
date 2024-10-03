using GameQuests.API.ViewModel;
using GameQuests.Domain.AggregatesModel.PlayerAggregate;

namespace GameQuests.API.Mappers
{
    public static class PlayerQuestMapper
    {
        public static PlayerQuestViewModel? ToViewModel(this PlayerQuest? playerQuest)
        {
            if (playerQuest == null)
            {
                return null;
            }
            throw new NotImplementedException();
        }
    }
}
