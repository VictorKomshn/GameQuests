using GameQuests.API.ViewModel;
using GameQuests.Domain.AggregatesModel.QuestAggregate;

namespace GameQuests.API.Mappers
{
    public static class QuestMapper
    {
        public static QuestViewModel? ToViewModel(this Quest? quest)
        {
            if (quest == null)
            {
                return null;
            }
            throw new NotImplementedException();
        }
    }
}
