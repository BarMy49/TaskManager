using System.Diagnostics;
using TaskManager.Model;
using TaskManager.View;
using Microsoft.Toolkit.Uwp.Notifications;

namespace TaskManager.Controller
{
    public class TaskController
    {
        private readonly ITaskRepository repository;
        private readonly IView view;
        private readonly Func<ConsoleKey> input;

        public TaskController(ITaskRepository repository, IView view, Func<ConsoleKey> input)
        {
            this.repository = repository;
            this.view = view;
            this.input = input;
        }

        public void GetSelectionFromMenu()
        {
            var options = new[] {
                "Wyświetl wszystkie zadania",
                "Dodaj nowe zadanie",
                "Edytuj zadanie",
                "Usuń zadanie",
                "Oznacz zadanie jako ukończone/nieukończone",
                "Wyświetl zadania nieukończone",
                "Wyświetl zadania posortowane według priorytetu",
                "Filtrowanie zadań",
                "Wyszukiwanie zadań",
                "Wyjście"
            };

            int selectedOption = 0;
            bool isSelected = false;

            while (!isSelected)
            {
                view.DisplayMenu(options, selectedOption);
                var key = input();
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedOption = selectedOption == 0 ? options.Length - 1 : selectedOption - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedOption = selectedOption == options.Length - 1 ? 0 : selectedOption + 1;
                        break;
                    case ConsoleKey.Enter:
                        isSelected = true;
                        break;
                }
            }
            int choice = selectedOption;
            switch (choice)
            {
                case 0: ListAllTasks(); break;
                case 1: AddTask(); break;
                case 2: EditTask(); break;
                case 3: DeleteTask(); break;
                case 4: ToggleTaskCompletion(); break;
                case 5: ListIncompleteTasks(); break;
                case 6: ListTasksByPriority(); break;
                case 7: FilterTasks(GetSelectionFromFilterMenu()); break;
                case 8: SearchTasks(); break;
                case 9: Exit(); break;
                default:
                    view.DisplayMessage("Nieprawidłowy wybór. Naciśnij dowolny klawisz, aby kontynuować...");
                    break;
            }
        }
        
        private void Notification(string title, string message)
        {
            new ToastContentBuilder()
                .AddText(title)
                .AddText(message)
                .Show();
        }

        private void LaunchWindowsApp()
        {
            Process.Start(@"D:\Politechnika Białostocka KCK\OSTATECZNY GRAFIKA\WpfTaskManager\bin\Debug\net8.0-windows\WpfTaskManager.exe");
        }

        public void ListAllTasks()
        {
            var tasks = repository.GetAllTasks();
            view.DisplayTasks(tasks);
        }

        public void Exit()
        {
            view.DisplayMessage("Do widzenia!");
            Environment.Exit(0);
        }

        /// <summary>
        /// Dodawanie zadania + odświeżanie widoku
        /// </summary>
        public void AddTask()
        {
            try
            {
                var task = view.GetNewTaskDetails();
                repository.AddTask(task);
                //view.DisplayMessage("Zadanie dodane pomyślnie.");
                Notification($"{task.Title} DODANO POMYŚLNIE", "Zadanie zostało dodane do listy zadań.");
                // Odświeżanie widoku - wyświetlamy wszystkie zadania
                ListAllTasks();
            }
            catch (Exception ex)
            {
                view.DisplayMessage($"Błąd: {ex.Message}");
            }
        }

        /// <summary>
        /// Edycja zadania + odświeżanie widoku
        /// </summary>
        public void EditTask()
        {
            int id = view.GetTaskId();
            var task = repository.GetTaskById(id);
            if (task != null)
            {
                try
                {
                    var updatedTask = view.GetUpdatedTaskDetails(task);
                    repository.UpdateTask(updatedTask);
                    view.DisplayMessage("Zadanie zaktualizowane pomyślnie.");

                    // Odświeżanie
                    ListAllTasks();
                }
                catch (Exception ex)
                {
                    view.DisplayMessage($"Błąd: {ex.Message}");
                }
            }
            else
            {
                view.DisplayMessage("Zadanie o podanym ID nie istnieje.");
            }
        }

