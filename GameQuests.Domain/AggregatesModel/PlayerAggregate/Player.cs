using GameQuests.Domain.AggregatesModel.EnemyAggregate;
using GameQuests.Domain.AggregatesModel.ItemAggregate;
using GameQuests.Domain.AggregatesModel.LocaitonAggregate;
using GameQuests.Domain.AggregatesModel.QuestAggregate;
using GameQuests.Domain.AggregatesModel.QuestAggregate.Abstract;
using GameQuests.Domain.AggregatesModel.QuestAggregate.Exceptions;
using GameQuests.Domain.SeedWork;
using GameQuests.Domain.SeedWork.Base;
using GameQuests.Domain.SeedWork.Exceptions;

namespace GameQuests.Domain.AggregatesModel.PlayerAggregate
{
    public class Player : BaseEntity
    {
        public Player()
        {
            Id = Guid.NewGuid();
            Name = "player_" + DateTime.UtcNow.GetHashCode();
            Lvl = 1;
            GoldAmount = 0;
            Position = new Position(0, 0);
            Inventory = new List<Item>();
            PlayerQuests = new List<PlayerQuest>();
        }

        public Player(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Lvl = 1;
            GoldAmount = 0;
            Position = new Position(0, 0);
            Inventory = new List<Item>();
            PlayerQuests = new List<PlayerQuest>();
        }

        /// <summary>
        /// Никнейм
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Уровень
        /// </summary>
        public int Lvl { get; set; }

        /// <summary>
        /// Количество золота
        /// </summary>
        public int GoldAmount { get; set; }

        /// <summary>
        /// Инвентарь игрока
        /// </summary>
        public ICollection<Item> Inventory { get; set; }

        /// <summary>
        /// Положение игрока на карте
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// Квесты игрока
        /// </summary>
        public ICollection<PlayerQuest> PlayerQuests { get; set; }

        public IEnumerable<PlayerQuest>? CheckQuests(IQuestObjective gameObject) =>
            gameObject switch
            {
                Position position => UpdateLocationQuestProgress(position),
                Item item => UpdateQuestsProgress(QuestType.Collect, item),
                Enemy enemy => UpdateQuestsProgress(QuestType.Eliminate, enemy),
                _ => throw new NotFoundException("")
            };

        private IEnumerable<PlayerQuest>? UpdateQuestsProgress(QuestType questType, IQuestObjective questObjective)
        {
            IEnumerable<PlayerQuest>? requiredQuest = null;
            GameObject? gameObject = questObjective as GameObject;
            if (gameObject != null)
            {
                requiredQuest = PlayerQuests.Where(x => x.Quest.QuestType == questType)
                                                  .Where(x =>
                                                  {
                                                      var objective = x.Quest.Objectives.FirstOrDefault(x => x.ObjectiveGameObject.Id == gameObject.Id);

                                                      return objective != null;
                                                  });
            }

            if (requiredQuest == null || !requiredQuest.Any())
            {
                return null;
            }

            IncimentQuestProgress(requiredQuest);
            return requiredQuest;
        }

        private IEnumerable<PlayerQuest>? UpdateLocationQuestProgress(Position position)
        {
            try
            {
                var locationQuests = PlayerQuests.Where(x => x.Quest.QuestType == QuestType.Visit);

                if (locationQuests is null || !locationQuests.Any())
                {
                    return null;
                }

                var relatedQuests = locationQuests.Where(x => x.Quest.Objectives.Any(y =>
                    {
                        var questLocaiton = y.ObjectiveGameObject as Location;
                        if (questLocaiton == null)
                        {
                            return false;
                        };
                        return questLocaiton.Start <= position && questLocaiton.End > position;
                    }));

                IncimentQuestProgress(relatedQuests);
                return relatedQuests;
            }
            catch
            {
                throw;
            }
        }

        private void IncimentQuestProgress(IEnumerable<PlayerQuest> quests)
        {
            if (quests == null)
            {
                throw new Exception("");
            }

            foreach (var quest in quests)
            {
                if (quest.Quest.QuestType == QuestType.Collect)
                {
                    quest.Progress = Inventory.Count(x => quest.Quest.Objectives.Any(y => y.ObjectiveGameObject.Id == x.Id));
                }
                else
                {
                    quest.Progress++;
                }

                if (quest.Progress >= quest.Quest.Objectives.Sum(x => x.Amount))
                {
                    quest.Status = PlayerQuestStatus.Completed;

                    foreach (var reward in quest.Quest.Reward)
                    {
                        Inventory.Add(reward);
                    }

                    quest.Status = PlayerQuestStatus.Finished;
                }
            }
        }
    }
}
