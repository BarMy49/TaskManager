using System.Collections.ObjectModel;
using System.Windows;
using TaskManager.Controller;
using TaskManager.View;
using System.Windows.Input;
using TaskManager.Localization;

namespace WpfTaskManager
{
    public partial class MainWindow : Window
    {
        private WpfView? view;
        private TaskController? controller;
        private readonly ILocalizer _localizer;

        public MainWindow(ILocalizer localizer)
        {
            InitializeComponent();
            _localizer = localizer;
            
        }

        public void SetView(WpfView view, TaskController controller)
        {
            this.controller = controller;
            DataContext = this.view = view;
        }
        
        public void RefreshColumnHeaders()
        {
            var loc = (ResourceLocalizer)_localizer;
            // ID nie tłumaczymy
            TasksGrid.Columns[1].Header = Application.Current.Resources["TitleText"];
            TasksGrid.Columns[2].Header = Application.Current.Resources["DescriptionText"];
            TasksGrid.Columns[3].Header = Application.Current.Resources["StatusText"];
            TasksGrid.Columns[4].Header = Application.Current.Resources["CategoryText"];
            TasksGrid.Columns[5].Header = Application.Current.Resources["PriorityText"];
            TasksGrid.Columns[6].Header = Application.Current.Resources["DueDateText"];
        }
        
        public void RefreshControllerBindings()
        {
            // Ponownie ustaw DataContext
            DataContext = view;
    
            // Uaktualnij bindowania kontrolek
            if (TasksGrid != null)
            {
                TasksGrid.ItemsSource = null;
                TasksGrid.ItemsSource = view?.Records;
            }
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            view?.SetKey(System.ConsoleKey.UpArrow);
        }

        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            view?.SetKey(System.ConsoleKey.DownArrow);
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            view?.SetKey(System.ConsoleKey.Enter);
        }

        private void ListAllTasks_Click(object sender, RoutedEventArgs e)
        {
            view.Ignore = true;
            controller?.ListAllTasks();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            view.DeleteVisibility = Visibility.Collapsed;
            view.ToggleVisibility = Visibility.Collapsed;
            view.EditVisibility = Visibility.Collapsed;
            view.CategoryFilterVisibility = Visibility.Collapsed;

            view.FieldsVisibility = Visibility.Visible;
            view.AddConfirmVisibility = Visibility.Visible;
            view.EditConfirmVisibility = Visibility.Collapsed;

            view.ClearTaskFields();

            view.Ignore = true;
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            view.DeleteVisibility = Visibility.Collapsed;
            view.ToggleVisibility = Visibility.Collapsed;
            view.CategoryFilterVisibility = Visibility.Collapsed;

            view.FieldsVisibility = Visibility.Visible;
            view.AddConfirmVisibility = Visibility.Collapsed;
            view.EditConfirmVisibility = Visibility.Visible;

            view.EditVisibility = Visibility.Visible;
            view.EditId = null;

            view.ClearTaskFields();

            view.Ignore = true;
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            view.FieldsVisibility = Visibility.Collapsed;
            view.AddConfirmVisibility = Visibility.Collapsed;
            view.EditConfirmVisibility = Visibility.Collapsed;

            view.ToggleVisibility = Visibility.Collapsed;
            view.EditVisibility = Visibility.Collapsed;
            view.CategoryFilterVisibility = Visibility.Collapsed;

            view.DeleteVisibility = Visibility.Visible;
            view.DeleteId = null;

            view.Ignore = true;
        }

        private void ToggleTaskCompletion_Click(object sender, RoutedEventArgs e)
        {
            view.FieldsVisibility = Visibility.Collapsed;
            view.AddConfirmVisibility = Visibility.Collapsed;
            view.EditConfirmVisibility = Visibility.Collapsed;
            view.DeleteVisibility = Visibility.Collapsed;
            view.EditVisibility = Visibility.Collapsed;
            view.CategoryFilterVisibility = Visibility.Collapsed;

            view.ToggleVisibility = Visibility.Visible;
            view.ToggleId = null;

            view.Ignore = true;
        }
        
