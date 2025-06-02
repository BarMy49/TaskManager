using System.Text.Json;
using System.Text.Json.Serialization;

namespace TaskManager.Model
{
    public interface ITaskRepository
    {
        void AddTask(TaskModel task);
        bool RemoveTask(int id);
        bool UpdateTask(TaskModel task);
        List<TaskModel> GetAllTasks();
        TaskModel GetTaskById(int id);
        List<TaskModel> GetIncompleteTasks();
        List<TaskModel> GetCompleteTasks();
        List<TaskModel> GetTasksSortedByPriority();

        List<TaskModel> GetTasksByCategory(string category);
        List<TaskModel> GetTasksDueBefore(DateTime date);
        List<TaskModel> GetTasksDueAfter(DateTime date);
        List<TaskModel> GetTasksByCompletionStatus(bool isCompleted);
        List<TaskModel> SearchTasks(string keyword);

    }

    public class TaskRepository : ITaskRepository
    {
        private List<TaskModel> tasks;
        private readonly string filePath = "tasks.json";
        private int nextId;

        public TaskRepository()
        {
            tasks = new List<TaskModel>();
            LoadTasks();
            nextId = tasks.Any() ? tasks.Max(t => t.Id) + 1 : 1;
        }

        public void AddTask(TaskModel task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            task.Id = nextId++;
            tasks.Add(task);
            SaveTasks();
        }

        public bool RemoveTask(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                tasks.Remove(task);
                SaveTasks();
                return true;
            }
            return false;
        }

        public bool UpdateTask(TaskModel updatedTask)
        {
            if (updatedTask == null)
                throw new ArgumentNullException(nameof(updatedTask));

            var task = tasks.FirstOrDefault(t => t.Id == updatedTask.Id);
            if (task != null)
            {
                task.Title = updatedTask.Title;
                task.Description = updatedTask.Description;
                task.IsCompleted = updatedTask.IsCompleted;
                task.Category = updatedTask.Category;
                task.Priority = updatedTask.Priority;
                task.DueDate = updatedTask.DueDate;
                SaveTasks();
                return true;
            }
            return false;
        }

        public List<TaskModel> GetAllTasks()
        {
            return tasks;
        }

        public TaskModel GetTaskById(int id)
        {
            return tasks.FirstOrDefault(t => t.Id == id);
        }

        public List<TaskModel> GetIncompleteTasks()
        {
            return tasks.Where(t => !t.IsCompleted).ToList();
        }
        
        public List<TaskModel> GetCompleteTasks()
        {
            return tasks.Where(t => t.IsCompleted).ToList();
        }

        public List<TaskModel> GetTasksSortedByPriority()
        {
            return tasks.OrderByDescending(t => t.Priority).ToList();
        }

        private void SaveTasks()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new JsonStringEnumConverter() }
                };
                string json = JsonSerializer.Serialize(tasks, options);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas zapisywania danych: {ex.Message}");
            }
        }

        private void LoadTasks()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    var options = new JsonSerializerOptions
                    {
                        Converters = { new JsonStringEnumConverter() }
                    };
                    tasks = JsonSerializer.Deserialize<List<TaskModel>>(json, options);
                }
                else
                {
                    tasks = new List<TaskModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas wczytywania danych: {ex.Message}");
                tasks = new List<TaskModel>();
            }
        }

        public List<TaskModel> GetTasksByCategory(string category)
        {
            return tasks.Where(t => !string.IsNullOrEmpty(t.Category) &&
                t.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<TaskModel> GetTasksDueBefore(DateTime date)
        {
            return tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date <= date.Date).ToList();
        }

        public List<TaskModel> GetTasksDueAfter(DateTime date)
        {
            return tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date >= date.Date).ToList();
        }

        public List<TaskModel> GetTasksByCompletionStatus(bool isCompleted)
        {
            return tasks.Where(t => t.IsCompleted == isCompleted).ToList();
        }

        public List<TaskModel> SearchTasks(string keyword)
        {
            return tasks.Where(t =>
                (!string.IsNullOrEmpty(t.Title) && t.Title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (!string.IsNullOrEmpty(t.Description) && t.Description.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
            ).ToList();
        }

    }
}
