using GameQuests.Domain.AggregatesModel.ItemAggregate;
using GameQuests.Domain.AggregatesModel.QuestAggregate.Abstract;
using GameQuests.Domain.SeedWork;

namespace GameQuests.Domain.AggregatesModel.EnemyAggregate
{
    public class Enemy : GameObject
    {
        public int Power { get; set; }
    }
}
