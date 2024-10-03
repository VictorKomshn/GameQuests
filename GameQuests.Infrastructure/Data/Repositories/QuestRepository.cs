using GameQuests.Domain.AggregatesModel.QuestAggregate;
using GameQuests.Domain.AggregatesModel.QuestAggregate.Abstract;

namespace GameQuests.Infrastructure.Data.Repositories
{
    public class QuestRepository : RepositoryBase<Quest>, IQuestRepository
    {
        public QuestRepository(GameQuestDataConext conext) : base(conext) { }
    }
}
