using GameQuests.Domain.AggregatesModel.PlayerAggregate;
using GameQuests.Domain.AggregatesModel.PlayerAggregate.Abstract;

namespace GameQuests.Infrastructure.Data.Repositories
{
    public class PlayerRepository : RepositoryBase<Player>, IPlayerRepository
    {
        public PlayerRepository(GameQuestDataConext conext) : base(conext) { }
    }
}
