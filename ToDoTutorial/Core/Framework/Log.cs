namespace ToDoTutorial.Core.Framework;

using Microsoft.Extensions.Logging;

internal static partial class Log
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "A Task Item with Id {Id} was added to localStorage.")]
    public static partial void TaskItemAdded(ILogger logger, string Id);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "A Task Item with Id {Id} was deleted from localStorage.")]
    public static partial void TaskItemDeleted(ILogger logger, string Id);

    [LoggerMessage(EventId = 3, Level = LogLevel.Information, Message = "A Task Item with Id {Id} had its Complete property set to {Completed}")]
    public static partial void TaskItemCompletionChanged(ILogger logger, string Id, bool Completed);


}


