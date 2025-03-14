using Figgle;
using System.ComponentModel;
using System.Windows;
using TaskManager.Model;
using WpfTaskManager;

namespace TaskManager.View
{
    public class WpfView : IView, INotifyPropertyChanged
    {
        // Widoczność głównej sekcji pól
        private Visibility _fieldsVisibility = Visibility.Collapsed;
        public Visibility FieldsVisibility
        {
            get => _fieldsVisibility;
            set
            {
                _fieldsVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FieldsVisibility)));
            }
        }
        // Przyciski "Dodaj" i "Edytuj"
        private Visibility _addConfirmVisibility = Visibility.Collapsed;
        public Visibility AddConfirmVisibility
        {
            get => _addConfirmVisibility;
            set
            {
                _addConfirmVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddConfirmVisibility)));
            }
        }

        private Visibility _editConfirmVisibility = Visibility.Collapsed;
        public Visibility EditConfirmVisibility
        {
            get => _editConfirmVisibility;
            set
            {
                _editConfirmVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditConfirmVisibility)));
            }
        }

        // Usuwanie
        private Visibility _deleteVisibility = Visibility.Collapsed;
        public Visibility DeleteVisibility
        {
            get => _deleteVisibility;
            set
            {
                _deleteVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeleteVisibility)));
            }
        }
        public string? DeleteId { get; set; }

        // Togglowanie
        private Visibility _toggleVisibility = Visibility.Collapsed;
        public Visibility ToggleVisibility
        {
            get => _toggleVisibility;
            set
            {
                _toggleVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ToggleVisibility)));
            }
        }
        public string? ToggleId { get; set; }

        // Edycja
        private Visibility _editVisibility = Visibility.Collapsed;
        public Visibility EditVisibility
        {
            get => _editVisibility;
            set
            {
                _editVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditVisibility)));
            }
        }
        public string? EditId { get; set; }

        //Filtr kategorii
        private Visibility _categoryFilterVisibility = Visibility.Collapsed;
        public Visibility CategoryFilterVisibility
        {
            get => _categoryFilterVisibility;
            set
            {
                _categoryFilterVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryFilterVisibility)));
            }
        }
        public string? CategoryFilterText { get; set; }

        // Pola do dodawania/edycji
        public bool Ignore { get; set; }
        public MainWindow? Window { get; set; }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public string? Category { get; set; }
        public TaskPriority? Priority { get; set; }
        public DateTime? DueDate { get; set; }

        public string? Input { get; set; }
        public string? Text { get; set; }
        public List<TaskModel> Records { get; set; } = [];

        private const int Width = 50;
        private ConsoleKey lastKey = ConsoleKey.UpArrow;
        private int i = 0;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ConsoleKey WaitForKey()
        {
            if (Ignore)
                return ConsoleKey.Enter;

            while (i != 0)
                Thread.Sleep(10);
            i = 1;
            return lastKey;
        }

        public void SetKey(ConsoleKey key)
        {
            i = 0;
            lastKey = key;
        }

        private string ReadLine()
        {
            string input = Input ?? string.Empty;
            Input = null;
            return input;
        }

        public void ClearTaskFields()
        {
            Title = null;
            Description = null;
            Category = null;
            Priority = null;
            DueDate = null;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Category)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Priority)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DueDate)));
        }

        private void Clear()
        {
            if (Window is not null)
                Text = string.Empty;
            PropertyChanged?.Invoke(this, new(nameof(Text)));
        }

        private void WriteLine(string line = "")
        {
            if (Window is not null)
                Text += line + "\n";
            PropertyChanged?.Invoke(this, new(nameof(Text)));
        }

        private void Write(string line = "")
        {
            if (Window is not null)
                Text += line;
            PropertyChanged?.Invoke(this, new(nameof(Text)));
        }

        private (int x, int y) GetCursorPosition() => (0, 0);
        private void SetCursorPosition(int x, int y) { /*pusto*/ }

        public void DisplayMenu(string[] options, int selectedOption) { }
        public int GetUserChoice()
        {
            int.TryParse(ReadLine(), out var choice);
            return choice;
        }

        public void DisplayMessage(string message)
        {
            MessageBox.Show(message, "", MessageBoxButton.OK);
        }

        public void DisplayFilterMenu(string[] options, int selectedOption)
        {
            Clear();
            Center("=== Filtrowanie zadań ===");
            WriteLine();
            Center("Użyj strzałek 🠕 i 🠗 do nawigacji oraz Enter, aby wybrać opcję:");
            WriteLine();

            (int left, int top) = GetCursorPosition();
            for (int i = 0; i < options.Length; i++)
            {
                SetCursorPosition(left, top + i);
                if (i == selectedOption)
                {
                    Center($"✅ {options[i]}");
                }
                else
                {
                    Center($"   {options[i]}");
                }
            }
        }

        public void DisplayTasks(List<TaskModel> tasks)
        {
            Records.Clear();
            Records = tasks.ToList();
            PropertyChanged?.Invoke(this, new(nameof(Records)));
        }

        // Zwracamy CategoryFilterText jeśli jest wpisany.
        public string GetFilterCategory()
        {
            if (!string.IsNullOrWhiteSpace(CategoryFilterText))
            {
                return CategoryFilterText;
            }
            return ReadLine();
        }

        public bool GetFilterCompletionStatus()
        {
            Clear();
            Write("Wprowadź status ukończenia (1 - Ukończone, 0 - Nieukończone): ");
            int val;
            while (!int.TryParse(ReadLine(), out val) || (val != 0 && val != 1))
            {
                Write("Nieprawidłowy wybór. Wprowadź 1 (Ukończone) lub 0 (Nieukończone): ");
            }
            return val == 1;
        }

        public DateTime GetFilterDate()
        {
            Write("Wprowadź datę (dd-MM-yyyy): ");
            DateTime date;
            while (!DateTime.TryParseExact(ReadLine(), "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out date))
            {
                Write("Nieprawidłowy format daty. Wprowadź w formacie dd-MM-yyyy: ");
            }
            return date;
        }

        public int GetTaskId()
        {
            if (!string.IsNullOrWhiteSpace(EditId))
            {
                int.TryParse(EditId, out var editVal);
                return editVal;
            }
            if (!string.IsNullOrWhiteSpace(DeleteId))
            {
                int.TryParse(DeleteId, out var deleteVal);
                return deleteVal;
            }
            if (!string.IsNullOrWhiteSpace(ToggleId))
            {
                int.TryParse(ToggleId, out var toggleVal);
                return toggleVal;
            }
            int.TryParse(ReadLine(), out var fallbackVal);
            return fallbackVal;
        }

        public TaskModel GetNewTaskDetails()
        {
            Clear();
            if (string.IsNullOrWhiteSpace(Title))
                throw new ArgumentException("Title is required!");
            if (string.IsNullOrWhiteSpace(Category))
                throw new ArgumentException("Category is required!");
            if (!Priority.HasValue)
                throw new ArgumentException("Priority is required!");

            return new TaskModel(
                Title ?? string.Empty,
                Description ?? string.Empty,
                Category ?? string.Empty,
                Priority.Value,
                DueDate
            );
        }

        public TaskModel GetUpdatedTaskDetails(TaskModel task)
        {
            if (!string.IsNullOrWhiteSpace(Title))
                task.Title = Title;
            if (!string.IsNullOrWhiteSpace(Description))
                task.Description = Description;
            if (!string.IsNullOrWhiteSpace(Category))
                task.Category = Category;
            if (Priority.HasValue)
                task.Priority = Priority.Value;
            if (DueDate.HasValue)
                task.DueDate = DueDate;

            return task;
        }

        public string GetSearchKeyword()
        {
            Write("Wprowadź słowo kluczowe do wyszukania: ");
            return ReadLine();
        }

        public void Center(string message)
        {
            int screenWidth = Width;
            int stringWidth = message.Length;
            int spaces = (screenWidth / 2) + (stringWidth / 2);
            WriteLine(message.PadLeft(spaces));
        }

        public void DisplayMessageFiggle(string message)
        {
            string figgleText = FiggleFonts.Big.Render(message);
            foreach (var line in figgleText.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
            {
                WriteLine(line);
            }
        }
    }
}
