using Blazored.LocalStorage;
using ToDoTutorial.Core.Features.AddToDoTask;
using ToDoTutorial.Core.Framework;

namespace ToDoTutorial.Core.Logic;

public class ToDoManager(ILogger<ToDoManager> logger, LocalStorageContext localStorageContext)
{
    private readonly ILogger<ToDoManager> _logger = logger;

    private readonly List<TaskItem> Items = [];

    private int GetTotalCount() => Items.Count;
    private int GetCompletedCount() => Items.FindAll(x => x.Complete).Count;
    private string GetTaskCountSingleOrPlural() => GetTotalCount() == 1 ? "Task" : "Tasks";

    public string GetCompletionString()
    {
        var result = "(Currently No Tasks)";
        if(GetTotalCount() > 0)
        {
            result = $"({GetCompletedCount()} of {GetTotalCount()}) {GetTaskCountSingleOrPlural()} Complete";
        }

        return result;            
    }

    public async Task<List<TaskItem>> GetItemsAsync()
    {
        if (Items.Count == 0 && localStorageContext is not null)
        {
            var items = await localStorageContext.GetItemsAsync<TaskItem>("Tasks");
            if (items is not null)
            {
                Items.AddRange(items);
                
            }   
        }
        return Items;
    }

    public async Task AddTaskAsync(string taskName)
    {
        if (!string.IsNullOrWhiteSpace(taskName))
        {            
            TaskItem item = new(Guid.CreateVersion7(), taskName);
            Items.Add(item);            
            await UpdateTaskItems();
            Log.TaskItemAdded(_logger, item.Id.ToString());
        }
    }

    public async Task ToggleComplete(Guid id)
    {
        var item = Items.Find(x => x.Id == id);
        if (item is not null)
        {
            item.Complete = !item.Complete;
            await UpdateTaskItems();
            Log.TaskItemCompletionChanged(_logger, item.Id.ToString(), item.Complete);
        }
    }

    public async Task DeleteTaskAsync(Guid id)
    {
        var item = Items.Find(x => x.Id == id);
        if (item is not null)
        {
            Items.Remove(item);
            await UpdateTaskItems();
            Log.TaskItemDeleted(_logger, item.Id.ToString());
        }
    }

    private async Task UpdateTaskItems()
    {
        if (localStorageContext is not null)
        {
            await localStorageContext.SetItemsAsync<TaskItem>("Tasks", Items);
        }        
    }
}
