using GameQuests.Domain.AggregatesModel.QuestAggregate;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameQuests.Domain.AggregatesModel.PlayerAggregate
{
    public class PlayerQuest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid PlayerId { get; set; }

        public Player Player { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid QuestId { get; set; }

        public Quest Quest { get; set; }

        public PlayerQuestStatus Status { get; set; }

        public int Progress { get; set; }

    }
}
