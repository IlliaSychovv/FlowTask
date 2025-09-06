namespace FlowTask.Domain.Exceptions;

public class TaskAccessException : Exception
{
    public TaskAccessException()
        : base("Task not found or you do not have access to it")
    { }

    public TaskAccessException(string message)
        : base(message)
    { }
}