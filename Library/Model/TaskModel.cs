using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaskManager.Model
{
    public enum TaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }

    public interface ITask
    {
        int Id { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        bool IsCompleted { get; set; }
        string Category { get; set; }
        TaskPriority Priority { get; set; }
        DateTime? DueDate { get; set; }
    }

    public class TaskModel : ITask
    {
        public int Id { get; set; }

        private string title;
        private string description;
        public string Title
        {
            get => title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tytuł zadania nie może być pusty.");
                title = value;
            }
        }

        public const int MaxLength = 40;

        public string Description
        {
            get => description;
            set
            {
                if (value != null && value.Length > MaxLength)
                    throw new ArgumentException($"Opis nie może być dłuższy niż {MaxLength} znaków!.");
                description = value;
            }
        }

        public bool IsCompleted { get; set; }

        public string Category { get; set; }

        public TaskPriority Priority { get; set; }

        public DateTime? DueDate { get; set; }

        public TaskModel()
        {
            IsCompleted = false;
            Priority = TaskPriority.Medium;
        }

        public TaskModel(string title, string description = "", string category = "", TaskPriority priority = TaskPriority.Medium, DateTime? dueDate = null)
        {
            Title = title;
            Description = description;
            Category = category;
            Priority = priority;
            DueDate = dueDate;
            IsCompleted = false;
        }

        public override string ToString()
        {
            string status = IsCompleted ? "Ukończone" : "Nieukończone";
            return $"ID: {Id}\nTytuł: {Title}\nOpis: {Description}\nStatus: {status}\nKategoria: {Category}\nPriorytet: {Priority}\nTermin: {DueDate?.ToString("dd-MM-yyyy")}\n";
        }
    }
}
