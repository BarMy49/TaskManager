using System;
using System.Collections.Generic;
using System.Resources;
using System.Windows;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using WpfTaskManager;

namespace TaskManager.Localization
{
    public class ResourceLocalizer : ILocalizer, INotifyPropertyChanged
    {
        private readonly ResourceManager _resourceManager;
        private CultureInfo _currentCulture;
        public string this[string key] => GetString(key);

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ResourceLocalizer()
        {
            _resourceManager = new ResourceManager("WpfTaskManager.Resources.Strings", Assembly.GetExecutingAssembly());
            SetLanguage("en"); // Domyślnie angielski
        }

        public string GetString(string key)
        {
            try
            {
                string value = _resourceManager.GetString(key, _currentCulture);
                return string.IsNullOrEmpty(value) ? key : value;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return key;
            }
        }

        public void SetLanguage(string language)
        {
            switch (language.ToLower())
            {
                case "en":
                    _currentCulture = new CultureInfo("en-US");
                    break;
                default:
                    _currentCulture = new CultureInfo("pl-PL");
                    break;
            }
            
            // Aktualizacja zasobów aplikacji
            UpdateApplicationResources();
            
            // Powiadom o zmianie wszystkich właściwości
            OnPropertyChanged(string.Empty);
            
            var mw = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();
            mw?.RefreshColumnHeaders();
        }
        
        private void UpdateApplicationResources()
        {
            var app = Application.Current;
            if (app == null) return;
    
            app.Resources["SearchText"] = GetString("Search");
            app.Resources["TitleText"] = GetString("Title");
            app.Resources["DescriptionText"] = GetString("Description");
            app.Resources["StatusText"] = GetString("Status");
            app.Resources["CategoryText"] = GetString("Category");
            app.Resources["PriorityText"] = GetString("Priority");
            app.Resources["DueDateText"] = GetString("DueDate");
            app.Resources["PriorityFieldText"] = GetString("PriorityField");
            app.Resources["DueDateFieldText"] = GetString("DueDateField");
            app.Resources["AddText"] = GetString("Add");
            app.Resources["EditText"] = GetString("Edit");
            app.Resources["EnterDeleteIdText"] = GetString("EnterDeleteId");
            app.Resources["DeleteText"] = GetString("Delete");
            app.Resources["EnterToggleIdText"] = GetString("EnterToggleId");
            app.Resources["ChangeStatusText"] = GetString("ChangeStatus");
            app.Resources["EnterEditIdText"] = GetString("EnterEditId");
            app.Resources["SearchByIdText"] = GetString("SearchById");
            app.Resources["SearchByCategoryText"] = GetString("SearchByCategory");
            app.Resources["AllTasksText"] = GetString("AllTasks");
            app.Resources["AddTaskText"] = GetString("AddTask");
            app.Resources["EditTaskText"] = GetString("EditTask");
            app.Resources["DeleteTaskText"] = GetString("DeleteTask");
            app.Resources["ToggleTaskText"] = GetString("ToggleTask");
            app.Resources["ListIncompleteText"] = GetString("ListIncomplete");
            app.Resources["ListByPriorityText"] = GetString("ListByPriority");
            app.Resources["FilterByCategoryText"] = GetString("FilterByCategory");
            app.Resources["ExitText"] = GetString("Exit");
            app.Resources["LanguageText"] = GetString("Language");
            app.Resources["ThemeText"] = GetString("ThemeText");
        }

        public string CurrentLanguage => _currentCulture.TwoLetterISOLanguageName;
        public CultureInfo CurrentCulture => _currentCulture;
    }
}