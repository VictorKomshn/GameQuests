using GameQuests.Domain.AggregatesModel.PlayerAggregate;

namespace GameQuests.Domain.AggregatesModel.QuestAggregate.Abstract
{
    public interface IQuestService
    {
        /// <summary>
        /// Получение квестов, доступных игроку
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        Task<ICollection<Quest>> GetAvailableAsync(Player player);
    }
}
