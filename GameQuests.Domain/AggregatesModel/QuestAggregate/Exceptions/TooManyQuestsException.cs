namespace GameQuests.Domain.AggregatesModel.QuestAggregate.Exceptions
{
    public class TooManyQuestsException : Exception
    {
        public TooManyQuestsException()
        {
        }

        public TooManyQuestsException(string? message) : base(message)
        {
        }

        public TooManyQuestsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
