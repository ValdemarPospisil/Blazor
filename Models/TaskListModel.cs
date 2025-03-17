namespace Blazor.Models;

public class TaskListModel
{
    public string Name { get; set; } = string.Empty;
    public List<TaskModel> Tasks { get; set; } = new List<TaskModel>(); // Tasks in the list
    
}