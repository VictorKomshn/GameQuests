namespace GameQuests.Domain.AggregatesModel.PlayerAggregate
{
    public class PlayerOptions
    {
        public const string SectionName = "Player";

        public int MaxActiveQuests { get; set; }
    }
}
