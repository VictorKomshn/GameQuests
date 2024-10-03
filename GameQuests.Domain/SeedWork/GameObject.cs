using GameQuests.Domain.AggregatesModel.QuestAggregate.Abstract;
using GameQuests.Domain.SeedWork.Base;

namespace GameQuests.Domain.SeedWork
{
    public abstract class GameObject : BaseEntity, IQuestObjective
    {
        public string Name { get; set; }

        public string Desciption { get; set; }
    }
}
