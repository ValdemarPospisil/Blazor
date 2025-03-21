using Blazor.Models;
using System.Text.Json;

namespace Blazor.Services
{
    public class TaskService
    {
        private List<TaskListModel> taskLists = new List<TaskListModel>(); // List všech seznamů úkolů | List of all task lists
        private readonly string _filePath = "Data/tasks.json"; // Cesta k souboru pro ukládání dat | Path to the file for saving data

        public TaskListModel DefaultTaskList { get; private set; } // Výchozí seznam úkolů | Default task list
        
        /*
        Konstruktor třídy TaskService
        - Pokud neexistuje žádný seznam úkolů, vytvoří se výchozí seznam úkolů
        - Pokud existuje nějaký seznam úkolů, nastaví se výchozí seznam úkolů na první nalezený seznam
        Constructor of the TaskService class
        - If there is no task list, a default task list is created
        - If there is any task list, the default task list is set to the first found task list
        */
        public TaskService()
        {
            // Pokud neexistuje žádný seznam úkolů, vytvořte výchozí seznam
            if (!taskLists.Any())
            {
                DefaultTaskList = new TaskListModel { Name = "ToDo" }; // Vytvoření výchozího seznamu úkolů
                taskLists.Add(DefaultTaskList);      // Přidání výchozího seznamu do seznamu úkolů
            }
            else
            {
                // Nastavení výchozího seznamu úkolů pokud již existuje
                DefaultTaskList = taskLists.FirstOrDefault(tl => tl.Name == "ToDo") ?? taskLists.First(); 
            }
        }

        /*
        Metoda pro získání seznamu úkolů
        - Vrací List všech seznamů úkolů
        Method for getting the list of tasks
        - Returns a List of all task lists
        */
        public List<TaskListModel> GetTaskLists()
        {
            return taskLists;
        }

        /*
            Metoda pro přidaní seznamu úkolů podle názvu
            Method for adding the list of tasks by name
        */
        public void AddTaskList(string name)
        {
            taskLists.Add(new TaskListModel { Name = name });
        }

        /*
        Metoda pro přidání úkolu do seznamu úkolů
        - Pokud existuje seznam úkolů s názvem taskListName, přidá se úkol do tohoto seznamu
        Method for adding a task to the list of tasks
        - If there is a task list with the name taskListName, the task is added to this list
        */
        public void AddTask(string taskListName, string taskText)
        {
            var taskList = taskLists.FirstOrDefault(tl => tl.Name == taskListName);
            if (taskList != null)
            {
                taskList.Tasks.Add(new TaskModel { Text = taskText });
            }
        }

        

        /*
        Metoda pro smazání seznamu úkolů podle názvu
        - Pokud existuje seznam úkolů s názvem name, smaže se tento seznam úkolů
        Method for deleting the list of tasks by name
        - If there is a task list with the name name, this task list is deleted
        */
        public void RemoveTaskList(string name)
        {
            var taskList = taskLists.FirstOrDefault(tl => tl.Name == name);
            if (taskList != null)
            {
                taskLists.Remove(taskList);
            }
        }

        /*
        Metoda pro aktualizaci názvu seznamu úkolů
        - Pokud je název prázdný, vyvolá se výjimka
        - Pokud existuje seznam úkolů s novým názvem, vyvolá se výjimka
        - Pokud existuje seznam úkolů s původním názvem, aktualizuje se název toho seznamu úkolů
        Method for updating the name of the list of tasks
        - If the name is empty, an exception is thrown
        - If there is a task list with the new name, an exception is thrown
        - If there is a task list with the original name, the name of that task list is updated
        */

        public void UpdateTaskListName(TaskListModel updatedTaskList)
        {
            if (string.IsNullOrWhiteSpace(updatedTaskList.Name))
            {
                throw new ArgumentException("Název task listu nemůže být prázdný.");
            }

            if (taskLists.Any(tl => tl != updatedTaskList && tl.Name == updatedTaskList.Name))
            {
                throw new InvalidOperationException($"Task List s názvem '{updatedTaskList.Name}' už existuje.");
            }

            var existingTaskList = taskLists.FirstOrDefault(t => t == updatedTaskList);
            if (existingTaskList != null)
            {
                existingTaskList.Name = updatedTaskList.Name;
            }
        }

        /*
        Metoda pro smazání úkolu ze seznamu úkolů
        Method for deleting a task from the list of tasks
        */
        public void RemoveTask(TaskListModel taskList, TaskModel task)
        {
            taskList.Tasks.Remove(task);
        }

        /*
        Metoda pro uložení dat do JSON souboru
        Method for saving data to a JSON file
        */
        public async Task SaveTasksAsync()
        {
            var options = new JsonSerializerOptions { WriteIndented = true }; // Formátování JSONu
            string json = JsonSerializer.Serialize(taskLists, options);
            await File.WriteAllTextAsync(_filePath, json);
        }

        /*
        Metoda pro načtení dat ze souboru
        Method for loading data from a file
        */
        public async Task LoadTasksAsync()
        {
            if (File.Exists(_filePath))
            {
                string json = await File.ReadAllTextAsync(_filePath);
                taskLists = JsonSerializer.Deserialize<List<TaskListModel>>(json) ?? new List<TaskListModel>();
            }
        }
    }
}