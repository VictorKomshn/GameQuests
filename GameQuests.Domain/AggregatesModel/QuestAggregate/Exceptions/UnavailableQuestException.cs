namespace GameQuests.Domain.AggregatesModel.QuestAggregate.Exceptions
{
    public class UnavailableQuestException : Exception
    {
        public UnavailableQuestException()
        {
        }

        public UnavailableQuestException(string? message) : base(message)
        {
        }

        public UnavailableQuestException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
