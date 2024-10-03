using GameQuests.Domain.AggregatesModel.ItemAggregate;
using GameQuests.Domain.AggregatesModel.PlayerAggregate;
using GameQuests.Domain.AggregatesModel.QuestAggregate.Abstract;
using GameQuests.Domain.SeedWork;
using GameQuests.Domain.SeedWork.Base;

namespace GameQuests.Domain.AggregatesModel.QuestAggregate
{
    public class Quest : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public QuestType QuestType { get; set; }

        /// <summary>
        /// Требования для доступа к уровню (минимальный уровень)
        /// </summary>
        public int MinLvlRequired { get; set; }

        /// <summary>
        /// Требования для доступа к уровню (завершенные квесты)
        /// </summary>
        public IEnumerable<Quest>? CompletedQuestsRequired { get; set; }

        /// <summary>
        /// Награда за выполнение квеста
        /// </summary>
        public IEnumerable<Item> Reward { get; set; }

        /// <summary>
        /// Объект квеста
        /// </summary>
        public IEnumerable<QuestObjective> Objectives { get; set; }

        public bool IsAvailableFor(Player player)
        {
            var questIsFinished = player.PlayerQuests.Any(x => x.Quest.Id == this.Id);

            if (!questIsFinished)
            {
                var questAvailable = player.Lvl >= MinLvlRequired;
                if (CompletedQuestsRequired != null && CompletedQuestsRequired.Any())
                {
                    questAvailable = CompletedQuestsRequired.All(x => player.PlayerQuests.Any(y => y.Quest.Id == x.Id));
                }
                return questAvailable;
            }
            return false;

        }
    }
}
