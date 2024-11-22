namespace ToDoTutorial.Core.Features.AddToDoTask;

public class TaskItem
{
    public TaskItem(Guid id, string task) =>
        (Id, Task) = (id, task);

    public Guid Id { get; init; }
    public string Task { get; init; }
    public bool Complete { get; set; }

}
