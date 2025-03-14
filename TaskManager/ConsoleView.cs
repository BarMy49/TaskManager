using Figgle;
using System.Text;
using TaskManager.Model;

namespace TaskManager.View
{
    public class ConsoleView : IView
    {
        public void DisplayMenu(string[] options, int selectedOption)
        {
            Console.Clear();
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            DisplayMessageFiggle("Task Manager");
            Console.WriteLine();
            Console.ResetColor();
            Center("Użyj strzałek 🠕 i 🠗 do nawigacji oraz Enter, aby wybrać opcję:");
            Console.WriteLine();

            (int left, int top) = Console.GetCursorPosition();

            for (int i = 0; i < options.Length; i++)
            {
                Console.SetCursorPosition(left, top + i);
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Center($"✅ {options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Center($"   {options[i]}");
                }
            }
        }

        public int GetUserChoice()
        {
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.Write("Nieprawidłowy wybór. Wprowadź liczbę: ");
            }
            return choice;
        }

        public void DisplayTasks(List<TaskModel> tasks)
        {
            Console.Clear();
            if (tasks.Count == 0)
            {
                Console.WriteLine("Brak zadań do wyświetlenia.");
            }
            else
            {
                Center("=== Lista zadań ===");

                Console.WriteLine("{0,5} | {1,-20} | {2,-15} | {3,-12} | {4,-10} | {5,-12} | {6,-40}|",
                    "ID", "Tytuł", "Status", "Kategoria", "Priorytet", "Termin", "Opis");

                Console.WriteLine(new string('-', 133));

                foreach (var task in tasks)
                {
                    Console.WriteLine("{0,5} | {1,-20} | {2,-15} | {3,-12} | {4,10} | {5,-12} | {6,-40}|",
                        task.Id,
                        task.Title,
                        task.IsCompleted ? "Ukończone" : "Nieukończone",
                        task.Category,
                        task.Priority,
                        task.DueDate?.ToString("dd-MM-yyyy") ?? "Brak",
                        task.Description
                    );
                }
            }
            Center("Naciśnij dowolny klawisz, aby kontynuować...");
            Console.ReadKey();
        }


        public TaskModel GetNewTaskDetails()
        {
            Console.Clear();
            Center("=== Dodawanie nowego zadania ===");
            Console.Write("Tytuł: ");
            string title = Console.ReadLine();

            Console.Write("Opis (opcjonalnie | max 30 znaków): ");
            string description = Console.ReadLine();

            Console.Write("Kategoria (opcjonalnie): ");
            string category = Console.ReadLine();

            Console.Write("Priorytet (1 - Niski, 2 - Średni, 3 - Wysoki): ");
            TaskPriority priority = GetTaskPriority();

            Console.Write("Termin (dd-MM-rrrr) (opcjonalnie): ");
            DateTime? dueDate = GetDueDate();

            return new TaskModel(title, description, category, priority, dueDate);
        }

        public int GetTaskId()
        {
            Console.Write("Wprowadź ID zadania: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.Write("Nieprawidłowe ID. Wprowadź liczbę: ");
            }
            return id;
        }

        public TaskModel GetUpdatedTaskDetails(TaskModel task)
        {
            Console.Clear();
            Center("=== Edycja zadania ===");
            Console.WriteLine("Pozostaw pole puste, aby nie zmieniać wartości.");

            Console.Write($"Tytuł ({task.Title}): ");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
                task.Title = title;

            Console.Write($"Opis ({task.Description}): ");
            string description = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(description))
                task.Description = description;

