using GameQuests.Domain.AggregatesModel.QuestAggregate.Abstract;

namespace GameQuests.Domain.SeedWork
{
    public class Position : IQuestObjective
    {

        public Position()
        {
            X = 0;
            Y = 0;
        }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public static Position operator +(Position a, Position b)
        {
            Position? result = null;

            if (a == null && b == null)
            {
                result = new Position();
            }
            else if (a != null && b == null)
            {
                result = new Position(a.X, a.Y);
            }
            else if (a == null && b != null)
            {
                result = new Position(b.X, b.Y);
            }
            else
            {
                result = new Position(a!.X + b!.X, a.Y + b.Y);
            }
            return result;
        }

        public static Position operator -(Position? a, Position? b)
        {
            Position? result = null;

            if (a == null && b == null)
            {
                result = new Position();
            }
            else if (a != null && b == null)
            {
                result = new Position(-a.X, -a.Y);
            }
            else if (a == null && b != null)
            {
                result = new Position(-b.X, -b.Y);
            }
            else
            {
                result = new Position(a!.X - b!.X, a.Y - b.Y);
            }
            return result;
        }

        public static bool operator >(Position? a, Position? b)
        {
            if (a == null || b == null)
            {
                return false;
            }
            return a.X > b.X && a.Y > b.Y;
        }

        public static bool operator >=(Position? a, Position? b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            else if (a == null || b == null)
            {
                return false;
            }
            return a.X >= b.X && a.Y >= b.Y;
        }

        public static bool operator <(Position? a, Position? b)
        {
            if (a == null || b == null)
            {
                return false;
            }
            return a.X < b.X && a.Y < b.Y;
        }

        public static bool operator <=(Position? a, Position? b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            else if (a == null || b == null)
            {
                return false;
            }
            return a.X <= b.X && a.Y <= b.Y;
        }

        public static bool operator ==(Position? a, Position? b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            else if (a is null || b is null)
            {
                return false;
            }
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Position? a, Position? b)
        {
            if (a is null && b is null)
            {
                return false;
            }
            else if (a is null || b is null)
            {
                return true;
            }
            return a.X != b.X || a.Y != b.Y;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            var position = obj as Position;
            if (position != null)
            {
                return X == position.X && Y == position.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }
    }
}
