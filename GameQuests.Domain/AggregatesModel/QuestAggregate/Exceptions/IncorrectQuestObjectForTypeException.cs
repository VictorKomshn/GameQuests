namespace GameQuests.Domain.AggregatesModel.QuestAggregate.Exceptions
{
    public class IncorrectQuestObjectForTypeException : Exception
    {
        public IncorrectQuestObjectForTypeException()
        {
        }

        public IncorrectQuestObjectForTypeException(string? message) : base(message)
        {
        }

        public IncorrectQuestObjectForTypeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

    }
}