            Console.Write($"Kategoria ({task.Category}): ");
            string category = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(category))
                task.Category = category;

            Console.Write($"Priorytet ({(int)task.Priority} - {task.Priority}) (1 - Niski, 2 - Średni, 3 - Wysoki): ");
            TaskPriority? priority = GetOptionalTaskPriority();
            if (priority.HasValue)
                task.Priority = priority.Value;

            Console.Write($"Termin ({task.DueDate?.ToString("dd-MM-yyyy")}): ");
            DateTime? dueDate = GetDueDate();
            if (dueDate.HasValue)
                task.DueDate = dueDate;

            return task;
        }

        public void DisplayMessage(string message)
        {
            Center(message);
            Center("Naciśnij dowolny klawisz, aby kontynuować...");
            Console.ReadKey();
        }

        private TaskPriority GetTaskPriority()
        {
            int priorityValue;
            while (!int.TryParse(Console.ReadLine(), out priorityValue) ||
                   !Enum.IsDefined(typeof(TaskPriority), priorityValue))
            {
                Console.Write("Nieprawidłowy priorytet. Wprowadź 1 (Niski), 2 (Średni) lub 3 (Wysoki): ");
            }
            return (TaskPriority)priorityValue;
        }

        private TaskPriority? GetOptionalTaskPriority()
        {
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                return null;

            int priorityValue;
            while (!int.TryParse(input, out priorityValue) ||
                   !Enum.IsDefined(typeof(TaskPriority), priorityValue))
            {
                Console.Write("Nieprawidłowy priorytet. Wprowadź 1 (Niski), 2 (Średni) lub 3 (Wysoki): ");
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    return null;
            }
            return (TaskPriority)priorityValue;
        }

        private DateTime? GetDueDate()
        {
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                return null;

            DateTime dueDate;
            while (!DateTime.TryParseExact(input, "dd-MM-yyyy", null,
                    System.Globalization.DateTimeStyles.None, out dueDate))
            {
                Console.Write("Nieprawidłowy format daty. Wprowadź w formacie dd-MM-rrrr: ");
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    return null;
            }
            return dueDate;
        }


        public void DisplayFilterMenu(string[] options, int selectedOption)
        {
            Console.Clear();
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Center("=== Filtrowanie zadań ===");
            Console.WriteLine();
            Console.ResetColor();
            Center("Użyj strzałek 🠕 i 🠗 do nawigacji oraz Enter, aby wybrać opcję:");
            Console.WriteLine();

            (int left, int top) = Console.GetCursorPosition();

            for (int i = 0; i < options.Length; i++)
            {
                Console.SetCursorPosition(left, top + i);
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Center($"✅ {options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Center($"   {options[i]}");
                }
            }
        }

        public string GetFilterCategory()
        {
            Console.Write("Wprowadź nazwę kategorii: ");
            return Console.ReadLine();
        }

        public DateTime GetFilterDate()
        {
            Console.Write("Wprowadź datę (dd-MM-yyyy): ");
            DateTime date;
            while (!DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out date))
            {
                Console.Write("Nieprawidłowy format daty. Wprowadź w formacie dd-MM-yyyy: ");
            }
            return date;
        }


        public bool GetFilterCompletionStatus()
        {
            Console.Clear();
            Console.Write("Wprowadź status ukończenia (1 - Ukończone, 0 - Nieukończone): ");
            int input;
            while (!int.TryParse(Console.ReadLine(), out input) || (input != 0 && input != 1))
            {
                Console.Write("Nieprawidłowy wybór. Wprowadź 1 (Ukończone) lub 0 (Nieukończone): ");
            }
            return input == 1;
        }

        public string GetSearchKeyword()
        {
            Console.Write("Wprowadź słowo kluczowe do wyszukania: ");
            return Console.ReadLine();
        }

        public void Center(string message)
        {
            int screenWidth = Console.WindowWidth;
            int stringWidth = message.Length;
            int spaces = (screenWidth / 2) + (stringWidth / 2);

            Console.WriteLine(message.PadLeft(spaces));
        }

        public static void DisplayMessageFiggle(string message)
        {
            string figgleText = FiggleFonts.Big.Render(message);
            int consoleWidth = Console.WindowWidth;
            string[] lines = figgleText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                int spaces = (consoleWidth - line.Length) / 2;
                Console.WriteLine(new string(' ', spaces) + line);
            }
        }
    }
}
