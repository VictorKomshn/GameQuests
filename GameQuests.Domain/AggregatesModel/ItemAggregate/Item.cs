using GameQuests.Domain.AggregatesModel.QuestAggregate.Abstract;
using GameQuests.Domain.SeedWork;

namespace GameQuests.Domain.AggregatesModel.ItemAggregate
{
    public class Item : GameObject
    {
        public int Price { get; set; }
    }
}
