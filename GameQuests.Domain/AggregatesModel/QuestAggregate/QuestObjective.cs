using GameQuests.Domain.SeedWork;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GameQuests.Domain.AggregatesModel.QuestAggregate
{
    public class QuestObjective
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid QuestId { get; private set; }


        private Quest _quest;

        public Quest Quest
        {
            get
            {
                return _quest;
            }
            set
            {
                _quest = value;
                QuestId = value.Id;
            }
        }

        public GameObject ObjectiveGameObject { get; set; }

        public int Amount { get; set; }
    }
}
