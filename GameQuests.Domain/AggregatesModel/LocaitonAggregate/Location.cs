using GameQuests.Domain.SeedWork;

namespace GameQuests.Domain.AggregatesModel.LocaitonAggregate
{
    public class Location : GameObject
    {
        public Location(Position start, Position end, string name, string desciption)
        {
            Start = start;
            End = end;
            Name = name;
            Desciption = desciption;
        }

        public Position Start { get; private set; }

        public Position End { get; private set; }
    }
}