        private void FilterTasksByCategory_Click(object sender, RoutedEventArgs e)
        {
            // Sneaky inne formularze
            view.DeleteVisibility = Visibility.Collapsed;
            view.ToggleVisibility = Visibility.Collapsed;
            view.EditVisibility = Visibility.Collapsed;
            view.FieldsVisibility = Visibility.Collapsed;
            view.AddConfirmVisibility = Visibility.Collapsed;
            view.EditConfirmVisibility = Visibility.Collapsed;

            // Nowy border do filtra kategorii
            view.CategoryFilterVisibility = Visibility.Visible;
            view.CategoryFilterText = null; 

            view.Ignore = true;
        }

        private void ListIncompleteTasks_Click(object sender, RoutedEventArgs e)
        {
            view.Ignore = true;
            controller?.ListIncompleteTasks();
        }

        private void ListTasksByPriority_Click(object sender, RoutedEventArgs e)
        {
            view.Ignore = true;
            controller?.ListTasksByPriority();
        }

        private void SearchTasks_Click(object sender, RoutedEventArgs e)
        {
            view.Ignore = true;
            controller?.SearchTasks();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            view.Ignore = true;
            controller?.Exit();
        }

        private void DataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        // Obsługa Enter w polu "Szukaj" (u góry)
        private void SearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                if (view != null && controller != null)
                {
                    view.Ignore = true;
                    controller.SearchTasks();
                }
            }
        }

        // ============================ USUWANIE ============================
        private void DeleteId_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                DeleteItem();
            }
        }
        private void DeleteIdButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteItem();
        }
        private void DeleteItem()
        {
            if (view != null && controller != null)
            {
                view.Ignore = true;
                controller.DeleteTask();
                view.DeleteVisibility = Visibility.Collapsed;
                view.DeleteId = null;
            }
        }

        // ============================ TOGGLE ============================
        private void ToggleId_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                ToggleCompletion();
            }
        }
        private void ToggleIdButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleCompletion();
        }
        private void ToggleCompletion()
        {
            if (view != null && controller != null)
            {
                view.Ignore = true;
                controller.ToggleTaskCompletion();
                view.ToggleVisibility = Visibility.Collapsed;
                view.ToggleId = null;
            }
        }

        // ============================ EDYCJA ============================
        private void EditId_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                SearchById_Click(sender, e);
            }
        }
        private void SearchById_Click(object sender, RoutedEventArgs e)
        {
            if (view != null && controller != null)
            {
                view.Ignore = true;
                controller.SearchTaskById();
            }
        }
        private void AddConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (view != null && controller != null)
            {
                view.Ignore = true;
                controller.AddTask();

                view.FieldsVisibility = Visibility.Collapsed;
                view.AddConfirmVisibility = Visibility.Collapsed;
            }
        }
        private void EditConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (view != null && controller != null)
            {
                view.Ignore = true;
                controller.EditTask();

                view.FieldsVisibility = Visibility.Collapsed;
                view.EditConfirmVisibility = Visibility.Collapsed;
                view.EditVisibility = Visibility.Collapsed;
            }
        }

        // ============================ FILTR KATEGORII ============================
        // Obsługa Enter w polu "CategoryFilterText"
        private void CategoryFilterText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                FilterCategory();
            }
        }
        // Obsługa kliknięcia przycisku "Szukaj"
        private void CategoryFilterTextButton_Click(object sender, RoutedEventArgs e)
        {
            FilterCategory();
        }
        // Metoda wywołująca logikę filtra w kontrolerze
        private void FilterCategory()
        {
            if (view != null && controller != null)
            {
                view.Ignore = true;
                controller.FilterTasks(0);

                view.CategoryFilterVisibility = Visibility.Collapsed;
                view.CategoryFilterText = null;
            }
        }
    }
}
