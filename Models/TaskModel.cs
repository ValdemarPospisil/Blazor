namespace Blazor.Models;

public class TaskModel
{
    public string Text { get; set; } = string.Empty; // Task name
    public string TaskListName { get; set; } = "ToDo"; // Reference to the task list it belongs to

    public string Description { get; set; } = "Write your own description"; // Task description
    public DateTime DueDate { get; set; } = DateTime.Now; // Due date of the task
}