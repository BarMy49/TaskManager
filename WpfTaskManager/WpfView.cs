using System.Collections.ObjectModel;
using Figgle;
using System.ComponentModel;
using System.Windows;
using TaskManager.Localization;
using TaskManager.Model;
using WpfTaskManager;
using WpfTaskManager.Themes;

namespace TaskManager.View
{
    public class WpfView : IView, INotifyPropertyChanged
    {
        private readonly ILocalizer _localizer;
        
        // Dodaj tę właściwość do wyboru języka
        public List<string> AvailableLanguages { get; } = new List<string> { "pl", "en" };
        
        private string _selectedLanguage = "en";
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    _localizer.SetLanguage(value);
                    RefreshUI();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedLanguage)));
                }
            }
        }
        
        private readonly IThemeManager _themeManager;
        public List<string> AvailableThemes { get; } = Enum.GetNames(typeof(ThemeType)).ToList();

        private string _selectedTheme;
        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (_selectedTheme != value)
                {
                    _selectedTheme = value;
                    
                    var theme = Enum.GetValues(typeof(ThemeType))
                        .Cast<ThemeType>()
                        .FirstOrDefault(t => t.ToString() == value);
                    _themeManager.SetTheme(theme);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTheme)));
                }
            }
        }
        
        // Konstruktor z lokalizacją
        public WpfView(ILocalizer localizer, IThemeManager themeManager)
        {
            _localizer = localizer;
            _themeManager = themeManager;
            _themeManager.SetTheme(ThemeType.Task);
            _selectedLanguage = _localizer.CurrentLanguage;
            _selectedTheme = _themeManager.CurrentTheme.ToString();
        }
        
        // Odświeżanie UI po zmianie języka
        private void RefreshUI()
        {
            // Wywołaj PropertyChanged dla wszystkich tekstów UI
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty)); // Odświeża wszystko
        }
        
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
        public ObservableCollection<string> PriorityOptions { get; set; } = new ObservableCollection<string> { "Low", "Medium", "High" };
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
            System.Windows.MessageBox.Show(message, "", MessageBoxButton.OK);
        }

        public void DisplayFilterMenu(string[] options, int selectedOption)
        {
            Clear();
            Center(_localizer.GetString("Menu_FilterTasks"));
            WriteLine();
            Center(_localizer.GetString("Menu_Navigation"));
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
            Write(_localizer.GetString("Input_EnterCategory"));
            return ReadLine();
        }

        public bool GetFilterCompletionStatus()
        {
            Clear();
            Write(_localizer.GetString("Input_CompletionStatus"));
            int val;
            while (!int.TryParse(ReadLine(), out val) || (val != 0 && val != 1))
            {
                Write(_localizer.GetString("Input_InvalidChoice"));
            }
            return val == 1;
        }

        public DateTime GetFilterDate()
        {
            Write(_localizer.GetString("Input_EnterDate"));
            DateTime date;
            while (!DateTime.TryParseExact(ReadLine(), "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out date))
            {
                Write(_localizer.GetString("Input_InvalidDate"));
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
            Write(_localizer.GetString("Input_SearchKeyword"));
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