        /// <summary>
        /// Usuwanie zadania + odświeżanie widoku
        /// </summary>
        public void DeleteTask()
        {
            int id = view.GetTaskId();
            if (repository.RemoveTask(id))
            {
                view.DisplayMessage("Zadanie usunięte pomyślnie.");

                // Odświeżanie
                ListAllTasks();
            }
            else
            {
                view.DisplayMessage("Zadanie o podanym ID nie istnieje.");
            }
        }

        /// <summary>
        /// Oznaczanie zadania jako ukończone/nieukończone + odświeżanie widoku
        /// </summary>
        public void ToggleTaskCompletion()
        {
            int id = view.GetTaskId();
            var task = repository.GetTaskById(id);
            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted;
                repository.UpdateTask(task);
                // view.DisplayMessage($"Zadanie oznaczone jako {(task.IsCompleted ? "ukończone" : "nieukończone")}.");

                // Odświeżanie
                // ListAllTasks();
            }
            else
            {
                view.DisplayMessage("Zadanie o podanym ID nie istnieje.");
            }
        }


        public void ListIncompleteTasks()
        {
            var tasks = repository.GetIncompleteTasks();
            view.DisplayTasks(tasks);
        }

        public void ListTasksByPriority()
        {
            var tasks = repository.GetTasksSortedByPriority();
            view.DisplayTasks(tasks);
        }

        public int GetSelectionFromFilterMenu()
        {
            var options = new[] {
                "Filtrowanie według kategorii",
                "Filtrowanie według terminu (przed datą)",
                "Filtrowanie według terminu (po dacie)",
                "Filtrowanie według statusu ukończenia",
                "Powrót do głównego menu"
            };

            int selectedOption = 0;
            bool isSelected = false;

            while (!isSelected)
            {
                view.DisplayFilterMenu(options, selectedOption);
                var key = input();
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedOption = selectedOption == 0 ? options.Length - 1 : selectedOption - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedOption = selectedOption == options.Length - 1 ? 0 : selectedOption + 1;
                        break;
                    case ConsoleKey.Enter:
                        isSelected = true;
                        break;
                }
            }
            return selectedOption;
        }

        public void FilterTasks(int option)
        {
            int filterChoice = option;
            switch (filterChoice)
            {
                case 0:
                    FilterTasksByCategory();
                    break;
                case 1:
                    FilterTasksDueBefore();
                    break;
                case 2:
                    FilterTasksDueAfter();
                    break;
                case 3:
                    FilterTasksByCompletionStatus();
                    break;
                case 4:
                    GetSelectionFromMenu();
                    break;
                default:
                    view.DisplayMessage("Nieprawidłowy wybór. Naciśnij dowolny klawisz, aby kontynuować...");
                    break;
            }
        }

        private void FilterTasksByCategory()
        {
            string category = view.GetFilterCategory();
            var tasks = repository.GetTasksByCategory(category);
            view.DisplayTasks(tasks);
        }

        private void FilterTasksDueBefore()
        {
            DateTime date = view.GetFilterDate();
            var tasks = repository.GetTasksDueBefore(date);
            view.DisplayTasks(tasks);
        }

        private void FilterTasksDueAfter()
        {
            DateTime date = view.GetFilterDate();
            var tasks = repository.GetTasksDueAfter(date);
            view.DisplayTasks(tasks);
        }

        private void FilterTasksByCompletionStatus()
        {
            bool isCompleted = view.GetFilterCompletionStatus();
            var tasks = repository.GetTasksByCompletionStatus(isCompleted);
            view.DisplayTasks(tasks);
        }

        public void SearchTasks()
        {
            string keyword = view.GetSearchKeyword();
            var tasks = repository.SearchTasks(keyword);
            view.DisplayTasks(tasks);
        }

        public void SearchTaskById()
        {
            int id = view.GetTaskId();
            var found = repository.GetTaskById(id);
            if (found != null)
            {
                view.DisplayTasks(new List<TaskModel> { found });
            }
            else
            {
                view.DisplayMessage($"Nie znaleziono zadania o ID = {id}");
            }
        }
    }
}
