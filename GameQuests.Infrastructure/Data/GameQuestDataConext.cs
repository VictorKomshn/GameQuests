using GameQuests.Domain.AggregatesModel.PlayerAggregate;
using GameQuests.Domain.AggregatesModel.QuestAggregate;
using GameQuests.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace GameQuests.Infrastructure.Data
{
    public class GameQuestDataConext : DbContext
    {
        public GameQuestDataConext(DbContextOptions<GameQuestDataConext> options) : base(options)
        {

        }
        public DbSet<Player> Players { get; set; }

        public DbSet<Quest> Quests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerQuest>().HasKey(x => new { x.PlayerId, x.QuestId });

            modelBuilder.Entity<PlayerQuest>()
                .HasOne(pq => pq.Player)
                .WithMany(p => p.PlayerQuests)
                .HasForeignKey(pq => pq.PlayerId);

            modelBuilder.Entity<Quest>()
               .HasMany(pq => pq.Objectives)
               .WithOne(p => p.Quest)
               .HasForeignKey(pq => pq.QuestId);

            modelBuilder.Entity<PlayerQuest>()
            .HasOne(pq => pq.Quest);

            modelBuilder.Entity<Player>().OwnsOne(x => x.Position);

            base.OnModelCreating(modelBuilder);

        }
    }
}
